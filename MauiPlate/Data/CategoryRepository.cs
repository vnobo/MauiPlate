using MauiPlate.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace MauiPlate.Data
{
    /// <summary>
    /// Repository class for managing categories in the database.
    /// </summary>
    public class CategoryRepository
    {
        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public CategoryRepository(ILogger<CategoryRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Initializes the database connection and creates the Category table if it does not exist.
        /// </summary>
        private async Task Init()
        {
            if (_hasBeenInitialized)
                return;

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            try
            {
                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Category (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Color TEXT NOT NULL
            );";
                await createTableCmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating Category table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        /// <summary>
        /// Retrieves a list of all categories from the database.
        /// </summary>
        /// <returns>A list of <see cref="Category"/> objects.</returns>
        public async Task<List<Category>> ListAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Category";
            var categories = new List<Category>();

            await using var reader = await selectCmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Color = reader.GetString(2)
                });
            }

            return categories;
        }

        /// <summary>
        /// Retrieves a specific category by its Id.
        /// </summary>
        /// <param name="id">The Id of the category.</param>
        /// <returns>A <see cref="Category"/> object if found; otherwise, null.</returns>
        public async Task<Category?> GetAsync(int id)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Category WHERE Id = @id";
            selectCmd.Parameters.AddWithValue("@id", id);

            await using var reader = await selectCmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Category
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Color = reader.GetString(2)
                };
            }

            return null;
        }

        /// <summary>
        /// Saves a category to the database. If the category Id is 0, a new category is created; otherwise, the existing category is updated.
        /// </summary>
        /// <param name="item">The category to save.</param>
        /// <returns>The Id of the saved category.</returns>
        public async Task<int> SaveItemAsync(Category item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var saveCmd = connection.CreateCommand();
            if (item.Id == 0)
            {
                saveCmd.CommandText = @"
                INSERT INTO Category (Title, Color)
                VALUES (@Title, @Color);
                SELECT last_insert_rowid();";
            }
            else
            {
                saveCmd.CommandText = @"
                UPDATE Category SET Title = @Title, Color = @Color
                WHERE Id = @Id";
                saveCmd.Parameters.AddWithValue("@Id", item.Id);
            }

            saveCmd.Parameters.AddWithValue("@Title", item.Title);
            saveCmd.Parameters.AddWithValue("@Color", item.Color);

            var result = await saveCmd.ExecuteScalarAsync();
            if (item.Id == 0)
            {
                item.Id = Convert.ToInt32(result);
            }

            return item.Id;
        }

        /// <summary>
        /// Deletes a category from the database.
        /// </summary>
        /// <param name="item">The category to delete.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> DeleteItemAsync(Category item)
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Category WHERE Id = @id";
            deleteCmd.Parameters.AddWithValue("@id", item.Id);

            return await deleteCmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Drops the Category table from the database.
        /// </summary>
        public async Task DropTableAsync()
        {
            await Init();
            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var dropTableCmd = connection.CreateCommand();
            dropTableCmd.CommandText = "DROP TABLE IF EXISTS Category";

            await dropTableCmd.ExecuteNonQueryAsync();
            _hasBeenInitialized = false;
        }
    }
}