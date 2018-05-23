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
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the opacity behavior.
    /// </summary>
    public sealed partial class PeoplePickerPage : Page, IXamlRenderListener
    {
        private PeoplePicker peoplePickerControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeoplePickerPage"/> class.
        /// </summary>
        public PeoplePickerPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            peoplePickerControl = control.FindName("PeoplePicker") as PeoplePicker;
            if (peoplePickerControl != null)
            {
                peoplePickerControl.SelectionChanged += PeopleSelectionChanged;
            }
        }

        private async void PeopleSelectionChanged(object sender, PeopleSelectionChangedEventArgs e)
        {
            if (e.Selections != null)
            {
                await new MessageDialog($"Selected Person Counter {e.Selections.Count}", "Selection Changed").ShowAsync();
            }
        }
    }
}