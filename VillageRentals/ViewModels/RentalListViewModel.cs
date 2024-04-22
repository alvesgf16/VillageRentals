using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VillageRentals.Services;

namespace VillageRentals.ViewModels;

internal class RentalListViewModel : IQueryAttributable
{
    private readonly RentalService _database;

    public RentalListViewModel()
    {
        _database = new RentalService();
        SetRentals();
        AddCommand = new AsyncRelayCommand(AddRentalAsync);
        SelectRentalCommand = new AsyncRelayCommand<RentalViewModel>(SelectRentalAsync);
    }

    public ObservableCollection<RentalViewModel> Rentals { get; set; } = [];

    public ICommand AddCommand { get; }

    public ICommand SelectRentalCommand { get; }

    private void SetRentals()
    {
        var rentals = _database.GetRentals();

        Rentals.Clear();
        foreach (var rental in rentals)
        {
            Rentals.Add(new RentalViewModel(rental));
        }
    }

    private async Task AddRentalAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.RentalPage));
    }

    private async Task SelectRentalAsync(RentalViewModel rental)
    {
        if (rental is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.RentalPage)}?id={rental.Id}");
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            int rentalId = int.Parse(query["deleted"].ToString()!);
            RentalViewModel? matchedRental = Rentals.Where((rental) => rental.Id == rentalId)
                                                            .FirstOrDefault();
            // If rental exists, delete it
            if (matchedRental is not null) Rentals.Remove(matchedRental);
        }
        else if (query.ContainsKey("saved"))
        {
            int rentalId = int.Parse(query["saved"].ToString()!);
            RentalViewModel? matchedRental = Rentals.Where((rental) => rental.Id == rentalId)
                                                             .FirstOrDefault();

            // If rental is found, update it
            if (matchedRental is not null)
            {
                matchedRental.Reload();
            }
            // If rental isn't found, it's new; add it.
            else Rentals.Add(new RentalViewModel(_database.GetRental(rentalId)));
        }
    }
}
