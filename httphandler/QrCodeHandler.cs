using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace httphandler
{
    /// <summary>
    /// ASP.NET Http Handler for QRCode Generation
    /// </summary>
    public class QrCodeHandler : IHttpAsyncHandler
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
            try
            {
                var url = context.Request["url"];
                if (string.IsNullOrWhiteSpace(url) == false)
                {
                    var encoder = new QrEncoder(ErrorCorrectionLevel.L);
                    var qr = new QrCode();
                    if (encoder.TryEncode(url, out qr) == true)
                    {
                        using (var ms = new MemoryStream { })
                        {
                            var render = new GraphicsRenderer(new FixedModuleSize(10, QuietZoneModules.Two));
                            render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                            context.Response.BinaryWrite(ms.GetBuffer());
                        }

                        context.Response.ContentType = "image/png";
                        context.Response.Flush();
                        context.Response.End();
                    }
                }
            }
            catch
            {
                context.Response.Clear();
                context.Response.ContentType = "image/png";
                context.Response.StatusCode = 404;
            }
        }
    }
}
