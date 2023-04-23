using API.FurnitureStore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Testing.Shared.Test.Model.Test
{
    public class ProductTest
    {
        [Fact]
        public async Task Product_Ok_Test()
        {
            //Arrange
            var productCategory = new ProductCategory() { Id = 1, Name = "Catagory name"};

            var product = new Product()
            {
                Id = 101,
                Name = "Test",
                Price = 1,
                ProductCategoryId = 1,
                IsActive = true,
                OrderDetails = null,
                ProductCategory = null
            };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(product);

            //ASSERT
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public async Task Product_NameExceedsLength_Error_Test()
        {
            //Arrange
            var expectedErrorMessage = "must be a string with a maximum length of";
            var stringLengthGreater250 = @"iZFEwpoYGHvgPI3xuFRXxMC0AZ9RMwPkxJR87GFZyimWa0h7NFOZjhYx9UNac2CWKJfY5vZWs3e4R6X6
                                        Zc00JmHzSVPrpvoZ1xnWIGKYSjBb2O2uSiNlTaDbCTkjdS6iDkft9Prhdx8riana1pzY7qb6PLF3d2ew
                                        2DvcXBBKjCYbvELbd1ye0kJ1pF4LCGUXmY3iNpDwzcg9nWfyprBrR1B2lzQc7x7SLCTQHY7HpnF8okCl";

            var product = new Product()
            {
                Id = 101,
                Name = stringLengthGreater250,
                Price = 1,
                ProductCategoryId = 1,
                IsActive = true,
                OrderDetails = null,
                ProductCategory = null
            };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(product);

            //ASSERT
            Assert.True(lstErrors.Count >= 1);
            Assert.True(lstErrors.Where(x => x.ErrorMessage.Contains(expectedErrorMessage)).Count() >= 1);
        }

        [Fact]
        public async Task Product_NameNotRequiredWidth_Error_Test()
        {
            //Arrange
            var expectedErrorMessage = "field must be at least";

            var product = new Product()
            {
                Id = 101,
                Name = "x",
                Price = 1,
                ProductCategoryId = 1,
                IsActive = true,
                OrderDetails = null,
                ProductCategory = null
            };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(product);

            //ASSERT
            Assert.True(lstErrors.Count >= 1);
            Assert.True(lstErrors.Where(x => x.ErrorMessage.Contains(expectedErrorMessage)).Count() >= 1);
        }

        [Fact]
        public async Task Product_PriceEquealToZero_Error_Test()
        {
            //Arrange
            var expectedErrorMessage = "The field Price must be greater than 0.";

            var product = new Product()
            {
                Id = 101,
                Name = "Test",
                Price = 0,
                ProductCategoryId = 1,
                IsActive = true,
                OrderDetails = null,
                ProductCategory = null
            };


            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(product);

            //ASSERT
            Assert.True(lstErrors.Count >= 1);
            Assert.True(lstErrors.Where(x => x.ErrorMessage.Contains(expectedErrorMessage)).Count() >= 1);
        }

        [Fact]
        public async Task Product_MissingRequiredFields_Error_Test()
        {
            //Arrange
            var product = new Product()
            {
                Id = 101,
                OrderDetails = null,
                ProductCategory = null
            };


            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(product);

            //ASSERT
            Assert.True(lstErrors.Count >= 3);
        }
        
        [Fact]
        public async Task Product_IfThe_IsActive_Field_IsNotSpecified_MakeItTrue_Test()
        {
            //Arrange
            var expectedValue = true;
            var product = new Product() {};

            //ASSERT
            Assert.Equal(expectedValue, product.IsActive);
        }
    }
}
