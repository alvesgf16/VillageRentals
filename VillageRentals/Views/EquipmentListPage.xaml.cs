using System.Collections.ObjectModel;
using VillageRentals.Data;
using VillageRentals.Models;

namespace VillageRentals.Views;


public partial class EquipmentListPage : ContentPage
{
    public EquipmentListPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        equipmentsCollection.SelectedItem = null;
    }
}