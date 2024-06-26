﻿using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VillageRentals.Services;

namespace VillageRentals.ViewModels;

internal class EquipmentListViewModel : IQueryAttributable
{
    private readonly EquipmentService _database;

    public EquipmentListViewModel()
    {
        _database = new EquipmentService();
        SetEquipments();
        AddCommand = new AsyncRelayCommand(AddEquipmentAsync);
        SelectEquipmentCommand = new AsyncRelayCommand<EquipmentViewModel>(SelectEquipmentAsync);
    }

    public ObservableCollection<EquipmentViewModel> Equipments { get; set; } = [];

    public ICommand AddCommand { get; }

    public ICommand SelectEquipmentCommand { get; }

    private void SetEquipments()
    {
        var equipments = _database.GetEquipments();
        equipments = equipments.OrderBy((equipment) => equipment.CategoryId).ToList();

        Equipments.Clear();
        foreach (var equipment in equipments)
        {
            Equipments.Add(new EquipmentViewModel(equipment));
        }
    }

    private async Task AddEquipmentAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.EquipmentPage));
    }

    private async Task SelectEquipmentAsync(EquipmentViewModel equipment)
    {
        if (equipment is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.EquipmentPage)}?id={equipment.Id}");
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            int equipmentId = int.Parse(query["deleted"].ToString()!);
            EquipmentViewModel? matchedEquipment = Equipments.Where((equipment) => equipment.Id == equipmentId)
                                                            .FirstOrDefault();
            // If equipment exists, delete it
            if (matchedEquipment is not null) Equipments.Remove(matchedEquipment);
        }
        else if (query.ContainsKey("saved"))
        {
            int equipmentId = int.Parse(query["saved"].ToString()!);
            EquipmentViewModel? matchedEquipment = Equipments.Where((equipment) => equipment.Id == equipmentId)
                                                             .FirstOrDefault();

            // If equipment is found, update it
            if (matchedEquipment is not null)
            {
                matchedEquipment.Reload();
            }
            // If equipment isn't found, it's new; add it.
            else Equipments.Add(new EquipmentViewModel(_database.GetEquipment(equipmentId)));
        }
    }
}
