using SQLite;

namespace VillageRentals.Models;

public class Equipment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
