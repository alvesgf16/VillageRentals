using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VillageRentals.Services;
using VillageRentals.Models;
using System.Text.RegularExpressions;

namespace VillageRentals.ViewModels;

internal partial class CustomerViewModel : ObservableObject, IQueryAttributable
{
    private readonly CustomerService _database;

    private Customer _customer;

    public CustomerViewModel()
    {
        _customer = new Customer();
        _database = new CustomerService();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public CustomerViewModel(Customer customer)
    {
        _customer = customer;
        _database = new CustomerService();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public int Id => _customer.Id;

    public string FirstName
    {
        get => _customer.FirstName;
        set
        {
            if (_customer.FirstName != value)
            {
                _customer.FirstName = value;
                OnPropertyChanged();
            }
        }
    }

    public string LastName
    {
        get => _customer.LastName;
        set
        {
            if (_customer.LastName != value)
            {
                _customer.LastName = value;
                OnPropertyChanged();
            }
        }
    }

    public string ContactPhone
    {
        get => _customer.ContactPhone;
        set
        {
            if (_customer.ContactPhone != value)
            {
                _customer.ContactPhone = value;
                OnPropertyChanged();
            }
        }
    }

    public string Email
    {
        get => _customer.Email;
        set
        {
            if (_customer.Email != value)
            {
                _customer.Email = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsBanned
    {
        get => _customer.IsBanned;
        set
        {
            if (_customer.IsBanned != value)
            {
                _customer.IsBanned = value;
                OnPropertyChanged();
            }
        }
    }

    public bool HasDiscount
    {
        get => _customer.HasDiscount;
        set
        {
            if (_customer.HasDiscount != value)
            {
                _customer.HasDiscount = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(_customer.FirstName))
        {
            await Shell.Current.DisplayAlert("Name Required", "Please enter a first name for the customer.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_customer.LastName))
        {
            await Shell.Current.DisplayAlert("Name Required", "Please enter a last name for the customer.", "OK");
            return;
        }

        if (!PhoneRegex().IsMatch(_customer.ContactPhone))
        {
            await Shell.Current.DisplayAlert("Description Required", "Please enter a valid phone number for the customer.", "OK");
            return;
        }

        if (!EmailRegex().IsMatch(_customer.Email))
        {
            await Shell.Current.DisplayAlert("Description Required", "Please enter a valid email for the customer.", "OK");
            return;
        }

        _database.SaveCustomer(_customer);
        await Shell.Current.GoToAsync($"..?saved={_customer.Id}");
    }

    private async Task Delete()
    {
        if (_customer.Id == 0) return;

        _database.DeleteCustomer(_customer);
        await Shell.Current.GoToAsync($"..?deleted={_customer.Id}");
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id))
        {
            _customer = _database.GetCustomer(int.Parse(id.ToString()));
            RefreshProperties();
        }
        else
        {
            List<Customer> customers = _database.GetCustomers();
            _customer.Id = customers.Count + 1001;
            RefreshProperties();
        }
    }

    public async void Reload()
    {
        _customer = _database.GetCustomer(_customer.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(FirstName));
        OnPropertyChanged(nameof(LastName));
        OnPropertyChanged(nameof(ContactPhone));
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(IsBanned));
        OnPropertyChanged(nameof(HasDiscount));
    }

    [GeneratedRegex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
