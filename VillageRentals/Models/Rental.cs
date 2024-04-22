using SQLite;

namespace VillageRentals.Models;

public class Rental
{
    [PrimaryKey]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int CustomerId { get; set; }

    public int EquipmentId { get; set; }

    public DateTime RentalDate { get; set; }

    public DateTime ReturnDate { get; set; }

    public decimal Cost { get; set; }
}
