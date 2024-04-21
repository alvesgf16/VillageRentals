using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VillageRentals.Services;

namespace VillageRentals.ViewModels;

internal class CategoryListViewModel : IQueryAttributable
{
    private readonly CategoryService _database;

    public CategoryListViewModel()
    {
        _database = new CategoryService();
        SetCategories();
        AddCommand = new AsyncRelayCommand(AddCategoryAsync);
        SelectCategoryCommand = new AsyncRelayCommand<CategoryViewModel>(SelectCategoryAsync);
    }

    public ObservableCollection<CategoryViewModel> Categories { get; set; } = [];

    public ICommand AddCommand { get; }

    public ICommand SelectCategoryCommand { get; }

    private async void SetCategories()
    {
        var categories = _database.GetCategories();

        Categories.Clear();
        foreach (var category in categories)
        {
            Categories.Add(new CategoryViewModel(category));
        }
    }

    private async Task AddCategoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CategoryPage));
    }

    private async Task SelectCategoryAsync(CategoryViewModel category)
    {
        if (category is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.CategoryPage)}?id={category.Id}");
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            int categoryId = int.Parse(query["deleted"].ToString()!);
            CategoryViewModel? matchedCategory = Categories.Where((category) => category.Id == categoryId)
                                                            .FirstOrDefault();
            // If category exists, delete it
            if (matchedCategory is not null) Categories.Remove(matchedCategory);
        }
        else if (query.ContainsKey("saved"))
        {
            int categoryId = int.Parse(query["saved"].ToString()!);
            CategoryViewModel? matchedCategory = Categories.Where((category) => category.Id == categoryId)
                                                             .FirstOrDefault();

            // If category is found, update it
            if (matchedCategory is not null)
            {
                matchedCategory.Reload();
            }
            // If category isn't found, it's new; add it.
            else Categories.Add(new CategoryViewModel(_database.GetCategory(categoryId)));
        }
    }
}
