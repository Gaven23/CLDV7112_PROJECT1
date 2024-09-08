using Microsoft.AspNetCore.Mvc;
using ST10451547_CLDV7112_PROJECT1.BusinessLogic;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1.Controllers
{
    public class CustomerProfileController : Controller
    {
        private readonly CustomerProfileService _customerProfileService;
        public CustomerProfileController(CustomerProfileService customerProfileService)
        {
            _customerProfileService = customerProfileService;
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

            return RedirectToAction(nameof(Index));
        }
    }
}
