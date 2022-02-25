using AliveStoreTemplate.Model;
using AliveStoreTemplate.Repositories;
using No2DB.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AliveStoreTemplate.Repository
{
    public class ProductRepositoryImpl : ProductRepository
    {
        public void DeleteProduct(int productId)
        {
            throw new System.NotImplementedException();
        }

        public ProductList GetProductInfoById(string ProductId)
        {
            try
            {
                var collection = new DRole("PokemonCardInfo");
                var obj = collection.Query<ProductList>("Product").DataByKey(ProductId);
                if (obj == null)
                {
                    throw new Exception("找不到卡片資訊");
                }
                return obj;
            }
            catch
            {
                throw;
            }
        }

        public void InsertProduct(ProductList product)
        {
            try
            {
                var collection = new DRole("PokemonCardInfo");
                collection.GetOp("Product").Update(product.ProductId, product);
            }
            catch
            {
                throw;
            }
        }

        public void PatchProduct(ProductList product)
        {
            try
            {
                var collection = new DRole("PokemonCardInfo");
                collection.GetOp("Product").Update(product.ProductId, product);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ProductList> SearchProduct(string category, string subCategory)
        {
            try
            {
                var collection = new DRole("PokemonCardInfo");

                var allObjList = collection.Query<ProductList>("Product").AllDatas();
                if(allObjList != null)
                {
                    var objList = (category != "") ?
                        (subCategory != "") ?
                             allObjList.Where(x => x.Category == category).Where(x => x.Subcategory == subCategory).ToList() :
                                 allObjList.Where(x => x.Category == category).ToList() :
                                    allObjList.ToList();
                    
                    if (objList != null)
                    {
                        return objList;
                    }
                }

                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
