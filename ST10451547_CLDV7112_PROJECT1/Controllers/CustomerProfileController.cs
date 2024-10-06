using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using ST10451547_CLDV7112_PROJECT1.BusinessLogic;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1.Controllers
{
    public class CustomerProfileController : Controller
    {
        private readonly CustomerProfileService _customerProfileService;
		private const string ContainerName = "clouddevelopementproject1";
		public const string SuccessMessageKey = "SuccessMessage";
		public const string ErrorMessageKey = "ErrorMessage";

		private readonly BlobServiceClient _blobServiceClient;
		private readonly BlobContainerClient _containerClient;

		public CustomerProfileController(CustomerProfileService customerProfileService, BlobServiceClient blobServiceClient)
        {
            _customerProfileService = customerProfileService;
			_blobServiceClient = blobServiceClient;
			_containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
			_containerClient.CreateIfNotExists();
		}

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var products = await _customerProfileService.GetCustomerProfilesAsync(cancellationToken);

            return View(products.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerProfile customerProfile)
        {

            if (customerProfile is null)
                return BadRequest("A customer Details must be present");

            await _customerProfileService.AddCustomerProfileAsync(customerProfile);
			Upload(customerProfile?.Upload);
            return RedirectToAction(nameof(Create));
        }

		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			try
			{
				var blobClient = _containerClient.GetBlobClient(file.FileName);
				await blobClient.UploadAsync(file.OpenReadStream(), true);
				TempData[SuccessMessageKey] = "File uploaded successfully.";
			}
			catch (Exception ex)
			{
				TempData[ErrorMessageKey] = "An error occurred while uploading the file: " + ex.Message;
				return View();
			}

			return RedirectToAction("Index");
		}
	}
}
