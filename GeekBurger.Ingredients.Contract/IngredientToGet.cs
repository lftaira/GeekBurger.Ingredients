using System;
using System.Collections.Generic;

namespace GeekBurger.Ingredients.Contract
{
    public class IngredientToGet
    {
        public Guid ProductId { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
