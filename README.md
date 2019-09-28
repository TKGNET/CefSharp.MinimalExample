# CefSharp.MinimalExample

Demo for CefSharpIssue[#2903]: Custom ResourceHandler Unable to Load Image

This demo creates custom RequestHandler / ResourceResponseHandler // MemoryStreamResponseFilter to access response payload in CefWinforms
They are simplified versions from CefSharp master: ExampleRequestHandler / ExampleRequestResourceHandler / MemoryStreamResponseFilter

Goal is to access the response payload by copying the responseStream a memoryStream and finalizing it in the LoadComplete method.

This has worked in previous versions of CefSharp, but not in this tested version 75.

Reason: Currently MemoryStreamResponseFilter.Dispose is always called IMMEDIATELY before IRequestResourceHandler.LoadCompleted
and the memoryStream is set to null.

