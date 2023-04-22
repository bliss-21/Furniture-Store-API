using API.FurnitureStore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitureStore.Testing.Shared.Test.Model.Test
{
    public class ClientTest
    {
        [Fact]
        public async Task Client_Ok_Test()
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

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(client);

            //ASSERT
            Assert.True(lstErrors.Count == 0);
        }

        [Fact]
        public async Task Client_ExceedsLength_Test()
        {
            //Arrange
            var expectedErrorMessage = "must be a string with a maximum length of";
            var stringLengthGreater250 = @"iZFEwpoYGHvgPI3xuFRXxMC0AZ9RMwPkxJR87GFZyimWa0h7NFOZjhYx9UNac2CWKJfY5vZWs3e4R6X6
                                        Zc00JmHzSVPrpvoZ1xnWIGKYSjBb2O2uSiNlTaDbCTkjdS6iDkft9Prhdx8riana1pzY7qb6PLF3d2ew
                                        2DvcXBBKjCYbvELbd1ye0kJ1pF4LCGUXmY3iNpDwzcg9nWfyprBrR1B2lzQc7x7SLCTQHY7HpnF8okCl";
            
            var client = new Client()
            {
                Id = 101,
                FirstName = stringLengthGreater250,
                LastName = stringLengthGreater250,
                BirthDate = DateTime.Now,
                Phone = stringLengthGreater250,
                Address = stringLengthGreater250
            };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(client);

            //ASSERT
            Assert.True(lstErrors.Count >= 4);
            Assert.True(lstErrors.Where(x => x.ErrorMessage.Contains(expectedErrorMessage)).Count() >= 4);
        }

        [Fact]
        public async Task Client_WithoutMeetingMinimumLength_Test()
        {
            //Arrange
            var lengthOfOne = "x";
            var client = new Client()
            {
                Id = 101,
                FirstName = lengthOfOne,
                LastName = lengthOfOne,
                BirthDate = DateTime.Now,
                Phone = lengthOfOne,
                Address = "Calle siempre falsa #123"
            };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(client);

            //ASSERT
            Assert.True(lstErrors.Count >= 3);
        }

        [Fact]
        public async Task Client_NoRequiredValues_Test()
        {
            //Arrange
            var lengthOfOne = "x";
            var client = new Client()
            {
                Id = 101,
                FirstName = string.Empty,
                LastName = string.Empty,
                BirthDate = DateTime.Now,
                Phone = "+56111111111",
                Address = "Calle siempre falsa #123"
            };

            //ACT
            var lstErrors = ModelDataAnnotation.ValidateModel(client);

            //ASSERT
            Assert.True(lstErrors.Count >= 2);
        }
    }
}
