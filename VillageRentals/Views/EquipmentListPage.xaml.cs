using System.Collections.ObjectModel;
using VillageRentals.Data;
using VillageRentals.Models;

namespace VillageRentals.Views;


public partial class EquipmentListPage : ContentPage
{
    private VillageRentalsDatabase database;

    public ObservableCollection<Equipment> Equipments { get; set; } = [];

    public EquipmentListPage(VillageRentalsDatabase villageRentalsDatabase)
    {
        InitializeComponent();
        database = villageRentalsDatabase;
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var equipments = await database.GetEquipmentsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Equipments.Clear();
            equipments.ForEach(Equipments.Add);
        });
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EquipmentPage), true, new Dictionary<string, object>
        {
            ["Equipment"] = new Equipment()
        });
    }

    private async void equipmentsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Equipment equipment) return;

        await Shell.Current.GoToAsync(nameof(EquipmentPage), true, new Dictionary<string, object>
        {
            ["Equipment"] = equipment
        });
    }
}