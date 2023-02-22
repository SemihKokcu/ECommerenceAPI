﻿using ECommerenceAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerenceAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                // IDısposible olduğu için using ile verdik
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;

            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }

        public async Task DeleteAsync(string path, string fileName)
        {
            File.Delete($"{path}/{fileName}");
        }

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return  directory.GetFiles().Select(f=>f.Name).ToList();

        }

        public bool HasFile(string path, string fileName)
        {
           return File.Exists($"{path}{fileName}");
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!File.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                //string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
                bool result = await CopyFileAsync($"{uploadPath}\\{file.Name}", file);
                datas.Add((file.Name, $"{path}\\{file.Name}"));
            }
   
            return datas;
        }
    }
}
