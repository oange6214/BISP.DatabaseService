using BISP.Base;
using BISP.Client.WPF.Models;
using BISP.Infra.Entity.Entities;
using BISP.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BISP.Client.WPF.ViewModel;

public class RecipeListViewModel : ViewModelBase
{
    #region Fields

    private readonly IRecipeListService _recipeListService;
    private readonly IHost _host;
    private Recipe _selectedItem;
    private Recipe _createItem;
    private ObservableCollection<Recipe> _recipes = new();

    #endregion


    #region Ctors

    public RecipeListViewModel(IHost host, IRecipeListService recipeListService)
    {
        _host = host;
        _recipeListService = recipeListService;
        _operatingState = new OperatingState()
        {
            Add = true,
            Remove = false,
            Update = false,
            Cancel = false
        };

        AddCommand = new AsyncCommand<object>(Add);
        RemoveCommand = new AsyncCommand<object>(Remove);
        UpdateCommand = new AsyncCommand<object>(Update);
        CancelCommand = new AsyncCommand<object>(Cancel);

        CreateItem = new Recipe();
        Messages = new ObservableCollection<string>();

        GetRecipes().ConfigureAwait(false);
    }

    #endregion


    #region Properties

    private ObservableCollection<string> _messages;

    public ObservableCollection<string> Messages
    {
        get { return _messages; }
        set { _messages = value; }
    }



    public ObservableCollection<Recipe> Recipes
    {
        get => _recipes;
        set
        { 
            _recipes = value;
            OnPropertyChanged();
        }
    }

    public Recipe SelectedItem

    {
        get { return _selectedItem; }
        set 
        { 
            _selectedItem = value; 
            
            OnPropertyChanged();
            Set(_selectedItem);
        }
    }

    public Recipe CreateItem

    {
        get { return _createItem; }
        set
        {
            _createItem = value;
            OnPropertyChanged();
        }
    }


    private OperatingState _operatingState;
    public OperatingState OperatingState

    {
        get { return _operatingState; }
        set { _operatingState = value; }
    }

    #endregion


    #region Commands

    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand CancelCommand { get; }

    #endregion


    #region Public Methods

    public void Set(Recipe recipe)
    {
        if (recipe != null && recipe.Guid  != Guid.Empty)
        {
            CreateItem.Guid = recipe.Guid;
            CreateItem.ItemName = recipe.ItemName;
            CreateItem.ItemValue = recipe.ItemValue;
            CreateItem.CreateAt = recipe.CreateAt;

            OnPropertyChanged(nameof(CreateItem));

            _operatingState.Add = false;
            _operatingState.Remove = true;
            _operatingState.Update = true;
            _operatingState.Cancel = true;
        }
    }

    internal async Task Add(object obj)
    {
        try
        {
            if (CreateItem.ItemName != null)
            {
                CreateItem.Guid = Guid.NewGuid();
                CreateItem.CreateAt = DateTime.Now;

                await _recipeListService.AddRecipeAsync(CreateItem);

                Recipes.Add(CreateItem);

                ClearCurrentRecipe();
            }
        }
        catch (Exception ex)
        {
            Messages.Add(ex.ToString());
        }
    }

    internal async Task Remove(object obj)
    {
        try
        {
            await _recipeListService.DeleteRecipe(CreateItem.Guid);

            DeleteRecipe(CreateItem);

            ClearCurrentRecipe();
        }
        catch (Exception ex)
        {
            Messages.Add(ex.ToString());
        }
    }

    internal async Task Update(object obj)
    {
        try
        {
            var recipeToUpdate = _recipes.FirstOrDefault(r => r.Guid == CreateItem.Guid);
            recipeToUpdate.ItemValue = CreateItem.ItemValue;
            recipeToUpdate.ItemName = CreateItem.ItemName;

            await _recipeListService.UpdateRecipesAsync(recipeToUpdate);

            UpdateRecipe(CreateItem);

            ClearCurrentRecipe();
        }
        catch (Exception ex)
        {
            Messages.Add(ex.ToString());
        }
    }

    internal async Task Cancel(object obj)
    {
        ClearCurrentRecipe();
    }

    #endregion


    #region Private Methods

    private void DeleteRecipe(Recipe newRecipe)
    {
        var recipeToUpdate = _recipes.FirstOrDefault(r => r.Guid == newRecipe.Guid);

        if (recipeToUpdate != null)
        {
            int index = _recipes.IndexOf(recipeToUpdate);
            _recipes.RemoveAt(index);

            OnPropertyChanged(nameof(Recipes));
        }
    }


    private void UpdateRecipe(Recipe newRecipe)
    {
        var recipeToUpdate = _recipes.FirstOrDefault(r => r.Guid == newRecipe.Guid);

        if (recipeToUpdate != null)
        {
            int index = _recipes.IndexOf(recipeToUpdate);
            _recipes[index] = newRecipe;

            OnPropertyChanged(nameof(Recipes));
        }
    }

    private void ClearCurrentRecipe()
    {
        SelectedItem = null;
        CreateItem = new Recipe();

        _operatingState.Add = true;
        _operatingState.Remove = false;
        _operatingState.Update = false;
        _operatingState.Cancel = false;
    }

    private async Task GetRecipes()
    {
        try
        {
            Recipes = new ObservableCollection<Recipe>(await _recipeListService.GetRecipesAsync());
        }
        catch (Exception ex)
        {
            Messages.Add(ex.Message.ToString());
        }
    }

    #endregion


}
