using ECommerceClassLibrary.Models;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Repositories.Contracts
{
    public interface IProductService
    {
        void AddProduct(User currentUser);

        void DisplayProducts(List<Product> products);

        void ShowAllProducts();
        List<Product> GetSellerProducts(User currentUser);
        Product GetProductById(int id);
        bool UpdateProduct(int productId,User currentUser);

        void AddProductQuantity(int productId);

        int GetSellerProductCount(int userId);

        bool DeleteProduct(int productId,User currentUser);
    }
}
