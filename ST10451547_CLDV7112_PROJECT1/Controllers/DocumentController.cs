using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ST10451547_CLDV7112_PROJECT1.Controllers
{
    public class DocumentController : Controller
    {
        private const string ContainerName = "clouddevelopementproject1";
        public const string SuccessMessageKey = "SuccessMessage";
        public const string ErrorMessageKey = "ErrorMessage";

        private readonly BlobServiceClient _blobServiceClient;

        public DocumentController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadDocuments()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(Document document)
        {
            if (document.Upload == null || document.Upload.Length == 0)
            {
                TempData[ErrorMessageKey] = "Please select a file to upload.";
                return RedirectToAction("Index");
            }

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(document.Upload.FileName);
                await blobClient.UploadAsync(document.Upload.OpenReadStream(), true);
                TempData[SuccessMessageKey] = "File uploaded successfully.";
            }
            catch (Exception ex)
            {
                TempData[ErrorMessageKey] = "An error occurred while uploading the file: " + ex.Message;
                return RedirectToAction(nameof(GetData));
            }

            return RedirectToAction(nameof(GetData));
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var blobs = new List<string>();
            var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            return View(blobs);
        }

        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                using var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;
                var contentType = (await blobClient.GetPropertiesAsync()).Value.ContentType;

                return File(memoryStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData[ErrorMessageKey] = "An error occurred while downloading the file: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
                TempData[SuccessMessageKey] = "File deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData[ErrorMessageKey] = "An error occurred while deleting the file: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
