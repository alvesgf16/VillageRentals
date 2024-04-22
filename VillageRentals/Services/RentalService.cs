using SQLite;
using VillageRentals.Models;

namespace VillageRentals.Services;

public class RentalService
{
    private readonly SQLiteConnection? _database;

    public RentalService()
    {
        _database = new SQLiteConnection(Constants.DatabasePath);
        _database.CreateTable<Rental>();
    }
    public List<Rental> GetRentals() => _database!.Table<Rental>().ToList();

    public Rental GetRental(int id) => _database!.Find<Rental>(id);

    public void SaveRental(Rental equipment)
    {
        Rental? existingRental = GetRental(equipment.Id);
        if (existingRental is null)
        {
            _database!.Insert(equipment);
        }
        else
        {
            _database!.Update(equipment);
        }
    }

    public int DeleteRental(Rental equipment) => _database!.Delete(equipment);
}