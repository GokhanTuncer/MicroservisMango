using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetProduct(string productCode);
        Task<ResponseDTO?> GetAllProductsAsync(); 
        Task<ResponseDTO?> GetProductByIdAsync(int id);
        Task<ResponseDTO?> CreateProductsAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProductsAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProductsAsync(int id);

    }
}
