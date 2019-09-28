using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Handler;

namespace CefSharp.MinimalExample.WinForms.CustomHandler
{
    class CustomRequestHandler:RequestHandler
    {
    protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            // return custom handler for any request
            return new CustomResourceRequestHandler();
        }
    }
}
