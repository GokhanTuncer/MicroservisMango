using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCartByUserIDAsync(string userID);
        Task<ResponseDTO?> UpsertCartsAsync(ca); 
        Task<ResponseDTO?> GetCartByIdAsync(int id);
        Task<ResponseDTO?> CreateCartsAsync(CartDTO CartDTO);
        Task<ResponseDTO?> UpdateCartsAsync(CartDTO CartDTO);
        Task<ResponseDTO?> DeleteCartsAsync(int id);

    }
}
