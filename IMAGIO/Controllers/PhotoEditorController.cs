using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using IMAGIO.Models;
using IMAGIO.ViewModels;
using SixLabors.ImageSharp.Processing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using IMAGIO.Data;
using IMAGIO.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.CodeAnalysis;
using IPlugin;
using System.Text.Json;
using NuGet.Protocol.Plugins;
using static IMAGIO.Controllers.PhotoEditorController;

namespace IMAGIO.Controllers
{
    public class PhotoEditorController : Controller
    {
        private IEnumerable<Iplugin> _plugins;
        private readonly IWebHostEnvironment webHostEnvironment;
        private DataContext db;

        public struct Plugins{
            public string iconSrc { get; set; }
            public string name { get; set; }
            public bool isPro { get; set; }
        }

        public PhotoEditorController(IWebHostEnvironment env, DataContext context, IEnumerable<Iplugin> plugins)
        {
            webHostEnvironment = env;
            db = context;
            _plugins = plugins;
        }

        // GET: PhotoEditorController

        public ActionResult Index(int idPhoto = 0, string finalImageSrc = "", string finalImageSrcPng = "", string finalImageSrcBmp = "")
        {
            if (idPhoto == 999)
            {
                ViewBag.DownloadSrc1 = finalImageSrc;
                ViewBag.DownloadSrc2 = finalImageSrcPng;
                ViewBag.DownloadSrc3 = finalImageSrcBmp;
                ViewBag.PhotoId = idPhoto;
                return View();
            }
            else
            {
                List<Plugins> plugins = new List<Plugins>();
                for (int i = 0; i < _plugins.Count(); i++)
                {
                    Plugins plugs = new Plugins();
                    plugs.iconSrc = _plugins.ToList()[i].IconSrc;
                    plugs.name = _plugins.ToList()[i].Name;
                    plugs.isPro = _plugins.ToList()[i].IsProVersion;

                    plugins.Add(plugs);
                }

                List<string> Photos = new List<string>();

                int userId = Convert.ToInt32(
                    db.Users
                    .Where(x => x.Name == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefault().ToString()
                    );

                string imageSrc =
                    db.Images
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Src).FirstOrDefault().ToString();

                int imageId = Convert.ToInt32(
                    db.Images
                    .Where(x => x.Src == imageSrc)
                    .Select(x => x.Id)
                    .FirstOrDefault().ToString()
                    );

                List<string> changeImages =
                    db.ImageTemps
                    .Where(x => x.ImageId == imageId)
                    .Select(x => x.Src)
                    .ToList();

                foreach (string src in changeImages)
                {
                    Photos.Add(src);
                }

                ViewBag.Plugins = plugins;
                ViewBag.PhotoSrc = Photos;
                ViewBag.PhotoId = idPhoto;

                return View();
            }
        }

        [HttpPost]
        public IActionResult SaveChanges(string imgSrc, int count, int id)
        {
            string photoSrcTEMP;
            photoSrcTEMP = "..\\IMAGIO\\wwwroot\\";
            photoSrcTEMP += imgSrc;
            photoSrcTEMP.Replace("/", "\\");

            string photoSrc;
            photoSrc = $"..\\IMAGIO\\wwwroot\\ClientPhotos\\{User.Identity.Name}\\ChangePhotos\\";
            if (!Directory.Exists(photoSrc))
                Directory.CreateDirectory(photoSrc);

            photoSrc += imgSrc.Split("/").Last().Split(".").First();
            photoSrc.Replace("/", "\\");

            using (SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(photoSrcTEMP))
            {
                img.Save(photoSrc + ".jpg");
                img.Save(photoSrc + ".png");
                img.Save(photoSrc + ".bmp");
            }

            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );
            string changeLog = db.ImageTemps
                               .Where(x => x.Src == imgSrc)
                               .Select(x => x.ChangeName)
                               .FirstOrDefault().ToString();

