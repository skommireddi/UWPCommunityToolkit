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
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace Microsoft.Toolkit.Win32.Samples.WinForms.WebView
{
    public partial class Form1 : Form
    {
        private bool _isFullScreen;

        public Form1()
        {
            InitializeComponent();
        }

        private void webView1_ContainsFullScreenElementChanged(object sender, object e)
        {
            void EnterFullScreen()
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            void LeaveFullScreen()
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }

            // Toggle
            _isFullScreen = !_isFullScreen;

            if (_isFullScreen)
            {
                EnterFullScreen();
            }
            else
            {
                LeaveFullScreen();
            }
        }

        private void webView1_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            url.Text = e.Uri?.ToString() ?? string.Empty;
            Text = webView1.DocumentTitle;
            if (!e.IsSuccess)
            {
                MessageBox.Show($"Could not navigate to {e.Uri}", $"Error: {e.WebErrorStatus}",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void webView1_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            Text = "Navigating " + e.Uri?.ToString() ?? string.Empty;
            url.Text = e.Uri?.ToString() ?? string.Empty;
        }

        private void webView1_PermissionRequested(object sender, WebViewControlPermissionRequestedEventArgs e)
        {
            if (e.PermissionRequest.State == WebViewControlPermissionState.Allow) return;

            var msg = $"Allow {e.PermissionRequest.Uri.Host} to access {e.PermissionRequest.PermissionType}?";

            var response = MessageBox.Show(msg, "Permission Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (response == DialogResult.Yes)
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Allow();
                }
                else
                {
                    e.PermissionRequest.Allow();
                }
            }
            else
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Deny();
                }
                else
                {
                    e.PermissionRequest.Deny();
                }
            }
        }

        private void webView1_ScriptNotify(object sender, WebViewControlScriptNotifyEventArgs e)
        {
            MessageBox.Show(e.Value, e.Uri?.ToString() ?? string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webView1?.GoBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webView1?.GoForward();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(url.Text);
            webView1.Source = result;
        }

        private void url_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && webView1 != null)
            {
                var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(url.Text);
                webView1.Source = result;
            }
        }
    }
}