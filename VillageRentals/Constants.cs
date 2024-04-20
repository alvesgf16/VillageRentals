namespace VillageRentals;

internal class Constants
{
    public const string DatabaseFilename = "VillageRentals.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath
    {
        get
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new(baseDirectory);

            while (directory.Name != "VillageRentals")
            {
                directory = directory.Parent!;

                if (directory is null) throw new InvalidOperationException("Could not find the project root directory.");
            }

            string root = directory.FullName;
            string dataDirectory = Path.Combine(root, "Data");

            return Path.Combine(dataDirectory, DatabaseFilename);
        }
    }
}
