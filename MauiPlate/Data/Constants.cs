namespace MauiPlate.Data
{
    public static class Constants
    {
        public const string DatabaseFilename = "AppSQLite.db3";

        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";
        /// <summary>
        /// sqlite-net-pcl Flags for opening the SQLite database connection.
        /// </summary>
        public const SQLite.SQLiteOpenFlags Flags =
    // open the database in read/write mode
    SQLite.SQLiteOpenFlags.ReadWrite |
    // create the database if it doesn't exist
    SQLite.SQLiteOpenFlags.Create |
    // enable multi-threaded database access
    SQLite.SQLiteOpenFlags.SharedCache;

        /// <summary>
        /// sqlite-net-pcl database path for the current platform.
        /// Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)
        /// Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DatabaseFilename)
        /// </summary>
        public static string DatabasePathV1 =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}