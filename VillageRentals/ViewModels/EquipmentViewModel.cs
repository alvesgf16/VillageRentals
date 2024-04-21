using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VillageRentals.Data;
using VillageRentals.Models;

namespace VillageRentals.ViewModels;

internal class EquipmentViewModel : ObservableObject, IQueryAttributable
{
    private Equipment _equipment;

    private VillageRentalsDatabase _database;

    public EquipmentViewModel()
    {
        _equipment = new Equipment();
        _database = new VillageRentalsDatabase();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public EquipmentViewModel(Equipment equipment)
    {
        _equipment = equipment;
        _database = new VillageRentalsDatabase();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public int Id => _equipment.Id;

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

        await _database.SaveEquipmentAsync(_equipment);
        await Shell.Current.GoToAsync($"..?saved={_equipment.Id}");
    }

    private async Task Delete()
    {
        if (_equipment.Id == 0) return;

        await _database.DeleteEquipmentAsync(_equipment);
        await Shell.Current.GoToAsync($"..?deleted={_equipment.Id}");
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            _equipment = await _database.GetEquipmentAsync(int.Parse(query["id"].ToString()));
            RefreshProperties();
        }
    }

    public async void Reload()
    {
        _equipment = await _database.GetEquipmentAsync(_equipment.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Description));
    }
}
