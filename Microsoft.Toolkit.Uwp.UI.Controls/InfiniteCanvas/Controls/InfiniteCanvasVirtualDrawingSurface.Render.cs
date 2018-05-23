﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Graphics;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The virtual Drawing surface renderer used to render the ink and text.
    /// </summary>
    internal partial class InfiniteCanvasVirtualDrawingSurface
    {
        private readonly List<IDrawable> _visibleList = new List<IDrawable>();
        private readonly List<IDrawable> _drawableList = new List<IDrawable>();

        public void ReDraw(Rect viewPort)
        {
            _visibleList.Clear();
            double top = double.MaxValue,
                   bottom = double.MinValue,
                   left = double.MaxValue,
                   right = double.MinValue;

            foreach (var drawable in _drawableList)
            {
                if (drawable.IsVisible(viewPort))
                {
                    _visibleList.Add(drawable);

                    bottom = Math.Max(drawable.Bounds.Bottom, bottom);
                    right = Math.Max(drawable.Bounds.Right, right);
                    top = Math.Min(drawable.Bounds.Top, top);
                    left = Math.Min(drawable.Bounds.Left, left);
                }
            }

            Rect toDraw;
            if (_visibleList.Any())
            {
                toDraw = new Rect(Math.Max(left, 0), Math.Max(top, 0), Math.Max(right - left, 0), Math.Max(bottom - top, 0));

                toDraw.Union(viewPort);
            }
            else
            {
                toDraw = viewPort;
            }

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(_drawingSurface, toDraw))
            {
                drawingSession.Clear(Colors.White);
                foreach (var drawable in _visibleList)
                {
                    drawable.Draw(drawingSession, toDraw);
                }
            }
        }

        public void ClearAll(Rect viewPort)
        {
            _visibleList.Clear();
            ExecuteClearAll();
            _drawingSurface.Trim(new RectInt32[0]);
        }

        public string GetSerializedList()
        {
            return JsonConvert.SerializeObject(_drawableList, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public void RenderFromJsonAndDraw(Rect viewPort, string json)
        {
            _visibleList.Clear();
            _drawableList.Clear();
            _undoCommands.Clear();
            _redoCommands.Clear();

            var newList = JsonConvert.DeserializeObject<List<IDrawable>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            foreach (var drawable in newList)
            {
                _drawableList.Add(drawable);
            }

            ReDraw(viewPort);
        }
    }
}
