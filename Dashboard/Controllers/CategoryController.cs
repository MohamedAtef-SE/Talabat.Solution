using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Products;

namespace Route.Talabat.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController(IUnitOfWork _unitOfWork) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.GetRepository<ProductCategory, string>().GetAllAsync();

            return View(brands);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Create(CategoryViewModel catVM)
        {
            try
            {
                var category = new ProductCategory()
                {
                    Name = catVM.Name
                };
                await _unitOfWork.GetRepository<ProductCategory, string>().AddAsync(category);
                var result = await _unitOfWork.CompleteAsync();
                if (!result)
                    return View("Index",category);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "Please Enter new Name");
                return View("Index", await _unitOfWork.GetRepository<ProductCategory, string>().GetAllAsync());
            }
        }


        public async Task<IActionResult> Delete(string id)
        {
            var category = await _unitOfWork.GetRepository<ProductCategory, string>().GetAsync(id);
            if (category is null)
                return NotFound();

            _unitOfWork.GetRepository<ProductCategory, string>().Delete(category);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
    }
}