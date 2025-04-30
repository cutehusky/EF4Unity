using Microsoft.EntityFrameworkCore;

namespace EF4Unity.Scripts.Core
{
    public abstract class SQLiteDbContextFactory<T>: DbContextFactory<T> where T: DbContext
    {
        public SQLiteDbContextFactory(string dbRuntimePath, 
            string dbDesignPath, string dbName) : base(dbRuntimePath, dbDesignPath, dbName)
        {
            Startup.SetupSQLite();
        }

        public override T CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlite(connectionString);
            return InternalCreateDbContext(optionsBuilder.Options);
        }
    }
}