using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace rapidmq_as.net_core_6E.mq.Tests
{
    internal record Product(int productId, string productName);
    public class ProductTest : IDisposable
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7202/api/Product")
        };

        public void Dispose()
        {
            _httpClient.DeleteAsync("/deleteproduct?Id=1").GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GivenARequest_WhenCallingGetProduct_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent = new[]
            {
            new Product(1, "Araba #1"),
            new Product(2, "Araba #2"),
            new Product(3, "Araba  #3"),
            new Product(4, "Araba  #4"),
            new Product(5, "Araba #5")
        };
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var response = await _httpClient.GetAsync("/productlist");

            // Assert.
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingPostProduct_ThenTheAPIReturnsExpectedResponseAndAddsBook()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.Created;
            var expectedContent = new Product(6, "product");
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var response = await _httpClient.PostAsync("/addproduct", TestHelpers.GetJsonStringContent(expectedContent));

            // Assert.
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact]
        public async Task GivenARequest_WhenCallingPutProduct_ThenTheAPIReturnsExpectedResponseAndUpdatesBook()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.NoContent;
            var updateproduct = new Product(6, "Awesome book #6 - Updated");
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var response = await _httpClient.PutAsync("/updateproduct", TestHelpers.GetJsonStringContent(updateproduct));

           // Assert.
           //TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }

        //[Fact]
        //public async Task GivenARequest_WhenCallingDeleteProduct_ThenTheAPIReturnsExpectedResponseAndDeletesBook()
        //{
        //    // Arrange.
        //    var expectedStatusCode = System.Net.HttpStatusCode.NoContent;
        //    var bookIdToDelete = 1;
        //    var stopwatch = Stopwatch.StartNew();

        //    // Act.
        //    var response = await _httpClient.DeleteAsync($"/deleteproduct?Id=1");

        //    // Assert.
        //    TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        //}

        [Fact]
        public async Task GivenAnAuthenticatedRequest_WhenCallingAdmin_ThenTheAPIReturnsExpectedResponse()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent = "Hi admin!";
            var stopwatch = Stopwatch.StartNew();
            var request = new HttpRequestMessage(HttpMethod.Get, "/admin");
            request.Headers.Add("X-Api-Key", "SuperSecretApiKey");

            // Act.
            var response = await _httpClient.SendAsync(request);

            // Assert.
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("WrongApiKey")]
        public async Task GivenAnUnauthenticatedRequest_WhenCallingAdmin_ThenTheAPIReturnsUnauthorized(string apiKey)
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
            var stopwatch = Stopwatch.StartNew();
            var request = new HttpRequestMessage(HttpMethod.Get, "/admin");
            request.Headers.Add("X-Api-Key", apiKey);

            // Act.
            var response = await _httpClient.SendAsync(request);

            // Assert.
            TestHelpers.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
        }
    }

}
