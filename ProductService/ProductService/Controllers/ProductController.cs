using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var productDetailsList = await _unitOfWork.Products.GetAll();
            return Ok(productDetailsList);

           // return Ok(new List<Product>() { new Product { Id=1,Name="Samsung", Price =11123}, new Product { Id = 2, Name = "Nokia", Price = 34534 } });
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            if (productId > 0)
            {
                var product = await _unitOfWork.Products.GetById(productId);
                if (product != null)
                {
                    return Ok(product);
                }
                
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {            
            await _unitOfWork.Products.Add(product);
            var result = _unitOfWork.Save();

            var isProductCreated = result>0;

            if (isProductCreated)
            {
                return Ok(isProductCreated);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if(product!= null)
            {
                var productDetails = await _unitOfWork.Products.GetById(product.Id);
                if (productDetails != null)
                {
                    productDetails.Name = product.Name;
                    productDetails.Price = product.Price;

                    _unitOfWork.Products.Update(productDetails);

                    var result = _unitOfWork.Save();

                    
                    if (result > 0)
                    {
                        return Ok(true);
                    }
                    return BadRequest();

                }
            }
            return BadRequest();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (productId > 0)
            {
                var productDetails = await _unitOfWork.Products.GetById(productId);
                if (productDetails != null)
                {
                    _unitOfWork.Products.Delete(productDetails);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return Ok(true);
                    else
                        return  BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
