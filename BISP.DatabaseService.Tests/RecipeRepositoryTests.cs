
using BISP.Infra.EfCore;
using BISP.Infra.Entity.Data;
using BISP.Infra.Entity.Entities;
using BISP.Service.IRepository;

namespace BISP.DatabaseService.Tests;

public class RecipeRepositoryTests
{
    private readonly BispContext _dbContext;
    private readonly IRepository<Recipe> _repository;

    public RecipeRepositoryTests()
    {
        _dbContext = new BispContext();
        _repository = new EfRepository<Recipe>(_dbContext);
    }

    [Fact]
    public async Task InsertAsyncAndGetByIdAysnc_ShouldReturnRecipe_WhenRecipeExists()
    {
        // Arrange
        var recipe = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test GetByIdAysnc Recipe", ItemValue = 100, CreateAt = DateTime.Now.ToUniversalTime() };
        await _repository.InsertAsync(recipe);


        // Act
        var result = await _repository.GetByIdAsync(recipe.Guid);


        // Assert
        Assert.NotNull(result);
        Assert.Equal(recipe, result);
    }

    [Fact]
    public async Task InsertRangeAsyncAndGetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var recipe1 = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test GetAllAsync Recipe 1", ItemValue = 111, CreateAt = DateTime.Now.ToUniversalTime() };
        var recipe2 = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test GetAllAsync Recipe 2", ItemValue = 111, CreateAt = DateTime.Now.ToUniversalTime() };

        await _repository.InsertRangeAsync(new[] { recipe1, recipe2 });


        // Act
        var result = await _repository.GetAllAsync();


        // Assert
        Assert.Contains(recipe1, result);
        Assert.Contains(recipe2, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenReicpeDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();


        // Act
        var result = await _repository.GetByIdAsync(id);


        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertRangeAsyncAndFindAsync_ShouldReturnMatchingRecipes()
    {
        // Arrange
        var recipe1 = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test FindAsync Recipe 1", ItemValue = 222, CreateAt = DateTime.Now.ToUniversalTime() };
        var recipe2 = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test FindAsync Recipe 2", ItemValue = 222, CreateAt = DateTime.Now.ToUniversalTime() };
        await _repository.InsertRangeAsync(new[] { recipe1, recipe2 });


        // Act
        var result = await _repository.FindAsync(e => e.ItemName == recipe2.ItemName);


        // Assert
        Assert.Contains(recipe2, result);
        Assert.DoesNotContain(recipe1, result);
    }

    [Fact]
    public async Task Update_ShouldReturnMatchingRecipes()
    {
        // Arrange
        var recipes = await _repository.GetAllAsync();
        var recipe = recipes.FirstOrDefault();
        recipe.ItemName = "Test Update Reicpe 1";


        // Act
        await _repository.Update(recipe);
        var result = await _repository.GetByIdAsync(recipe.Guid);


        // Assert
        Assert.Equal(recipe, result);

    }

    [Fact]
    public async Task Delete_SingleEntity_ShouldDeleteFromDatabase()
    {
        // Arrange
        var recipe = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test GetByIdAysnc Recipe", ItemValue = 100, CreateAt = DateTime.Now.ToUniversalTime() };
        await _repository.InsertAsync(recipe);


        // Act
        await _repository.Delete(recipe.Guid);

        var result = await _repository.GetByIdAsync(recipe.Guid);


        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteRange_MultipleEntities_ShouldDeleteFromDatabase()
    {
        // Arrange
        var recipe1 = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test DeleteRange Recipe 1", ItemValue = 555, CreateAt = DateTime.Now.ToUniversalTime() };
        var recipe2 = new Recipe { Guid = Guid.NewGuid(), ItemName = "Test DeleteRange Recipe 2", ItemValue = 555, CreateAt = DateTime.Now.ToUniversalTime() };

        await _repository.InsertRangeAsync(new[] { recipe1, recipe2 });

        var recipes = new List<Recipe>
        {
            recipe1,
            recipe2
        };

        // Act
        await _repository.DeleteRange(recipes);

        var result = await _repository.GetAllAsync();


        // Assert
        Assert.DoesNotContain(recipe1, result);
        Assert.DoesNotContain(recipe2, result);
    }


}