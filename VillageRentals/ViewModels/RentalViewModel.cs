using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VillageRentals.Services;
using VillageRentals.Models;

namespace VillageRentals.ViewModels;

internal partial class RentalViewModel : ObservableObject, IQueryAttributable
{
    private readonly RentalService _rentalService;

    private readonly CustomerService _customerService;

    private readonly EquipmentService _equipmentService;

    private Rental _rental;

    private Customer _selectedCustomer;

    private Equipment _selectedEquipment;

    public RentalViewModel()
    {
        _rental = new Rental();
        _rentalService = new RentalService();
        _customerService = new CustomerService();
        _equipmentService = new EquipmentService();
        SetDates();
        SetCustomers();
        SetEquipments();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public RentalViewModel(Rental rental)
    {
        _rental = rental;
        _rentalService = new RentalService();
        _customerService = new CustomerService();
        _equipmentService = new EquipmentService();
        SetDates();
        SetCustomers(rental.CustomerId);
        SetEquipments(rental.EquipmentId);
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public ObservableCollection<Customer> Customers { get; } = [];

    public ObservableCollection<Equipment> Equipments { get; } = [];

    public string CustomerName
    {
        get
        {
            Customer customer = _customerService.GetCustomer(_rental.CustomerId);
            return $"{customer.FirstName} {customer.LastName}";
        }
    }

    public string EquipmentName => _equipmentService.GetEquipment(_rental.EquipmentId).Name;

    public int Id => _rental.Id;

    public DateTime Date
    {
        get => _rental.Date;
        set
        {
            if (_rental.Date != value)
            {
                _rental.Date = value;
                OnPropertyChanged();
            }
        }
    }

    public Customer SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            if (_selectedCustomer != value)
            {
                _rental.CustomerId = value.Id;
                SetObservableProperty(ref _selectedCustomer, value);
            }
        }
    }

    public Equipment SelectedEquipment
    {
        get => _selectedEquipment;
        set
        {
            if (_selectedEquipment != value)
            {
                _rental.EquipmentId = value.Id;
                CalculateCost();
                SetObservableProperty(ref _selectedEquipment, value);
            }
        }
    }

    public DateTime RentalDate
    {
        get => _rental.RentalDate;
        set
        {
            if (_rental.RentalDate != value)
            {
                _rental.RentalDate = value;
                CalculateCost();
                OnPropertyChanged();
            }
        }
    }

    public DateTime ReturnDate
    {
        get => _rental.ReturnDate;
        set
        {
            if (_rental.ReturnDate != value)
            {
                _rental.ReturnDate = value;
                CalculateCost();
                OnPropertyChanged();
            }
        }
    }

    public decimal Cost
    {
        get => _rental.Cost;
        set
        {
            if (_rental.Cost != value)
            {
                _rental.Cost = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }

    private async Task Save()
    {
        if (ReturnDate < RentalDate)
        {
            await Shell.Current.DisplayAlert("Invalid Dates", "The return date must be after the rental date.", "OK");
            return;
        }

        _rentalService.SaveRental(_rental);
        await Shell.Current.GoToAsync($"..?saved={_rental.Id}");
    }

    private async Task Delete()
    {
        if (_rental.Id == 0) return;

        _rentalService.DeleteRental(_rental);
        await Shell.Current.GoToAsync($"..?deleted={_rental.Id}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id))
        {
            _rental = _rentalService.GetRental(int.Parse(id.ToString()));
            RefreshProperties();
        }
        else
        {
            List<Rental> customers = _rentalService.GetRentals();
            _rental.Id = customers.Count + 1000;
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _rental = _rentalService.GetRental(_rental.Id);
        RefreshProperties();
    }

    private void SetCustomers(int customerId = 1001)
    {
        var customers = _customerService.GetCustomers();
        Customers.Clear();
        foreach (var customer in customers)
        {
            Customers.Add(customer);
        }
        SelectedCustomer = Customers.FirstOrDefault((customer) => customer.Id == customerId);
    }

    private void SetEquipments(int equipmentId = 101)
    {
        var equipments = _equipmentService.GetEquipments();
        Equipments.Clear();
        foreach (var equipment in equipments)
        {
            Equipments.Add(equipment);
        }
        SelectedEquipment = Equipments.FirstOrDefault((equipment) => equipment.Id == equipmentId);
    }

    private void SetDates()
    {
        if (_rental.Date ==  DateTime.MinValue)
        {
            _rental.Date = DateTime.Today;
        }

        if (_rental.RentalDate == DateTime.MinValue)
        {
            _rental.RentalDate = DateTime.Today;
        }

        if (_rental.ReturnDate == DateTime.MinValue)
        {
            _rental.ReturnDate = DateTime.Today;
        }
    }

    private void CalculateCost()
    {
        if (_selectedEquipment is not null)
        {
            var rentalSpan = (ReturnDate - RentalDate);
            Cost = _selectedEquipment.DailyRate * (decimal)rentalSpan.TotalDays;
            RefreshProperties();
        }
        else
        {
            Cost = 0;
            RefreshProperties();
        }
    }

protected void SetObservableProperty<T>(ref T field, T value,
    [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Date));
        OnPropertyChanged(nameof(SelectedCustomer));
        OnPropertyChanged(nameof(SelectedEquipment));
        OnPropertyChanged(nameof(RentalDate));
        OnPropertyChanged(nameof(ReturnDate));
        OnPropertyChanged(nameof(Cost));
    }
}
