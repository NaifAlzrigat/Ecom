using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _work.CategoryRepository.GetAllAsync();
                if (data is null)
                    return BadRequest(new ResponseAPI(400));
                return Ok(data);
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
                var data = await _work.CategoryRepository.GetByIdAsync(id);
                return data is null
                    ? NotFound(new ResponseAPI(404, "Category not found"))
                    : Ok(data);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                await _work.CategoryRepository.AddAsync(category);
                return Ok(new ResponseAPI(200, "Category has been added !"));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-category")]
        public async Task<IActionResult> Update(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(updateCategoryDTO);

                await _work.CategoryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Category has been updated !"));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-category")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _work.CategoryRepository.DeleteAsync(id);
                return Ok( new ResponseAPI(200, "category has been deleted !"));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
