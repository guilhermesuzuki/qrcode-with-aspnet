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
            string version = string.Empty;
            for (int i = 0; i <= 500; i++) version = string.Concat(version, "1");

            return View(new string[] {
                "www.uol.com.br",
                "http://cp24.com",
                "http://guilhermesuzuki.com",
                "http://guilhermesuzuki.com/posts/details/a9821d1c-19f6-41a4-aab1-63fef4aa7d7b",
                $"http://guilhermesuzuki.com/posts/details/a9821d1c-19f6-41a4-aab1-63fef4aa7d7b?v={version}",
            });
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