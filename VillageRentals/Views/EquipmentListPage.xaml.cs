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