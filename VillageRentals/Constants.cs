namespace VillageRentals;

internal class Constants
{
    public const string DatabaseFilename = "VillageRentals.db3";

    public static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFilename);
}
