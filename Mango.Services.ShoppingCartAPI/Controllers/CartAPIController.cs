using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTOs;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;
        private IProductService _productService;
        public CartAPIController(AppDBContext db, IMapper mapper, IProductService productService)
        {
            _db = db;
            _mapper = mapper;
            _productService = productService;
            _response = new ResponseDTO();
        }
        [HttpGet("GetCart/{userID}")]
        public async Task<ResponseDTO> GetCart(string userID)
        {
            try
            {
                CartDTO cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(_db.CartHeaders.First(u => u.UserID == userID))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(_db.CartDetails
                    .Where(u => u.CartHeaderID == cart.CartHeader.CartHeaderID));

                IEnumerable<ProductDTO> productDTOs = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDTOs.FirstOrDefault(u => u.ProductID == item.ProductID);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == cartDTO.CartHeader.UserID);
                if (cartHeaderFromDb == null)
                {
                    //Sepet başlığı yoksa yeniden oluştur
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDTO.CartDetails.First().CartHeaderID = cartHeader.CartHeaderID;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //Sepet başlığı varsa güncelle
                    //Sepette aynı ürün var mı onu kontrol et
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductID == cartDTO.CartDetails.First().ProductID &&
                    u.CartHeaderID == cartHeaderFromDb.CartHeaderID);

                    if (cartDetailsFromDb == null)
                    {
                        cartDTO.CartDetails.First().CartHeaderID = cartHeaderFromDb.CartHeaderID;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _db.SaveChangesAsync();

                    }
                    else
                    {
                        //Ürün sayısını güncelle
                        cartDTO.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDTO.CartDetails.First().CartHeaderID = cartDetailsFromDb.CartHeaderID;
                        cartDTO.CartDetails.First().CartDetailsID = cartDetailsFromDb.CartDetailsID;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDTO;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();  
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody] int cartDetailsID)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(u => u.CartDetailsID == cartDetailsID);

                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderID == cartDetails.CartHeaderID).Count();

                _db.CartDetails.Remove(cartDetails);

                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderID == cartDetails.CartHeaderID);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync(); 
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }
    }
}
