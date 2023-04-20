using API.FurnitureStore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Testing.Shared.Test.Model.Test
{
    public class ProductCategoryTest
    {

        [Fact]
        public async Task ProductCategory_Ok_Test()
        {
            //Arrange
            var productCategory = new ProductCategory() { Id=101 ,Name = "Category A" };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(productCategory);

            //ASSERT
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public async Task ProductCategory_NameExceedsLength_Test()
        {

            //Arrange
            var maxLength = 40;
            var expectedErrorMessage = $"The field Name must be a string with a maximum length of {maxLength}.";

            var textLargerThanFortyCharacters = $"{Guid.NewGuid().ToString("N")}{Guid.NewGuid().ToString("N")}";
            var productCategory = new ProductCategory() { Id = 101, Name = textLargerThanFortyCharacters };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(productCategory);

            //ASSERT
            Assert.True(lstErrors.Count > 0);
            Assert.True(lstErrors.Where(x => x.ErrorMessage.Contains(expectedErrorMessage)).Count() > 0);
        }

        [Fact]
        public async Task ProductCategory_EmptyName_Test()
        {

            //Arrange
            var maxLength = 40;
            var expectedErrorMessage = $"The Name field is required.";

            var productCategory = new ProductCategory() { Id = 101 };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(productCategory);

            //ASSERT
            Assert.True(lstErrors.Count > 0);
            Assert.True(lstErrors.Where(x => x.ErrorMessage.Contains(expectedErrorMessage)).Count() > 0);
        }

        [Fact]
        public async Task ProductCategory_EmptyId_Test()
        {
            //Arrange
            var productCategory = new ProductCategory() { Name = "Category A" };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(productCategory);

            //ASSERT
            Assert.True(lstErrors.Count == 0);
        }
    }
}
