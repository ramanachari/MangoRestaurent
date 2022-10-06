using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));    
            }

            return View();
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cart)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _cartService.ApplyCouponAsync<ResponseDto>(cart, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _cartService.RemoveCouponAsync<ResponseDto>(userId);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _cartService.Checkout<ResponseDto>(cartDto.CartHeader, accessToken);
                if (!response.IsSuccess)
                {
                    TempData["Error"] = response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception)
            {
                return View(cartDto);
            }
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

            CartDto cartDto = new ();
            if (response != null && response.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }

            if (cartDto.CartHeader != null)
            {
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    ResponseDto couponResp = await _couponService.GetCouponAsync<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);
                    if (couponResp != null && couponResp.IsSuccess)
                    {
                        CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponResp.Result));
                        cartDto.CartHeader.DiscountAmount = couponDto.DiscountAmount;
                    }
                }
                
                cartDto.CartHeader.OrderTotal = cartDto.CartDetails.Sum(c => c.Count * c.Product.Price) - cartDto.CartHeader.DiscountAmount;
            }

            return cartDto;

        }

    }
}
