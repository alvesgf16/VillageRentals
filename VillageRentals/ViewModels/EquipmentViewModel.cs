using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VillageRentals.Services;
using VillageRentals.Models;

namespace VillageRentals.ViewModels;

internal partial class EquipmentViewModel : ObservableObject, IQueryAttributable
{
    private readonly EquipmentService _equipmentService;

    private readonly CategoryService _categoryService;

    private Equipment _equipment;

    private Category _selectedCategory;

    public EquipmentViewModel()
    {
        _equipment = new Equipment();
        _equipmentService = new EquipmentService();
        _categoryService = new CategoryService();
        SetCategories();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public EquipmentViewModel(Equipment equipment)
    {
        _equipment = equipment;
        _equipmentService = new EquipmentService();
        _categoryService = new CategoryService();
        SetCategories(equipment.Id);
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public ObservableCollection<Category> Categories { get; } = [];

    public string CategoryName => _categoryService.GetCategory(_equipment.CategoryId).Name;
    
    public int Id
    {
        get => _equipment.Id;
        private set
        {
            if (_equipment.Id != value)
            {
                _equipment.Id = value;
                OnPropertyChanged();
            }
        }
    }

    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                SetObservableProperty(ref _selectedCategory, value);
                AssignEquipmentToCategory(value);
            }
        }
    }

    public string Name
    {
        get => _equipment.Name;
        set
        {
            if (_equipment.Name != value)
            {
                _equipment.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public string Description
    {
        get => _equipment.Description;
        set
        {
            if (_equipment.Description != value)
            {
                _equipment.Description = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal DailyRate
    {
        get => _equipment.DailyRate;
        set
        {
            if (_equipment.DailyRate != value)
            {
                _equipment.DailyRate = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(_equipment.Name))
        {
            await Shell.Current.DisplayAlert("Name Required", "Please enter a name for the equipment.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(_equipment.Description))
        {
            await Shell.Current.DisplayAlert("Description Required", "Please enter a description for the equipment.", "OK");
            return;
        }

        _equipmentService.SaveEquipment(_equipment);
        await Shell.Current.GoToAsync($"..?saved={_equipment.Id}");
    }

    private async Task Delete()
    {
        if (_equipment.Id == 0) return;

        _equipmentService.DeleteEquipment(_equipment);
        await Shell.Current.GoToAsync($"..?deleted={_equipment.Id}");
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            _equipment = _equipmentService.GetEquipment(int.Parse(query["id"].ToString()));
            RefreshProperties();
        }
    }

    public async void Reload()
    {
        _equipment = _equipmentService.GetEquipment(_equipment.Id);
        RefreshProperties();
    }

    private async void SetCategories(int categoryId = 10)
    {
        var categories = _categoryService.GetCategories();
        Categories.Clear();
        foreach (var category in categories)
        {
            Categories.Add(category);
        }
        SelectedCategory = Categories.FirstOrDefault((category) => category.Id == categoryId);
    }

    private void AssignEquipmentToCategory(Category category)
    {
        _equipment.CategoryId = category.Id;
        var categoryEquipments = _equipmentService.GetEquipmentsByCategory(category.Id);
        if (categoryEquipments.Count == 0)
        {
            Id = int.Parse($"{category.Id}1");
        }
        else
        {
            int lastIndex = int.Parse(categoryEquipments.Last().Id.ToString()[2..]);
            Id = int.Parse($"{category.Id}{lastIndex + 1}");
        }
    }

    protected void SetObservableProperty<T>(ref T field, T value,
    [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Description));
        OnPropertyChanged(nameof(DailyRate));
    }
}
