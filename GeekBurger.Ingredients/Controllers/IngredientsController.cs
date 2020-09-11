using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Ingredients.Contract;
using GeekBurger.Ingredients.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Ingredients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        [HttpGet("api")]
        public IActionResult Index()
        {
            return Ok("teste");
        }

        [HttpGet("api/products/byrestrictions/{productRequest}")]
        public IActionResult GetIngredient([FromQuery] ProductRequest productRequest)
        {
            return Ok(new List<IngredientToGet>() { new IngredientToGet() { ProductId = 1122, Ingredients = new List<string> { "soy", "gluten" } },
                                                    new IngredientToGet() { ProductId = 1122, Ingredients = new List<string> { "soy", "gluten" } } });
        }

    }
}
