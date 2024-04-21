using SQLite;
using VillageRentals.Models;

namespace VillageRentals.Services;

public class EquipmentService
{
    private readonly SQLiteConnection? _database;

    public EquipmentService()
    {
        _database = new SQLiteConnection(Constants.DatabasePath);
        _database.CreateTable<Category>();
    }
    public List<Equipment> GetEquipments() => _database!.Table<Equipment>().ToList();

    public List<Equipment> GetEquipmentsByCategory(int categoryId) => _database.Table<Equipment>().Where((equipment) => equipment.CategoryId == categoryId).ToList();

    public Equipment GetEquipment(int id) => _database!.Find<Equipment>(id);

    public void SaveEquipment(Equipment equipment)
    {
        Equipment? existingEquipment = GetEquipment(equipment.Id);
        if (existingEquipment is null)
        {
            _database!.Insert(equipment);
        }
        else
        {
            _database!.Update(equipment);
        }
    }

    public int DeleteEquipment(Equipment equipment) => _database!.Delete(equipment);
}