            db.FinalsImages.Add(new Entities.FinalImage { Src = $"ClientPhotos/{User.Identity.Name}/ChangePhotos/" + (imgSrc.Split("/").Last()).Split(".")[0] + "." + (imgSrc.Split("/").Last()).Split(".")[1], Changes = changeLog, UserId = userId, DateTimes = DateTime.Now});
            db.SaveChanges();

            int imageId = Convert.ToInt32(
                db.Images
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            for (int i = 0; i < count; i++)
            {
                int imagechanchgeid = Convert.ToInt32(db.ImageTemps
                                                    .Where(x => x.ImageId == imageId)
                                                    .Select(x => x.Id)
                                                    .ToList()
                                                    .LastOrDefault());
                ImageTemp? temp = db.ImageTemps.FirstOrDefault(p => p.Id == imagechanchgeid);
                if (temp != null)
                {
                    db.ImageTemps.Remove(temp);
                    db.SaveChanges();
                }
            }


            Entities.Image? tempImage = db.Images.FirstOrDefault(p => p.Id == imageId);

            if (tempImage != null)
            {
                db.Images.Remove(tempImage);
                db.SaveChanges();
            }

            string fullUpdoadPath = webHostEnvironment.WebRootPath + $@"\ClientPhotos\{User.Identity.Name}\Uploads";

            DirectoryInfo dirInfo = new DirectoryInfo(fullUpdoadPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }

            if (Directory.Exists(fullUpdoadPath))
                Directory.Delete(fullUpdoadPath);

            List<string> finalImageSrcs = new List<string>();
            finalImageSrcs.Add($"ClientPhotos/{User.Identity.Name}/ChangePhotos/" + (imgSrc.Split("/").Last()).Split(".")[0] + "." + "jpg");
            finalImageSrcs.Add($"ClientPhotos/{User.Identity.Name}/ChangePhotos/" + (imgSrc.Split("/").Last()).Split(".")[0] + "." + "png");
            finalImageSrcs.Add($"ClientPhotos/{User.Identity.Name}/ChangePhotos/" + (imgSrc.Split("/").Last()).Split(".")[0] + "." + "bmp");

            return RedirectToAction("Index", new { idPhoto = id , finalImageSrc = finalImageSrcs[0], finalImageSrcPng = finalImageSrcs[1], finalImageSrcBmp = finalImageSrcs[2] });
        }

