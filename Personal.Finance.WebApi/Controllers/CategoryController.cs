using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Personal.Finance.Application.Interface;
using Personal.Finance.Domain.Dtos;
using Personal.Finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Personal.Finance.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost(Name = "PostCategory")]
        public async Task<ActionResult<CategorieDto>> CreateCategory([FromBody] CategorieDto category)
        {
            try
            {
                var newCategory = CategorieDto.MapToEntity(category);
                newCategory.User = CurrentUser().Result;
                _unitOfWork.Categories.Create(newCategory);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                return StatusCode(200, CategorieDto.MapToDto(newCategory));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to save new category to Database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete(Name = "DeleteCategory")]
        public async Task<ActionResult> DeleteCategory(CategorieDto categoryDto)
        {
            try
            {
                if (!IsOwner(categoryDto.UserId,
                    (await _userManager.FindByIdAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value)
                        .ConfigureAwait(false)).Id))
                {
                    return StatusCode(401, "You don't have the permissions for this action");
                }

                await _unitOfWork.Categories.Delete(categoryDto.Id).ConfigureAwait(false);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to delete category to Database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets the list of all Categories.
        /// </summary>
        /// <returns>The list of Categories.</returns>
        // GET: api/Employee
        [HttpGet(Name = "GetAllCategories")]
        public async Task<ActionResult<IQueryable<CategorieDto>>> GetAllCategories()
        {
            IEnumerable<Category> result = await _unitOfWork.Categories
                .FindListConditionAsync(filter: u => u.UserId == CurrentUser().Result.Id || u.IsShared)
                .ConfigureAwait(false);
            return StatusCode(200, result.Select(CategorieDto.MapToDto).ToList().OrderBy(o => o.Name));
        }

        [HttpGet("{id}", Name = "GetSingleCategoryById")]
        public async Task<ActionResult<IQueryable<CategorieDto>>> GetSingleCategoryById(int id)
        {
            return StatusCode(200,
                CategorieDto.MapToDto(await _unitOfWork.Categories
                    .FindByConditionSingleAsync(filter: u =>
                        (u.Id == id && u.UserId == CurrentUser().Result.Id) || u.IsShared).ConfigureAwait(false)));
        }

        [HttpPut("{id}", Name = "PutCategory")]
        public async Task<ActionResult<CategorieDto>> UpdateCategory(int id, [FromBody] CategorieDto category)
        {
            try
            {
                var result = await _unitOfWork.Categories
                    .FindByConditionSingleAsync(filter: u =>
                        u.Id == id && u.UserId == CurrentUser().Result.Id || u.IsShared).ConfigureAwait(false);

                result = CategorieDto.MapToEntity(category);

                _unitOfWork.Categories.Update(result);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                return StatusCode(200, category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to update category to Database: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<User> CurrentUser() =>
            await _userManager.FindByIdAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}