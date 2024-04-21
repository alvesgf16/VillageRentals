namespace VillageRentals
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.EquipmentPage), typeof(Views.EquipmentPage));
            Routing.RegisterRoute(nameof(Views.CategoryPage), typeof(Views.CategoryPage));
        }
    }
}
