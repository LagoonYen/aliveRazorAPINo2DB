using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;
using AliveStoreTemplate.Repositories;
using AliveStoreTemplate.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AliveStoreTemplate.Service
{
    public class ProductServiceImpl : ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductServiceImpl(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public BaseResponseModel DeleteProduct(DeleteProductReqModel Req)
        {
            try
            {
                var productId = Req.ProductId;
                var ImgUrl = Req.ImgUrl;

                //刪除卡片路徑的卡
                if (ImgUrl != null)
                {
                    var path = $"./wwwroot/" + ImgUrl;
                    File.Delete(path);
                }

                _productRepository.DeleteProduct(productId);
                return new BaseResponseModel
                {
                    Message = "修改完成",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseQueryModel<ProductList> GetProductInfoById(string ProductId)
        {
            try
            {
                var productInfo = _productRepository.GetProductInfoById(ProductId);
                return new BaseQueryModel<ProductList>
                {
                    Results = new List<ProductList> { productInfo },
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductList>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseResponseModel InsertProduct(ProductReqModel Req)
        {
            try
            {
                //取出所有的卡片清單
                var category = "";
                var subcategory = "";
                var cardList = _productRepository.SearchProduct(category, subcategory);
                var NewcardPathInDb = "";
                if (Req.CardImg != null)
                {
                    Req.ImgUrl = Req.CardImg.FileName;
                    //比對是否有重複的圖片名稱
                    //var fileName = Req.CardImg.FileName;
                    var fileName = Guid.NewGuid().ToString("N");
                    foreach (var item in cardList)
                    {
                        var dbfileName = item.ImgUrl.Split(".").TakeLast(1).FirstOrDefault();
                        fileName = (fileName == dbfileName) ? Guid.NewGuid().ToString() : fileName;
                    }

                    //建造儲存路徑
                    var fileExtension = Req.CardImg.FileName.Split(".").TakeLast(1).FirstOrDefault();
                    NewcardPathInDb = $"img/{fileName}.{fileExtension}";

                    //感謝Kevin指引 [走丟的] 路徑要加在哪邊www
                    using (var stream = new FileStream($"./wwwroot/" + NewcardPathInDb, FileMode.Create))
                    {
                        Req.CardImg.CopyTo(stream);
                    }
                }

                for(int i = 0; i < Req.Abilities.Count; i++)
                {
                    if(Req.Abilities[i].AbilityName == null && Req.Abilities[i].AbilityDesc == null)
                    {
                        Req.Abilities.Remove(Req.Abilities[i]);
                    }
                }

                for (int i = 0; i < Req.Moves.Count; i++)
                {
                    if (Req.Moves[i].MoveName == null && Req.Moves[i].MoveDesc == null)
                    {
                        Req.Moves.Remove(Req.Moves[i]);
                    }
                }

                ProductList product = new ProductList
                {
                    ProductId = "Product" + Guid.NewGuid().ToString("N"),
                    CardName = Req.CardName,
                    Category = Req.Category,
                    Subcategory = Req.Subcategory,
                    Price = Req.Price,
                    Abilities = Req.Abilities,
                    Moves = Req.Moves,
                    Inventory = Req.Inventory,
                    ImgUrl = NewcardPathInDb,
                    RealseTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                _productRepository.InsertProduct(product);
                return new BaseResponseModel
                {
                    Message = "修改完成",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseResponseModel PatchProductAllInfo(ProductReqModel Req)
        {
            try
            {
                //先抓舊的ImgUrl
                var fileName = Req.ImgUrl;

                if (Req.CardImg != null)
                {
                    //新檔案的類別名稱
                    var fileExtension = Req.CardImg.FileName.Split(".").TakeLast(1).FirstOrDefault();
                    //設置新增卡片名稱 有舊的→取代 沒有→新增
                    fileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : $"img/{Guid.NewGuid().ToString()}.{fileExtension}";
                    //新增圖片進檔案
                    using (var stream = new FileStream($"./wwwroot/" + fileName, FileMode.Create))
                    {
                        Req.CardImg.CopyTo(stream);
                    }
                }

                for (int i = 0; i < Req.Abilities.Count; i++)
                {
                    if (Req.Abilities[i].AbilityName == null && Req.Abilities[i].AbilityDesc == null)
                    {
                        Req.Abilities.Remove(Req.Abilities[i]);
                    }
                }

                for (int i = 0; i < Req.Moves.Count; i++)
                {
                    if (Req.Moves[i].MoveName == null && Req.Moves[i].MoveDesc == null)
                    {
                        Req.Moves.Remove(Req.Moves[i]);
                    }
                }

                ProductList productList = new ProductList()
                {
                    ProductId = Req.Id,
                    Category = Req.Category,
                    Subcategory = Req.Subcategory,
                    CardName = Req.CardName,
                    Abilities = Req.Abilities,
                    Moves = Req.Moves,
                    HP = Req.HP,
                    Price = Req.Price,
                    Inventory = Req.Inventory,
                    ImgUrl = fileName,
                    UpdateTime = DateTime.Now
                };


                _productRepository.PatchProduct(productList);
                return new BaseResponseModel
                {
                    Message = "修改完成",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseQueryModel<ProductResultModel> Product_CategoryList()
        {
            try
            {
                string category = "";
                string subCategory = "";

                //先取出所有卡片的資料
                var productList = _productRepository.SearchProduct(category, subCategory);
                //製作卡片大小分類
                var productViewModel = productList.Select(x => new
                {
                    Category = x.Category,
                    SubCategory = x.Subcategory
                }).GroupBy(x => x.Category).Select(x => new ProductResultModel
                {
                    Category = x.Key,
                    SubCategory = x.Select(x => x.SubCategory).Distinct().ToList()
                });

                return new BaseQueryModel<ProductResultModel>
                {
                    Results = productViewModel,
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductResultModel>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseQueryModel<ProductList> SearchProduct(SearchProductReqModel Req)
        {
            try
            {
                //取得Category及SubCategory分類
                var category = Req.Category;
                var subCategory = Req.Subcategory;

                var productList = _productRepository.SearchProduct(category, subCategory);
                return new BaseQueryModel<ProductList>
                {
                    Results = productList,
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductList>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseQueryModel<ProductList> SearchProductInfo(string SearchString)
        {
            try
            {
                var productList = _productRepository.SearchProductInfo(SearchString);
                return new BaseQueryModel<ProductList>
                {
                    Results = productList,
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductList>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
