using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VillageRentals.Services;

namespace VillageRentals.ViewModels;

internal class CustomerListViewModel : IQueryAttributable
{
    private readonly CustomerService _database;

    public CustomerListViewModel()
    {
        _database = new CustomerService();
        SetCustomers();
        AddCommand = new AsyncRelayCommand(AddCustomerAsync);
        SelectCustomerCommand = new AsyncRelayCommand<CustomerViewModel>(SelectCustomerAsync);
    }

    public ObservableCollection<CustomerViewModel> Customers { get; set; } = [];

    public ICommand AddCommand { get; }

    public ICommand SelectCustomerCommand { get; }

    private async void SetCustomers()
    {
        var customers = _database.GetCustomers();

        Customers.Clear();
        foreach (var customer in customers)
        {
            Customers.Add(new CustomerViewModel(customer));
        }
    }

    private async Task AddCustomerAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CustomerPage));
    }

    private async Task SelectCustomerAsync(CustomerViewModel customer)
    {
        if (customer is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.CustomerPage)}?id={customer.Id}");
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            int customerId = int.Parse(query["deleted"].ToString()!);
            CustomerViewModel? matchedCustomer = Customers.Where((customer) => customer.Id == customerId)
                                                            .FirstOrDefault();
            // If customer exists, delete it
            if (matchedCustomer is not null) Customers.Remove(matchedCustomer);
        }
        else if (query.ContainsKey("saved"))
        {
            int customerId = int.Parse(query["saved"].ToString()!);
            CustomerViewModel? matchedCustomer = Customers.Where((customer) => customer.Id == customerId)
                                                             .FirstOrDefault();

            // If customer is found, update it
            if (matchedCustomer is not null)
            {
                matchedCustomer.Reload();
            }
            // If customer isn't found, it's new; add it.
            else Customers.Add(new CustomerViewModel(_database.GetCustomer(customerId)));
        }
    }
}
