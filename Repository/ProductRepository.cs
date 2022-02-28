using AliveStoreTemplate.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliveStoreTemplate.Repositories
{
    public interface ProductRepository
    {
        public IEnumerable<ProductList> SearchProduct(string category, string subCategory);

        public ProductList GetProductInfoById(string id);

        public void PatchProduct(ProductList product);

        public void InsertProduct(ProductList product);

        public void DeleteProduct(string productId);

        public IEnumerable<ProductList> SearchProductInfo(string SearchString);
    }
}