        [HttpPost]
        public IActionResult DeletePhotos(int count)
        {
            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            int imageId = Convert.ToInt32(
                db.Images
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                {

                    int imagechanchgeid = Convert.ToInt32(db.ImageTemps
                        .Where(x => x.ImageId == imageId)
                        .Select(x => x.Id)
                        .ToList()
                        .LastOrDefault());
                    string imagechanchgesrc = db.ImageTemps
                        .Where(x => x.ImageId == imageId)
                        .Select(x => x.Src)
                        .ToList()
                        .LastOrDefault();

                    ImageTemp? temp = db.ImageTemps.FirstOrDefault(p => p.Id == imagechanchgeid);
                    if (temp != null)
                    {
                        db.ImageTemps.Remove(temp);
                        db.SaveChanges();
                    }

                    string fullUpdoadPhotoPath = webHostEnvironment.WebRootPath;
                    fullUpdoadPhotoPath = fullUpdoadPhotoPath + "/" + imagechanchgesrc;
                    FileInfo fileInfo = new FileInfo(fullUpdoadPhotoPath);
                    fileInfo.Delete();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeletePhoto(string imgSrc, int id)
        {
            int imageId = Convert.ToInt32(
                db.ImageTemps
                .Where(x => x.Src == imgSrc)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            string fullUpdoadPhotoPath = webHostEnvironment.WebRootPath;
            fullUpdoadPhotoPath = fullUpdoadPhotoPath + "/" + imgSrc;
            FileInfo fileInfo = new FileInfo(fullUpdoadPhotoPath);
            fileInfo.Delete();

            ImageTemp? temp = db.ImageTemps.FirstOrDefault(p => p.Id == imageId);

            if (temp != null)
            {
                if (id == 0)
                {
                    int userId = Convert.ToInt32(
                        db.Users
                        .Where(x => x.Name == User.Identity.Name)
                        .Select(x => x.Id)
                        .FirstOrDefault().ToString()
                    );


                    Entities.Image? originalImage = db.Images.FirstOrDefault(p => p.UserId == userId);

                    if (originalImage != null)
                    {
                        db.Images.Remove(originalImage);
                        db.SaveChanges();
                        return RedirectToAction("Index", new { idPhoto = 999 });
                    }
                }
                else
                {
                    db.ImageTemps.Remove(temp);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idPhoto = id - 1 });
                } 
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult SelectedPhoto(int idImg)
        {
            return RedirectToAction("Index", new { idPhoto = idImg});
        }

        [HttpPost]
        public IActionResult EditPhoto(string src, int id, string plugName, int width = 0, int height = 0, float valueRange = 0, int filterValue = 0) 
        {
            string photoSrc;
            photoSrc = "..\\IMAGIO\\wwwroot\\";
            photoSrc += src;
            photoSrc.Replace("/", "\\");

            string plugWorkString = "";

            

            for (int i = 0; i < _plugins.Count(); i++)
            {
                if (_plugins.ToList()[i].Name == plugName)
                {
                    plugWorkString = _plugins.ToList()[i].Function(photoSrc, id, width, height, valueRange / 100, filterValue, 0);
                }
            }

            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            int imageId = Convert.ToInt32(
                db.Images
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            string changename = db.ImageTemps
                                .Where(x => x.ImageId == imageId)
                                .Select(x => x.ChangeName)
                                .ToList().LastOrDefault().ToString();

            db.ImageTemps.Add(new Entities.ImageTemp { Src = src.Split(".").First() + $"_{id}" + "." + src.Split(".").Last(), ImageId = imageId, ChangeName = changename + plugWorkString });
            db.SaveChanges();

            return RedirectToAction("Index", new { idPhoto = id}); 
        }

        [HttpPost]
        public IActionResult UploadDrawingImage([FromBody] ImageDataModel data)
        {

            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            int imageId = Convert.ToInt32(
                db.Images
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            string src = db.ImageTemps
                                .Where(x => x.ImageId == imageId)
                                .Select(x => x.Src)
                                .ToList().LastOrDefault().ToString();

            string changename = db.ImageTemps
                                .Where(x => x.ImageId == imageId)
                                .Select(x => x.ChangeName)
                                .ToList().LastOrDefault().ToString();

            string photoSrc;
            photoSrc = "";
            int id = 1;

            string src_img = src.Split(".").First();
            string src_img_format = src.Split(".").Last();

            Console.WriteLine(src_img);

            if (src_img[src_img.Length - 2] != '_')
            {
                src_img += '_';
                src_img += id.ToString();
                photoSrc += src_img;
                photoSrc += '.';
                photoSrc += src_img_format;
                photoSrc.Replace("/", "\\");
            }
            else
            {
                id = int.Parse(Convert.ToString(src_img[src_img.Length - 1])) + 1;
                photoSrc += src_img;
                photoSrc += '_';
                photoSrc += id.ToString();
                photoSrc += '.';
                photoSrc += src_img_format;
                photoSrc.Replace("/", "\\");
            }

            Console.WriteLine(photoSrc);

            byte[] imageBytes = Convert.FromBase64String(data.Image.Split(',')[1]);

            string imagePath = Path.Combine("wwwroot", "ClientPhotos", User.Identity.Name, "Uploads" , photoSrc.Split('/').Last());
            System.IO.File.WriteAllBytes(imagePath, imageBytes);

            db.ImageTemps.Add(new Entities.ImageTemp { Src = photoSrc, ImageId = imageId, ChangeName = changename + "drawing" });
            db.SaveChanges();

            string redirectUrl = Url.Action("Index", new { idPhoto = id });
            return Json(new { redirectTo = redirectUrl });
        }

        public class ImageDataModel
        {
            public string Image { get; set; }
        }

    }
}
