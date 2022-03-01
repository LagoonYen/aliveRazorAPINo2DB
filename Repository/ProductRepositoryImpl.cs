using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Repositories;
using No2DB.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AliveStoreTemplate.Repository
{
    public class ProductRepositoryImpl : ProductRepository
    {
        public void DeleteProduct(string productId)
        {
            try
            {
                No2DB.Transaction.Operator op = new No2DB.Transaction.Operator("aaa");
                var collection = new DRole("PokemonCardInfo");
                op.Delete(collection, "Product", productId);
                op.Done();
            }
            catch
            {
                throw;
            }
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

        public void PatchProductInventory(PatchProductInventoryConditionModel ProductInfo)
        {
            try
            {
                var productId = ProductInfo.ProductId;
                var inventory = ProductInfo.Inventory;
                No2DB.Transaction.Operator op = new No2DB.Transaction.Operator("aaa");

                var collection = new DRole("PokemonCardInfo");
                var obj = collection.GetQ<ProductList>("Product").DataByKey(productId);

                obj.Inventory = inventory;

                op.Update(collection, "Product", productId, obj);
                op.Done();
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

        public IEnumerable<ProductList> SearchProductInfo(string SearchString)
        {
            try
            {
                var collection = new DRole("PokemonCardInfo");

                var allObjList = collection.Query<ProductList>("Product").AllDatas();
                List<ProductList> SearchObj = new List<ProductList>();

                //先找到該署性是否有東西 再查詢是否相同
                var objs = allObjList.Where(x => x.Abilities.Count(c => c.AbilityName == SearchString) > 0);

                return objs;
            }
            catch
            {
                throw;
            }
        }
    }
}
