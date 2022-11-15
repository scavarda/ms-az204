using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sqlapp.Models;
using sqlapp.Services;
using System.Collections.Generic;

namespace sqlapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        public bool IsBeta;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public List<Product> Products; 
        
        public void OnGet()
        {
            IsBeta = _productService.IsBeta().GetAwaiter().GetResult();
            Products = _productService.GetProductList().GetAwaiter().GetResult();
        }
    }
}