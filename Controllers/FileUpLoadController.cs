using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UPLOADING_FILE.Context;
using UPLOADING_FILE.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace UPLOADING_FILE.Controllers
{
    
    public class FileUpLoadController : Controller
    {
        private readonly ApplicationContext context;

        public FileUpLoadController(ApplicationContext context1)
        {
            context = context1;
        }

        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }
    private async Task<FileUploadViewModel> LoadAllFiles()
    {
        var viewModel = new FileUploadViewModel();
        viewModel.FileOnDataBaseModel = await context.FileOnDataBaseModel.ToListAsync();
        viewModel.FileOnFileSystem = await context.FileOnFileSystem.ToListAsync();
        return viewModel;
    }
[HttpPost]
public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
{
    foreach(var file in files)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
        bool basePathExists = System.IO.Directory.Exists(basePath);
        if (!basePathExists) Directory.CreateDirectory(basePath);
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var filePath = Path.Combine(basePath, file.FileName);
        var extension = Path.GetExtension(file.FileName);
        if (!System.IO.File.Exists(filePath))
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var fileModel = new FileOnFileSystem()
            {
                CreatedOn = DateTime.UtcNow,
                FileType = file.ContentType,
                prolongation = extension,
                Name = fileName,
                Description = description,
                FilePath = filePath
            };
            context.FileOnFileSystem.Add(fileModel);
            context.SaveChanges();
        }
    }

    TempData["Message"] = "File successfully uploaded to File System.";
    return RedirectToAction("Index");
}

public async Task<IActionResult> DownloadFileFromFileSystem(int id)
{

    var file = await context.FileOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
    if (file == null) return null;
    var memory = new MemoryStream();
    using (var stream = new FileStream(file.FilePath, FileMode.Open))
    {
        await stream.CopyToAsync(memory);
    }
    memory.Position = 0;
    return File(memory, file.FileType, file.Name + file.prolongation);
}

public async Task<IActionResult> DeleteFileFromFileSystem(int id)
{

    var file = await context.FileOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
    if (file == null) return null;
    if (System.IO.File.Exists(file.FilePath))
    {
        System.IO.File.Delete(file.FilePath);
    }
    context.FileOnFileSystem.Remove(file);
    context.SaveChanges();
    TempData["Message"] = $"Removed {file.Name + file.prolongation} successfully from File System.";
    return RedirectToAction("Index");
}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}