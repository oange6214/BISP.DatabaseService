using BISP.Infra.Entity.Entities;
using BISP.Service.IRepository;
using BISP.ServiceInterface;

namespace BISP.Service;

public class RecipeListService : IRecipeListService
{
    private readonly IRepository<Recipe> _recipeRepository;


    public RecipeListService(IRepository<Recipe> recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<IEnumerable<Recipe>> GetRecipesAsync()
    {
        return await _recipeRepository.GetAllAsync();
    }

    public async Task<Recipe> GetRecipeByGuidAsync(Guid Guid)
    {
        return await _recipeRepository.GetByIdAsync(Guid);
    }

    public async Task AddRecipeAsync(Recipe recipe)
    {
        await _recipeRepository.InsertAsync(recipe);
    }

    public async Task AddRecipesAsync(IEnumerable<Recipe> recipes)
    {
        await _recipeRepository.InsertRangeAsync(recipes);
    }

    public async Task UpdateRecipesAsync(Recipe recipe)
    {
        await _recipeRepository.Update(recipe);
    }

    public async Task DeleteRecipe(Guid guid)
    {
        await _recipeRepository.Delete(guid);
    }
}