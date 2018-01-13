using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.Mvc;
using SD = System.Drawing;

namespace SignalRExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //public ActionResult SaveImage(object sender, EventArgs e)
        //{

        //    string serverPath = HttpContext.Current.Server.MapPath("~/");
        //    //path = serverPath + path;
        //    if (FileUpload1.HasFile)
        //    {
        //        string FileWithPat = serverPath + @"images/DP/" + UserName + FileUpload1.FileName;

        //        FileUpload1.SaveAs(FileWithPat);
        //        SD.Image img = SD.Image.FromFile(FileWithPat);
        //        SD.Image img1 = RezizeImage(img, 151, 150);
        //        img1.Save(FileWithPat);
        //        if (File.Exists(FileWithPat))
        //        {
        //            FileInfo fi = new FileInfo(FileWithPat);
        //            string ImageName = fi.Name;
        //            string query = "update tbl_Users set Photo='" + ImageName + "' where UserName='" + UserName + "'";
        //            if (ConnC.ExecuteQuery(query))
        //                UserImage = "images/DP/" + ImageName;
        //        }
        //    }
        //}


        #region Resize Image With Best Qaulity

        private SD.Image RezizeImage(SD.Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, SD.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    gr.Clear(Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new Rectangle(0, 0, nnx, nny),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        private MemoryStream BytearrayToStream(byte[] arr)
        {
            return new MemoryStream(arr, 0, arr.Length);
        }

        #endregion
    }
}