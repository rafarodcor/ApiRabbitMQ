using ApiRabbitMq.Models;
using ApiRabbitMq.RabbitMQ;
using ApiRabbitMq.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiRabbitMq.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IRabbitMQProducer _rabitMQProducer;

    public ProductsController(IProductService productService, IRabbitMQProducer rabitMQProducer)
    {
        _productService = productService;
        _rabitMQProducer = rabitMQProducer;
    }

    [HttpGet("productlist")]
    public IEnumerable<Product> ProductList() => _productService.GetProductList();

    [HttpGet("getproductbyid")]
    public Product? GetProductById(int Id) => _productService.GetProductById(Id);

    [HttpPost("addproduct")]
    public Product AddProduct(Product product)
    {
        var productData = _productService.AddProduct(product);
        _rabitMQProducer.SendProductMessage(productData);
        return productData;
    }

    [HttpPut("updateproduct")]
    public Product UpdateProduct(Product product) => _productService.UpdateProduct(product);

    [HttpDelete("deleteproduct")]
    public bool DeleteProduct(int Id) => _productService.DeleteProduct(Id);
}