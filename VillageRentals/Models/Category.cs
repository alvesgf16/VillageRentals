using SQLite;

namespace VillageRentals.Models;

public class Category
{
    [PrimaryKey]
    public int Id { get; set; }

    public string Name { get; set; }
}
