using VillageRentals.Data;
using VillageRentals.Models;

namespace VillageRentals.Views;

[QueryProperty("Equipment", "Equipment")]
public partial class EquipmentPage : ContentPage
{
    private VillageRentalsDatabase _database;

    public EquipmentPage(VillageRentalsDatabase villageRentalsDatabase)
	{
		InitializeComponent();
		_database = villageRentalsDatabase;
	}

    public Equipment Equipment
    {
        get => BindingContext as Equipment;
        set => BindingContext = value;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Equipment.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the equipment.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Equipment.Description))
        {
            await DisplayAlert("Description Required", "Please enter a description for the equipment.", "OK");
            return;
        }

        await _database.SaveEquipmentAsync(Equipment);
        await Shell.Current.GoToAsync("..");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (Equipment.Id == 0) return;

        await _database.DeleteEquipmentAsync(Equipment);
        await Shell.Current.GoToAsync("..");
    }
}