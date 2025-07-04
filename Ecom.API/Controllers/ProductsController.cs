using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _work.ProductRepository
                    .GetAllAsync(x => x.Category, x => x.Photos);
                if (products == null)
                    return BadRequest(new ResponseAPI(400));
                var result = _mapper.Map<List<ProductDTO>>(products);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _work.ProductRepository.GetByIdAsync(id,x=>x.Category,x=>x.Photos);
                if (product == null)
                    return BadRequest(new ResponseAPI(400));
                var result = _mapper.Map<ProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> Add(AddProductDTO productDTO)
        {
            try
            {
                await _work.ProductRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400));
            }
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await _work.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(400);
            }
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product =await _work.ProductRepository.GetByIdAsync(id,x=>x.Category,x=>x.Photos);
                if (product == null) return BadRequest(new ResponseAPI(400));
                await _work.ProductRepository.DeleteAsync(product);
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(400);
            }
        }
    }
}
