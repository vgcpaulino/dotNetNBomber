using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[Route("/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly string[] jsonFiles =
    [
        "data/products_1.json",
        "data/products_2.json",
        "data/products_3.json",
        "data/products_4.json",
        "data/products_5.json",
        "data/products_6.json",
        "data/products_7.json",
        "data/products_8.json",
        "data/products_9.json",
        "data/products_10.json",
    ];

    [HttpGet("{id}", Name = "GetProduct")]
    public ActionResult<ProductModel> GetProduct(string id)
    {
        foreach (var file in jsonFiles)
        {
            List<ProductModel>? products = JsonConvert.DeserializeObject<List<ProductModel>>(System.IO.File.ReadAllText(file));
            
            if (products != null) {
                ProductModel? product = products?.FirstOrDefault(p => p.Id == id);
            
                if (product != null)
                {
                    return Ok(product);
                }
            }
        }
        return NotFound();    
    }
}