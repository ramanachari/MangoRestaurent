using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> productList = new();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetAllProductsAsync<ResponseDto>(accessToken);

            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }

            return View(productList);
        }

        public Task<IActionResult> ProductCreate()
        {
            return Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            { 
                return View(productDto);
            }
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.CreateProductAsync<ResponseDto>(productDto, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }

            return View(productDto);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);

            if (response != null && response.IsSuccess)
            {
                ProductDto productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                
                return View(productDto);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.UpdateProductAsync<ResponseDto>(productDto, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }

            return View(productDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);

            if (response != null && response.IsSuccess)
            {
                ProductDto productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(productDto);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.DeleteProductAsync<ResponseDto>(productDto.Id, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }

            return View(productDto);
        }
    }
}
