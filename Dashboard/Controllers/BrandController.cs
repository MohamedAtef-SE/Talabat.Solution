using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Products;

namespace Route.Talabat.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BrandController(IUnitOfWork _unitOfWork) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, string>().GetAllAsync();

            return View(brands);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Create(BrandViewModel brandVM)
        {
            try
            {
                var brand = new ProductBrand()
                {
                    Name = brandVM.Name
                };
                await _unitOfWork.GetRepository<ProductBrand, string>().AddAsync(brand);
                var result = await _unitOfWork.CompleteAsync();
                if (!result)
                    return View("Index",brand);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "Please Enter new Name");
                return View("Index", await _unitOfWork.GetRepository<ProductBrand, string>().GetAllAsync());
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var brand = await _unitOfWork.GetRepository<ProductBrand, string>().GetAsync(id);
            if (brand is null)
                return NotFound();

            _unitOfWork.GetRepository<ProductBrand, string>().Delete(brand);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
    }
}