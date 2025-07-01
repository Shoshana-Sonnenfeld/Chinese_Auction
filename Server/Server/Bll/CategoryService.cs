using Server.Bll.Interfaces;
using Server.Bll.Interfaces;
using Server.Dal.Interfaces;
using Server.Models;
using Microsoft.Extensions.Logging;

namespace Server.Bll.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryDal _categoryDal;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryDal categoryDal, ILogger<CategoryService> logger)
        {
            _categoryDal = categoryDal;
            _logger = logger;
        }

        public async Task Add(Category category)
        {
            _logger.LogInformation($"Adding new category: {category?.Name}");
            await _categoryDal.Add(category);
            _logger.LogInformation("Category added successfully");
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation($"Deleting category with id {id}");
            await _categoryDal.Delete(id);
            _logger.LogInformation($"Category with id {id} deleted successfully");
        }

        public async Task<IEnumerable<Category>> Get()
        {
            _logger.LogInformation("Getting all categories");
            var result = await _categoryDal.Get();
            _logger.LogInformation($"Returned {result.Count()} categories");
            return result;
        }

        public async Task<Category> Get(int id)
        {
            _logger.LogInformation($"Getting category with id {id}");
            var result = await _categoryDal.Get(id);
            _logger.LogInformation($"Returned category with id {id}");
            return result;
        }

        public async Task<bool> NameExist(string name)
        {
            _logger.LogInformation($"Checking if category name exists: {name}");
            var result = await _categoryDal.NameExist(name);
            _logger.LogInformation($"Category name exists: {result}");
            return result;
        }

        public async Task Update(int id, Category category)
        {
            _logger.LogInformation($"Updating category with id {id}");
            await _categoryDal.Update(id, category);
            _logger.LogInformation($"Category with id {id} updated successfully");
        }
    }
}
