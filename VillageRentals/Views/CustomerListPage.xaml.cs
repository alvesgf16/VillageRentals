namespace VillageRentals.Views;

public partial class CustomerListPage : ContentPage
{
	public CustomerListPage()
	{
		InitializeComponent();
	}
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        customersCollection.SelectedItem = null;
    }
}