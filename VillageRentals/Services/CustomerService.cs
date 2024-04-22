using SQLite;
using VillageRentals.Models;

namespace VillageRentals.Services;

internal class CustomerService
{
    private readonly SQLiteConnection? _database;

    public CustomerService()
    {
        _database = new SQLiteConnection(Constants.DatabasePath);
        _database.CreateTable<Customer>();
    }

    public List<Customer> GetCustomers() => _database!.Table<Customer>().ToList();

    public Customer GetCustomer(int id) => _database!.Find<Customer>(id);

    public int SaveCustomer(Customer customer)
    {
        Customer? existingCustomer = GetCustomer(customer.Id);
        return existingCustomer is null ? _database!.Insert(customer) : _database!.Update(customer);
    }

    public int DeleteCustomer(Customer customer) => _database!.Delete(customer);
}
