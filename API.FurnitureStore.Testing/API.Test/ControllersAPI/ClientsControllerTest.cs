using API.FurnitureStore.API.Controllers;
using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Testing.API.Test.ControllersAPI
{
    public class ClientsControllerTest
    {
        #region [GET]
        [Fact]
        public async Task GetAllClientsTest()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var clientList = new List<Client>()
                {
                    new Client()
                    {
                        Id = 101,
                        FirstName = "Jose",
                        LastName = "Lee",
                        BirthDate = DateTime.Now,
                        Phone = "+56111111111",
                        Address = "Calle siempre falsa #123"
                    },
                    new Client()
                    {
                        Id = 102,
                        FirstName = "Pablo",
                        LastName = "Perez",
                        BirthDate = DateTime.Now,
                        Phone = "+56111111111",
                        Address = "Calle siempre falsa #123"
                    },
                };

                
                await context.AddRangeAsync(clientList);
                await context.SaveChangesAsync();

                var controllerTest = new ClientsController(context);
                var amountAdded = clientList.Count();

                //Act
                var result = await controllerTest.Get();

                //Assert
                Assert.NotNull(result);
                Assert.Equal(amountAdded, result.Count());
            }
        }


        [Fact]
        public async Task GetClientsByIdThatExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.OK;

                var idThatExist = 101;
                var client = new Client()
                {
                    Id = idThatExist,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };
                
                await context.AddAsync(client);
                await context.SaveChangesAsync();

                var controllerTest = new ClientsController(context);

                //Act
                var actionResult = await controllerTest.GetDetail(idThatExist);

                //Assert
                var okObjectResult = actionResult as OkObjectResult;
                Assert.NotNull(okObjectResult);
                Assert.Equal(expectedStatusCode, okObjectResult.StatusCode);

                var model = okObjectResult.Value as Client;
                Assert.NotNull(model);

                Assert.Equal(client.Id, model.Id);
                Assert.Equal(client.FirstName, model.FirstName);
                Assert.Equal(client.LastName, model.LastName);
                Assert.Equal(client.BirthDate, model.BirthDate);
                Assert.Equal(client.Phone, model.Phone);
                Assert.Equal(client.Address, model.Address);
            }
        }

        [Fact]
        public async Task GeClientByIdThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.BadRequest;
                var controllerTest = new ClientsController(context);
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
        public async Task PostClientWithIdOk()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.Created;
                var client = new Client()
                {
                    Id = 101,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };
                var controllerTest = new ClientsController(context);

                //Act
                var actionResult = await controllerTest.Post(client);

                //Assert
                var createdObjectResult = actionResult as ObjectResult;
                Assert.NotNull(createdObjectResult);
                Assert.Equal(expectedStatusCode, createdObjectResult.StatusCode);

                var model = createdObjectResult.Value as Client;
                Assert.NotNull(model);

                var searchedObjectBd = context.Clients.FirstOrDefault(x => x.Id == client.Id);
                Assert.NotNull(searchedObjectBd);

                Assert.Equal(client.Id, model.Id);
                Assert.Equal(client.FirstName, model.FirstName);
                Assert.Equal(client.LastName, model.LastName);
                Assert.Equal(client.BirthDate, model.BirthDate);
                Assert.Equal(client.Phone, model.Phone);
                Assert.Equal(client.Address, model.Address);
            }
        }

        [Fact]
        public async Task PostClientWithEmptyIdOk()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.Created;
                var client = new Client()
                {
                    Id = 101,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };
                var controllerTest = new ClientsController(context);

                //Act
                var actionResult = await controllerTest.Post(client);

                //Assert
                var createdObjectResult = actionResult as ObjectResult;
                Assert.NotNull(createdObjectResult);
                Assert.Equal(expectedStatusCode, createdObjectResult.StatusCode);

                var model = createdObjectResult.Value as Client;
                Assert.NotNull(model);

                var searchedObjectBd = context.Clients.FirstOrDefault(x => x.Id == client.Id);
                Assert.NotNull(searchedObjectBd);

                Assert.Equal(client.Id, model.Id);
                Assert.Equal(client.FirstName, model.FirstName);
                Assert.Equal(client.LastName, model.LastName);
                Assert.Equal(client.BirthDate, model.BirthDate);
                Assert.Equal(client .Phone, model.Phone);
                Assert.Equal(client.Address, model.Address);
            }
        }
        #endregion

        #region [PUT]
        [Fact]
        public async Task PutClientThatExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.NoContent;
                var client = new Client()
                {
                    Id = 101,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };
                await context.AddAsync(client);
                await context.SaveChangesAsync();

                var controllerTest = new ClientsController(context);
                var randomString = Guid.NewGuid().ToString();
                client.FirstName = randomString;
                client.LastName = randomString;
                client.BirthDate = DateTime.UtcNow;
                client.Phone = "+56111111234";
                client.Address = randomString;

                //Act
                var actionResult = await controllerTest.Put(client);

                //Assert
                var updateObjectResult = actionResult as NoContentResult;
                Assert.NotNull(updateObjectResult);
                Assert.Equal(expectedStatusCode, updateObjectResult.StatusCode);


                var searchedObjectBd = context.Clients.FirstOrDefault(x => x.Id == client.Id);
                Assert.NotNull(searchedObjectBd);

                Assert.Equal(client.Id, searchedObjectBd.Id);
                Assert.Equal(client.FirstName, searchedObjectBd.FirstName);
                Assert.Equal(client.LastName, searchedObjectBd.LastName);
                Assert.Equal(client.BirthDate, searchedObjectBd.BirthDate);
                Assert.Equal(client.Phone, searchedObjectBd.Phone);
                Assert.Equal(client.Address, searchedObjectBd.Address);
            }
        }

        [Fact]
        public async Task PutClientThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var client = new Client()
                {
                    Id = 101,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };
                var expectedMessageExeption = "Attempted to update or delete an entity that does not exist in the store.";

                var controllerTest = new ClientsController(context);
                var randomString = Guid.NewGuid().ToString();
                client.FirstName = randomString;
                client.LastName = randomString;
                client.BirthDate = DateTime.UtcNow;
                client.Phone = "+56111111234";
                client.Address = randomString;

                //Act
                Func<Task> act = () => controllerTest.Put(client);

                //Assert
                var exception = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(act);
                Assert.Equal(expectedMessageExeption, exception.Message);

                var searchedObjectBd = context.Clients.FirstOrDefault(x => x.Id == client.Id);
                Assert.Null(searchedObjectBd);
            }
        }
        #endregion

        #region [DELETE]
        [Fact]
        public async Task DeleteClientOk()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.NoContent;
                var client = new Client()
                {
                    Id = 101,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };
                await context.AddAsync(client);
                await context.SaveChangesAsync();

                var controllerTest = new ClientsController(context);

                //Act
                var actionResult = await controllerTest.Delete(client);

                //Assert
                var deleteObjectResult = actionResult as NoContentResult;
                Assert.NotNull(deleteObjectResult);
                Assert.Equal(expectedStatusCode, deleteObjectResult.StatusCode);


                var searchedObjectBd = context.Clients.FirstOrDefault(x => x.Id == client.Id);
                Assert.Null(searchedObjectBd);
            }
        }

        [Fact]
        public async Task DeletClientThatNotExist()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedMessageExeption = "Attempted to update or delete an entity that does not exist in the store.";
                var client = new Client()
                {
                    Id = 101,
                    FirstName = "Jose",
                    LastName = "Lee",
                    BirthDate = DateTime.Now,
                    Phone = "+56111111111",
                    Address = "Calle siempre falsa #123"
                };

                var controllerTest = new ClientsController(context);

                //Act
                Func<Task> act = () => controllerTest.Delete(client);

                //Assert
                var exception = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(act);
                Assert.Equal(expectedMessageExeption, exception.Message);

                var searchedObjectBd = context.Clients.FirstOrDefault(x => x.Id == client.Id);
                Assert.Null(searchedObjectBd);
            }
        }

        [Fact]
        public async Task DeletClientThatSendNull()
        {
            using (var context = new APIFurnitureStoreContext(ConfigOptionsDataBaseInMemory.CreateNewContextOptions()))
            {
                //Arrange
                var expectedStatusCode = (int)HttpStatusCode.NotFound;
                var controllerTest = new ClientsController(context);

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
