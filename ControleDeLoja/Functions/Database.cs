using System;
using System.Data.SQLite;

namespace ControleDeLoja.Functions
{
    public static class Database
    {
        private const string databasePath = @"C:\Users\Dougl\source\repos\ControleDeLoja\ControleDeLoja\Database\Database.db";

        public static SQLiteConnection ConnectionDB()
        {
            return new SQLiteConnection("Data Source=" + databasePath);
        }
    }
}