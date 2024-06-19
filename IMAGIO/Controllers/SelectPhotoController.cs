using IMAGIO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IMAGIO.Data;
using IMAGIO.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using NuGet.Versioning;
using Microsoft.EntityFrameworkCore;

namespace IMAGIO.Controllers
{
    public class SelectPhotoController : Controller
    {
        private IWebHostEnvironment hostEnv;
        private DataContext db;

        public SelectPhotoController(IWebHostEnvironment env, DataContext context)
        {
            hostEnv = env;
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
         {
            var fileDic = "";

            if (User.Identity.IsAuthenticated)
            {
                fileDic = $"ClientPhotos/{User.Identity.Name}/Uploads";
            }
            else
            {
                fileDic = $"ClientPhotos/Guest/Uploads";
            }


            string filePath = Path.Combine(hostEnv.WebRootPath, fileDic);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            var fileName = file.FileName;
            filePath = Path.Combine(filePath, fileName);

            int userId = Convert.ToInt32(
                db.Users
                .Where(x => x.Name == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            db.Images.Add(new Entities.Image { Src = fileDic + "/" + fileName, UserId = userId });
            db.SaveChanges();

            int imageId = Convert.ToInt32(
                db.Images
                .Where(x => x.Src == fileDic + "/" + fileName)
                .Select(x => x.Id)
                .FirstOrDefault().ToString()
                );

            db.ImageTemps.Add(new Entities.ImageTemp { Src = fileDic + "/" + fileName, ImageId = imageId, ChangeName = "Original " });
            db.SaveChanges();

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
