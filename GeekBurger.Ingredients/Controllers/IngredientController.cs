using GeekBurger.Ingredients.Contract;
using GeekBurger.Ingredients.Model;
using GeekBurger.Ingredients.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Controllers
{
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private IProductService productService;
        private List<GeekBurger.Products.Contract.ProductToGet> Products { get; set; }

        public IngredientController(IProductService productService)
        {
            this.productService = productService;
        }


        [HttpGet("api")]
        public IActionResult Index()
        {
            return Ok("teste");
        }

        [HttpGet("api/products/byrestrictions/{productRequest}")]
        public async Task<IActionResult> GetIngredient([FromQuery] ProductRequest productRequest)
        {
            var ingredients = new List<IngredientToGet>();
            var produtos = await productService.GetProducts(productRequest.StoreName);

            foreach (var product in produtos)
            {
                foreach (var restriction in productRequest.Restrictions)
                {
                    if (product.Items.FirstOrDefault(e => e.Name.Contains(restriction)) == null)
                    {
                        var ingredient = new IngredientToGet();
                        ingredient.ProductId = product.ProductId;
                        ingredient.Ingredients = new List<string>();

                        foreach (var item in product.Items)
                        {
                            ingredient.Ingredients.Add(item.Name);
                        }

                        ingredients.Add(ingredient);
                    }                                       
                }

            }

            return Ok(ingredients);
        }
    }
}