using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Ingredients.Model
{
    public class ProductRequest
    {
        public string StoreName { get; set; }

        public List<string> Restrictions { get; set; }
    }
}
