using SQLite;

namespace VillageRentals.Models;

internal class Equipment
{
    [PrimaryKey]
    public int Id { get; set; }

    public Category Category { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
