
using API.FurnitureStore.API.Controllers;
using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.FurnitureStore.Testing.ControllersAPI
{
    public class ProductCategoriesControllerTesting
    {

        #region [GET]
        [Fact]
        public async Task GetAllProductCategoriesTest()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var productCategoriesList = new List<ProductCategory>() 
                {
                    new ProductCategory(){ Id=101, Name="Category A" },
                    new ProductCategory(){ Id=102, Name="Category B" },
                    new ProductCategory(){ Id=103, Name="Category C" },
                    new ProductCategory(){ Id=104, Name="Category D" },
                };
                await context.AddRangeAsync(productCategoriesList);
                await context.SaveChangesAsync();

                var controllerTest = new ProductCategoriesController(context);
                var amountAdded = productCategoriesList.Count();

                //Act
                var result = await controllerTest.Get();

                //Assert
                Assert.NotNull(result);
                Assert.Equal(amountAdded, result.Count());
            }
        }

        [Fact]
        public async Task GetProductCategoryByIdThatExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.OK;

                var idThatExist = 101;
                var productCategory = new ProductCategory(){ Id=idThatExist, Name="Category A" };
                await context.AddAsync(productCategory);
                await context.SaveChangesAsync();

                var controllerTest = new ProductCategoriesController(context);

                //Act
                var actionResult = await controllerTest.GetDetail(productCategory.Id);

                //Assert
                var okObjectResult = actionResult as OkObjectResult;
                Assert.NotNull(okObjectResult);
                Assert.Equal(expectedStatusCode, okObjectResult.StatusCode);

                var model = okObjectResult.Value as ProductCategory;
                Assert.NotNull(model);
                Assert.Equal(productCategory.Id, model.Id);
                Assert.Equal(productCategory.Name, model.Name);
            }
        }

        [Fact]
        public async Task GetProductCategoryByIdThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.BadRequest;
                var controllerTest = new ProductCategoriesController(context);
                var idThatNoExist = 99;

                //Act
                var actionResult = await controllerTest.GetDetail(idThatNoExist);

                //Assert
                var badObjectResult = actionResult as BadRequestResult;
                Assert.NotNull(badObjectResult);
                Assert.Equal(expectedStatusCode, badObjectResult.StatusCode);
            }
        }
        #endregion

        #region [POST]
        [Fact]
        public async Task PostProductCategoryWithIdOk()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.Created;
                var productCategory = new ProductCategory() { Id = 101, Name = "Category A" };
                var controllerTest = new ProductCategoriesController(context);

                //Act
                var actionResult = await controllerTest.Post(productCategory);

                //Assert
                var createdObjectResult = actionResult as ObjectResult;
                Assert.NotNull(createdObjectResult);
                Assert.Equal(expectedStatusCode, createdObjectResult.StatusCode);

                var model = createdObjectResult.Value as ProductCategory;
                Assert.NotNull(model);

                var searchedObjectBd = context.ProductCategories.FirstOrDefault(x => x.Id == productCategory.Id);
                Assert.NotNull(searchedObjectBd);

                Assert.Equal(productCategory.Id, model.Id);
                Assert.Equal(productCategory.Name, model.Name);
            }
        }

        [Fact]
        public async Task PostProductCategoryWithEmptyIdOk()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.Created;
                var productCategory = new ProductCategory() { Name = "Category A" };
                var controllerTest = new ProductCategoriesController(context);

                //Act
                var actionResult = await controllerTest.Post(productCategory);

                //Assert
                var createdObjectResult = actionResult as ObjectResult;
                Assert.NotNull(createdObjectResult);
                Assert.Equal(expectedStatusCode, createdObjectResult.StatusCode);

                var model = createdObjectResult.Value as ProductCategory;
                Assert.NotNull(model);

                var searchedObjectBd = context.ProductCategories.FirstOrDefault(x => x.Id == productCategory.Id);
                Assert.NotNull(searchedObjectBd);

                Assert.Equal(productCategory.Id, model.Id);
                Assert.Equal(productCategory.Name, model.Name);
            }
        }
        #endregion

        #region [PUT]
        [Fact]
        public async Task PutProductCategoryThatExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.NoContent;
                var productCategory = new ProductCategory() { Id=99, Name = "Category" };
                await context.AddAsync(productCategory);
                await context.SaveChangesAsync();

                var controllerTest = new ProductCategoriesController(context);
                var newName = Guid.NewGuid().ToString();
                productCategory.Name = newName;

                //Act
                var actionResult = await controllerTest.Put(productCategory);

                //Assert
                var updateObjectResult = actionResult as NoContentResult;
                Assert.NotNull(updateObjectResult);
                Assert.Equal(expectedStatusCode, updateObjectResult.StatusCode);


                var searchedObjectBd = context.ProductCategories.FirstOrDefault(x => x.Id == productCategory.Id);
                Assert.NotNull(searchedObjectBd);

                Assert.Equal(productCategory.Id, searchedObjectBd.Id);
                Assert.Equal(productCategory.Name, searchedObjectBd.Name);
            }
        }

        [Fact]
        public async Task PutProductCategoryThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var productCategory = new ProductCategory() { Id = 99, Name = "Category" };
                var expectedMessageExeption = "Attempted to update or delete an entity that does not exist in the store.";

                var controllerTest = new ProductCategoriesController(context);
                var newName = Guid.NewGuid().ToString();
                productCategory.Name = newName;

                //Act
                Func<Task> act = () => controllerTest.Put(productCategory);

                //Assert
                var exception = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(act);
                Assert.Equal(expectedMessageExeption, exception.Message);
  
                var searchedObjectBd = context.ProductCategories.FirstOrDefault(x => x.Id == productCategory.Id);
                Assert.Null(searchedObjectBd);
            }
        }
        #endregion

        #region [DELETE]
        [Fact]
        public async Task DeleteProductCategoryOk()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var IdToDelete = 101;
                var expectedStatusCode = (int)HttpStatusCode.NoContent;
                var productCategory = new ProductCategory() { Id = IdToDelete, Name = "Category" };
                await context.AddAsync(productCategory);
                await context.SaveChangesAsync();

                var controllerTest = new ProductCategoriesController(context);

                //Act
                var actionResult = await controllerTest.Delete(productCategory);

                //Assert
                var deleteObjectResult = actionResult as NoContentResult;
                Assert.NotNull(deleteObjectResult);
                Assert.Equal(expectedStatusCode, deleteObjectResult.StatusCode);


                var searchedObjectBd = context.ProductCategories.FirstOrDefault(x => x.Id == productCategory.Id);
                Assert.Null(searchedObjectBd);
            }
        }

        [Fact]
        public async Task DeleteProductCategoryThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedMessageExeption = "Attempted to update or delete an entity that does not exist in the store.";
                var productCategory = new ProductCategory() { Id = 101, Name = "Category" };
 

                var controllerTest = new ProductCategoriesController(context);

                //Act
                Func<Task> act = () => controllerTest.Delete(productCategory);

                //Assert
                var exception = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(act);
                Assert.Equal(expectedMessageExeption, exception.Message);

                var searchedObjectBd = context.ProductCategories.FirstOrDefault(x => x.Id == productCategory.Id);
                Assert.Null(searchedObjectBd);
            }
        }

        [Fact]
        public async Task DeleteProductCategoryThatSendNull()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.NotFound;
                var controllerTest = new ProductCategoriesController(context);

                //Act
                var actionResult = await controllerTest.Delete(null);

                //Assert
                var badObjectResult = actionResult as NotFoundResult;

                Assert.NotNull(badObjectResult);
                Assert.Equal(expectedStatusCode, badObjectResult.StatusCode);
            }
        }
        #endregion

    }
}
