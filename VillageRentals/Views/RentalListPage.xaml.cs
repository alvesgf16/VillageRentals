namespace VillageRentals.Views;

public partial class RentalListPage : ContentPage
{
	public RentalListPage()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        rentalsCollection.SelectedItem = null;
    }
}