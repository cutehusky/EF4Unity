using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EF4Unity.Scripts.Core
{
    public abstract class SQLCipherDbContextFactory<T>: DbContextFactory<T> where T: DbContext
    {
        private readonly string _password;
        
        public SQLCipherDbContextFactory(
            string dbRuntimePath,
            string dbDesignPath,
            string dbName, string password): base(dbRuntimePath, dbDesignPath, dbName)
        {
            _password = password;
            Startup.SetupSQLCipher();
        }

        public override T CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"PRAGMA key = '{_password}';";
                cmd.ExecuteNonQuery();
            }
            optionsBuilder.UseSqlite(connection);
            return InternalCreateDbContext(optionsBuilder.Options);
        }
    }
}