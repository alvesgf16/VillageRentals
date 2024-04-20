namespace VillageRentals
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.EquipmentPage), typeof(Views.EquipmentPage));
        }
    }
}
