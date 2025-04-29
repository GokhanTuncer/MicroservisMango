using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCartByUserIDAsync(string userID);
        Task<ResponseDTO?> UpsertCartsAsync(CartDTO cartDTO); 
        Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsID); 
        Task<ResponseDTO?> ApplyCouponAsync(CartDTO cartDTO);
    }
}
