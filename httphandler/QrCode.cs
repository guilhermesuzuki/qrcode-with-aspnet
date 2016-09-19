using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace httphandler
{
    /// <summary>
    /// ASP.NET Http Handler for QRCode Generation
    /// </summary>
    public class QrCode : IHttpAsyncHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// async delegate
        /// </summary>
        private Action<HttpContext> asyncDelegate;

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            asyncDelegate = new Action<HttpContext>(this.ProcessRequest);
            return asyncDelegate.BeginInvoke(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            asyncDelegate.EndInvoke(result);
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
