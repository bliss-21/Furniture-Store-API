using API.FurnitureStore.API.Controllers;
using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Testing.API.Test.ControllersAPI
{
    public class ProductControllerTest
    {
        #region [GET]
        [Fact]
        public async Task GetAllProductTest()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var productCategoriesList = new List<ProductCategory>()
                {
                    new ProductCategory(){ Id=101, Name="Category A" },
                    new ProductCategory(){ Id=102, Name="Category B" }
                };

                var productList = new List<Product>()
                {
                    new Product() { Id=1001, Name="Product 1", Price=1000, ProductCategoryId=101 },
                    new Product() { Id=1002, Name="Product 2", Price=2000, ProductCategoryId=101 },
                    new Product() { Id=1003, Name="Product 3", Price=3000, ProductCategoryId=101 },
                    new Product() { Id=1004, Name="Product 4", Price=4000, ProductCategoryId=101 },
                    new Product() { Id=1005, Name="Product 5", Price=5000, ProductCategoryId=101 },
                    new Product() { Id=1006, Name="Product 6", Price=6000, ProductCategoryId=102 },
                    new Product() { Id=1007, Name="Product 7", Price=7000, ProductCategoryId=102 },
                    new Product() { Id=1008, Name="Product 8", Price=8000, ProductCategoryId=102 },
                    new Product() { Id=1009, Name="Product 9", Price=9000, ProductCategoryId=102 },
                    new Product() { Id=1010, Name="Product 0", Price=1100, ProductCategoryId=102 }
                };

                await context.AddRangeAsync(productCategoriesList);
                await context.AddRangeAsync(productList);
                await context.SaveChangesAsync();

                var controllerTest = new ProductsController(context);

                //var httpContext = new DefaultHttpContext()
                //{
                //    User =  new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "elias"), new Claim(ClaimTypes.Email, "elias") }))
                //};

                //controllerTest.ControllerContext.HttpContext = httpContext;


                var amountAdded = productList.Count();

                //Act
                var result = await controllerTest.Get();

                //Assert
                Assert.NotNull(result);
                Assert.Equal(amountAdded, result.Count());
            }
        }

        [Fact]
        public async Task GetProductByIdThatExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange

                //Arrange

                var idThatExist = 1001;

                var productCategory = new ProductCategory()
                {
                    Id = 101,
                    Name = "Category A"
                };

                var product = new Product()
                {
                    Id = 1001,
                    Name = "Product 1",
                    Price = 1000,
                    ProductCategoryId = 101
                };

                await context.AddAsync(productCategory);
                await context.AddAsync(product);
                await context.SaveChangesAsync();

                var expectedStatusCode = (int)HttpStatusCode.OK;

                var controllerTest = new ProductsController(context);

                //Act
                var actionResult = await controllerTest.GetDetails(idThatExist);

                //Assert
                var okObjectResult = actionResult as OkObjectResult;
                Assert.NotNull(okObjectResult);
                Assert.Equal(expectedStatusCode, okObjectResult.StatusCode);

                var model = okObjectResult.Value as Product;
                Assert.NotNull(model);

                Assert.Equal(product.Id, model.Id);
                Assert.Equal(product.Name, model.Name);
                Assert.Equal(product.Price, model.Price);
                Assert.Equal(product.ProductCategoryId, model.ProductCategoryId);
            }
        }

        [Fact]
        public async Task GetProductByIdThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.BadRequest;
                var controllerTest = new ProductsController(context);
                var idThatNoExist = 99;

                //Act
                var actionResult = await controllerTest.GetDetails(idThatNoExist);

                //Assert
                var badObjectResult = actionResult as BadRequestResult;
                Assert.NotNull(badObjectResult);
                Assert.Equal(expectedStatusCode, badObjectResult.StatusCode);
            }
        }
        #endregion


    }
}
