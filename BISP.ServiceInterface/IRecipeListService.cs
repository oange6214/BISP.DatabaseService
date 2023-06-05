using BISP.Infra.Entity.Entities;

namespace BISP.ServiceInterface;

public interface IRecipeListService
{
    Task<IEnumerable<Recipe>> GetRecipesAsync();

    Task<Recipe> GetRecipeByGuidAsync(Guid Guid);

    Task AddRecipeAsync(Recipe recipe);

    Task AddRecipesAsync(IEnumerable<Recipe> recipes);

    Task UpdateRecipesAsync(Recipe recipe);

    Task DeleteRecipe(Guid guid);

}