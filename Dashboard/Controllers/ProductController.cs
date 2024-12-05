using AutoMapper;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route.Talabat.Dashboard.Helpers;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Infrastructure.Persistence.Data;

namespace Route.Talabat.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController(StoreContext _dbContext, IUnitOfWork _unitOfWork, SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, IMapper _mapper, IHttpContextAccessor _httpContextAccessor) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.GetRepository<Product, string>().GetAllAsync();
            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel<string>>>(products);
            return View(mappedProducts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Create(ProductViewModel<string> productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                {
                    productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
                }
                else
                    productViewModel.PictureUrl = "assets/img/products/_Default.jpg";

                var mappedProduct = _mapper.Map<ProductViewModel<string>, Product>(productViewModel);

                await _unitOfWork.GetRepository<Product, string>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            else
                return View(productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var product = await _unitOfWork.GetRepository<Product, string>().GetAsync(Id);

            if (product is null) return View("Error");

            var mappedProduct = _mapper.Map<Product, ProductViewModel<string>>(product);
            return View(mappedProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Edit(ProductViewModel<string> productViewModel)
        {
            if (productViewModel is null) return NotFound();
            productViewModel.CreatedBy = await GetCreatedByAsync(productViewModel.Id);
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                {
                    if (productViewModel.PictureUrl != null)
                    {
                        PictureSettings.DeleteFile(productViewModel.PictureUrl, "products");
                    }

                    productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
                }
                else
                    productViewModel.PictureUrl = "assets/img/products/_Default.jpg";

                var mappedProduct = _mapper.Map<ProductViewModel<string>, Product>(productViewModel);

                _unitOfWork.GetRepository<Product, string>().Update(mappedProduct);

                var result = await _unitOfWork.CompleteAsync();
                if (result)
                {
                    return RedirectToAction("Index");
                };
            }
            return View(productViewModel);


        }

        private async Task<string> GetCreatedByAsync(string id)
        {
            var createdBy = await _dbContext.Product.Where(P => P.Id == id).Select(P => P.CreatedBy).FirstOrDefaultAsync();
            return createdBy ?? "N/A";
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var product = await _unitOfWork.GetRepository<Product, string>().GetAsync(Id);

            if (product is null) return RedirectToAction("Index");

            var mappedProduct = _mapper.Map<Product, ProductViewModel<string>>(product);

            return View(mappedProduct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]  // This validates the anti-forgery token
        public async Task<IActionResult> Delete(ProductViewModel<string> productViewModel)
        {
            if (productViewModel is null) return NotFound();
            try
            {
                var product = await _unitOfWork.GetRepository<Product, string>().GetAsync(productViewModel.Id);

                if (product.PictureUrl != null)
                {
                    PictureSettings.DeleteFile(product.PictureUrl, "products");
                }

                _unitOfWork.GetRepository<Product, string>().Delete(product);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");

            }
            catch (System.Exception)
            {
                return View(productViewModel);
            }
        }
    }
}