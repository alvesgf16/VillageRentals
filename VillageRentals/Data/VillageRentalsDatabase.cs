using SQLite;
using VillageRentals.Models;

namespace VillageRentals.Data;

internal class VillageRentalsDatabase
{
    SQLiteAsyncConnection? Database;

    public VillageRentalsDatabase() { }

    async Task Init()
    {
        if (Database is not null) return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<Equipment>();
    }

    public async Task<List<Equipment>> GetEquipmentsAsync()
    {
        await Init();
        return await Database!.Table<Equipment>().ToListAsync();
    }

    public async Task<Equipment> GetEquipmentAsync(int id)
    {
        await Init();
        return await Database!.FindAsync<Equipment>(id);
    }

    public async Task<int> SaveEquipmentAsync(Equipment equipment)
    {
        await Init();
        return equipment.Id == 0 ? await Database!.InsertAsync(equipment) : await Database!.UpdateAsync(equipment);
    }

    public async Task<int> DeleteEquipmentAsync(Equipment equipment)
    {
        await Init();
        return await Database!.DeleteAsync(equipment);
    }
}
