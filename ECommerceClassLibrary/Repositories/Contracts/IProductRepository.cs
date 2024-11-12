using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using System.Collections.Generic;

namespace ECommerce.Repositories
{
    public interface IProductRepository
    {
         List<Product> GetAllProducts();

        List<Product> ShowSellerProducts(User seller);

        void AddProduct(Product product);

        Product GetProductById(int id);
        void UpdateProduct(Product product);

        void AddProductQuantity(Product product, decimal additionalQuantity);

        int GetSellerProductCount(int sellerId);

        int GetAllProductCount();

        bool DeleteProduct(Product product);

        


    }
}
