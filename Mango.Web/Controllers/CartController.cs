using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsID)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            ResponseDTO? response = await _cartService.RemoveFromCartAsync(cartDetailsID);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item removed from cart successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDto)
        {

            ResponseDTO? response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null & response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<CartDTO> LoadCartDTOBasedOnLoggedInUser()
        {
           var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            ResponseDTO? response = await _cartService.GetCartByUserIDAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
                return cartDTO;
            }
            return new CartDTO();
        }
    }
}
