using SQLite;

namespace VillageRentals.Models;

internal class Customer
{
    [PrimaryKey]
    public int Id { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string ContactPhone { get; set; }

    public string Email { get; set; }

    public bool IsBanned { get; set; }

    public bool HasDiscount { get; set; }
}
