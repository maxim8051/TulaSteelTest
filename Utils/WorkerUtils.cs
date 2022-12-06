using Microsoft.AspNetCore.Hosting;
using MvcTest.Models;
using System.IO;
using System;
using System.Linq;

namespace MvcTest.Utils
{
    public class WorkerUtils
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };

        public WorkerUtils(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        //Загрузка фото
        public string UploadedFile(WorkerViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null & model.Photo.Length < 3145728 & CheckExt(model.Photo.FileName))
            {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fileStream);
                    }
            }
            return uniqueFileName;
        }

        //Получение пути до фотографии
        public string GetPathPhoto(string path)
        {
            return Path.Combine(webHostEnvironment.WebRootPath, "images", path);
        }

        //Проверка на разрешение файла 
        public bool CheckExt(string fileName)
        {
            string ext = fileName.Substring(fileName.LastIndexOf('.'));
            return permittedExtensions.Contains(ext);
        }
    }
}
