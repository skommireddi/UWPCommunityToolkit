// ******************************************************************
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
using Microsoft.Graph;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Get icon of DriveItem
    /// </summary>
    internal class DriveItemIconConverter : IValueConverter
    {
        private static readonly string OfficeIcon = "https://static2.sharepointonline.com/files/fabric/assets/brand-icons/document/png/{0}_32x1_5.png";
        private static readonly string LocalIcon = "ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Graph/Assets/{0}";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DriveItem driveItem = value as DriveItem;

            if (driveItem.Folder != null)
            {
                return string.Format(LocalIcon, "folder.svg");
            }
            else if (driveItem.File != null)
            {
                if (driveItem.File.MimeType.StartsWith("image"))
                {
                    return string.Format(LocalIcon, "photo.png");
                }
                else if (driveItem.File.MimeType.StartsWith("application/vnd.openxmlformats-officedocument"))
                {
                    int index = driveItem.Name.LastIndexOf('.');
                    if (index != -1)
                    {
                        string ext = driveItem.Name.Substring(index + 1);
                        return string.Format(OfficeIcon, ext);
                    }
                }
            }
            else if (driveItem.Package != null)
            {
                switch (driveItem.Package.Type)
                {
                    case "oneNote":
                        return string.Format(OfficeIcon, "one");
                }
            }

            return string.Format(LocalIcon, "genericfile.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
