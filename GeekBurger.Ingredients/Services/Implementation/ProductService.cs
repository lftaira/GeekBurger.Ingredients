using GeekBurger.Ingredients.Services.Interface;
using GeekBurger.Products.Contract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<ProductToGet>> GetProducts(string storeName)
        {
            var httpResponse = await httpClient.GetAsync(string.Format("{0}api/products?storeName={1}", httpClient.BaseAddress, storeName));

            return JsonConvert.DeserializeObject<List<ProductToGet>>(await httpResponse.Content.ReadAsStringAsync());
        }
    }
}
