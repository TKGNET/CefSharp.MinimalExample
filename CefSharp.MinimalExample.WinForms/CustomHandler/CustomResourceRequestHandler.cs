using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.WinForms.CustomHandler
{
    class CustomResourceRequestHandler : ResourceRequestHandler
    {
        // dictionary to store pair of requestID , CustomResponseFilter
        private readonly Dictionary<UInt64, MemoryStreamResponseFilter> responseDictionary = new Dictionary<UInt64, MemoryStreamResponseFilter>();

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var url = new Uri(request.Url);

            // comment / uncomment to filter specific hosts or content-types
            if (url.Host != "www.google.com") return null;
           // if (response.Headers["content-type"] != "image/png") return null;

            Console.WriteLine("");
            Console.WriteLine($"RequestID {request.Identifier} {this.GetType().Name}->{MethodBase.GetCurrentMethod().Name}: method: {request.Method} content-type: {response.Headers["content-type"]}" +
                $" url: {request.Url}"); 
              
            // get 
            var dataFilter = new MemoryStreamResponseFilter(request.Identifier);
            responseDictionary.Add(request.Identifier, dataFilter);

            return dataFilter;
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            MemoryStreamResponseFilter filter;
            // get filter back from dictionary
            if (responseDictionary.TryGetValue(request.Identifier, out filter))
            {
                // PROBLEM: FILTER.DISPOSE HAS ALREADY BEEN CALLED AND DATA CANT BE RETRIEVED
                Console.WriteLine($"RequestID {request.Identifier} {this.GetType().Name}->{MethodBase.GetCurrentMethod().Name}: IFilter.isDisposed: {filter.isDisposed}");
            }
        }
    }
}