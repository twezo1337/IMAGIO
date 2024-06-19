using IMAGIO.Data;
using IMAGIO.Entities;
using IPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using static IMAGIO.Controllers.PhotoEditorController;

namespace IMAGIO.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private DataContext db;
        // GET: HistoryController

        public struct HistoryPhoto
        {
            public string imgSrc { get; set; }
            public string changes { get; set; }
            public string date { get; set; }
        }

        public HistoryController(IWebHostEnvironment env, DataContext context, IEnumerable<Iplugin> plugins)
        {
            webHostEnvironment = env;
            db = context;
        }

        public ActionResult Index()
        {
            List<HistoryPhoto> historyPhotos = new List<HistoryPhoto>();

            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );
            List<FinalImage> finalImages = db.FinalsImages.Where(x => x.UserId == userId).ToList();

            for (int i = 0; i < finalImages.Count; i++)
            {
                HistoryPhoto hf = new HistoryPhoto();
                hf.imgSrc = finalImages[i].Src;
                hf.changes = finalImages[i].Changes;
                hf.date = finalImages[i].DateTimes.Date.ToString().Split(" ").First();
                historyPhotos.Add(hf);
            }
            
            ViewBag.PhotoHistory = historyPhotos;

            return View();
        }
        public ActionResult DeleteCard(string imgSrc)
        {
            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            int imageId = Convert.ToInt32(
                db.FinalsImages
                .Where(x => x.UserId == userId).Where(x => x.Src == imgSrc)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            string pngFile = imgSrc.Split("/")[0] + "/" + imgSrc.Split("/")[1] + "/" + imgSrc.Split("/")[2] + "/" + imgSrc.Split("/").Last().Split(".").First() + ".png";
            string bmpFile = imgSrc.Split("/")[0] + "/" + imgSrc.Split("/")[1] + "/" + imgSrc.Split("/")[2] + "/" + imgSrc.Split("/").Last().Split(".").First() + ".bmp";
            string fullUpdoadPhotoPathJpg = webHostEnvironment.WebRootPath;
            string fullUpdoadPhotoPathPng = webHostEnvironment.WebRootPath;
            string fullUpdoadPhotoPathBmp = webHostEnvironment.WebRootPath;
            fullUpdoadPhotoPathJpg = fullUpdoadPhotoPathJpg + "/" + imgSrc;
            fullUpdoadPhotoPathPng = fullUpdoadPhotoPathPng + "/" + pngFile;
            fullUpdoadPhotoPathBmp = fullUpdoadPhotoPathBmp + "/" + bmpFile;

            FileInfo fileInfo1 = new FileInfo(fullUpdoadPhotoPathJpg);
            fileInfo1.Delete();
            FileInfo fileInfo2 = new FileInfo(fullUpdoadPhotoPathPng);
            fileInfo2.Delete();
            FileInfo fileInfo3 = new FileInfo(fullUpdoadPhotoPathBmp);
            fileInfo3.Delete();

            FinalImage? temp = db.FinalsImages.FirstOrDefault(p => p.Id == imageId);

            if (temp != null)
            {
                db.FinalsImages.Remove(temp);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
