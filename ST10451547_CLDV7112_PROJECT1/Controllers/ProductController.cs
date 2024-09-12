using Microsoft.AspNetCore.Mvc;
using ST10451547_CLDV7112_PROJECT1.BusinessLogic;
using ST10451547_CLDV7112_PROJECT1.Data;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDataStore _dataStore;
        private readonly CustomerProfileController _customerProfileController;
        public ProductController(IDataStore dataStore, CustomerProfileController customerProfileController)
        {
            _dataStore = dataStore;
            _customerProfileController = customerProfileController;
        }

        public async Task<IActionResult> Index()
        {
            await _dataStore.GetProductAsync(Guid.NewGuid());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            if (product is null)
                return BadRequest("A customer Details must be present");

            await _dataStore.SaveProductAsync(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
