using SQLite;
using VillageRentals.Models;

namespace VillageRentals.Services;

public class CategoryService
{
    private readonly SQLiteConnection? _database;

    public CategoryService()
    {
        _database = new SQLiteConnection(Constants.DatabasePath);
        _database.CreateTable<Category>();
    }

    public List<Category> GetCategories() => _database!.Table<Category>().ToList();

    public Category GetCategory(int id) => _database!.Find<Category>(id);

    public int SaveCategory(Category category)
    {
        Category? existingCategory = GetCategory(category.Id);
        return existingCategory is null ? _database!.Insert(category) : _database!.Update(category);
    }

    public int DeleteCategory(Category category) => _database!.Delete(category);
}