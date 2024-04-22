using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VillageRentals.Services;
using VillageRentals.Models;

namespace VillageRentals.ViewModels;

internal class CategoryViewModel : ObservableObject, IQueryAttributable
{
    private readonly CategoryService _database;

    private Category _category;

    public CategoryViewModel()
    {
        _category = new Category();
        _database = new CategoryService();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public CategoryViewModel(Category category)
    {
        _category = category;
        _database = new CategoryService();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public int Id => _category.Id;

    public string Name
    {
        get => _category.Name;
        set
        {
            if (_category.Name != value)
            {
                _category.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(_category.Name))
        {
            await Shell.Current.DisplayAlert("Name Required", $"Please enter a name for the category with id {_category.Id}.", "OK");
            return;
        }

        _database.SaveCategory(_category);
        await Shell.Current.GoToAsync($"..?saved={_category.Id}");
    }

    private async Task Delete()
    {
        if (_category.Id == 0) return;

        _database.DeleteCategory(_category);
        await Shell.Current.GoToAsync($"..?deleted={_category.Id}");
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id))
        {
            _category = _database.GetCategory(int.Parse(id.ToString()));
            RefreshProperties();
        }
        else
        {
            List<Category> categories = _database.GetCategories();
            _category.Id = categories.Count * 10 + 10;
            RefreshProperties();
        }
    }

    public async void Reload()
    {
        _category = _database.GetCategory(_category.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
    }
}
