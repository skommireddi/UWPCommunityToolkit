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
using System.Security;
using System.Security.Permissions;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.PartialTrust
{
    [TestClass]
    public class PartialTrustTest : ContextSpecification
    {
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void UnableToCreateInstance()
        {
            MediumTrustContext.Create<ClassUnderTest>();
        }

        internal static class MediumTrustContext
        {
            public static T Create<T>()
            {
                var appDomain = CreatePartialTrustDomain();
                try
                {
                    var t = (T) appDomain.CreateInstanceAndUnwrap(typeof(T).Assembly.FullName, typeof(T).FullName);
                    return t;
                }
                catch (Exception e)
                {
                    // Getting a reflection exception since that is how the type is activated
                    // Interested in the inner-most exception; okay with blowing away the stack for this test
                    var e2 = e;
                    while (e2.InnerException != null)
                    {
                        e2 = e2.InnerException;
                    }

                    throw e2;
                }
            }

            public static AppDomain CreatePartialTrustDomain()
            {
                var setup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };
                var permissions = new PermissionSet(null);
                permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
                return AppDomain.CreateDomain("Partial Trust AppDomain: " + DateTime.Now.Ticks, null, setup, permissions);
            }
        }

    }

    [Serializable]
    public class ClassUnderTest : MarshalByRefObject
    {
        private readonly Context _context;

        public ClassUnderTest()
        {
            _context = new Context();
        }

        public void Navigate()
        {
            _context.Navigate();
        }

        private class Context : HostFormWebViewContextSpecification
        {
            public void Navigate()
            {
                base.Given();
                base.When();
                base.NavigateAndWaitForFormClose(TestConstants.Uris.AboutBlank);
            }


        }
    }
}
