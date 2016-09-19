using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new string[] { "www.uol.com.br", "http://cp24.com", "http://guilhermesuzuki.com" });
        }

        [HttpGet]
        public FileResult QrCode(string url)
        {
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
                        return File(ms.GetBuffer(), "image/png");
                    }
                }
            }

            return null;
        }
    }
}