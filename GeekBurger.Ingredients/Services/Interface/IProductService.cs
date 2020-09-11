using GeekBurger.Products.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Services.Interface
{
    public interface IProductService
    {
        Task<List<ProductToGet>> GetProducts(string storeName);
    }
}
