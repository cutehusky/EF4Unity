using Microsoft.EntityFrameworkCore;
using Utils;

namespace EF4Unity.Scripts.Core
{
    public abstract class DbContextFactory<T>: IDbContextFactory<T> where T : DbContext
    {
        private readonly string _dbRuntimePath;
        private readonly string _dbName;
        private readonly string _dbDesignPath;

        public abstract T CreateDbContext(string connectionString);

        public T CreateDbContext()
        {
            var dbContext = CreateDbContext(RuntimeDataSource);
            dbContext.Database.Migrate();
            return dbContext;
        }

        public T CreateDesignDbContext()
        {
            return CreateDbContext(DesignDataSource);
        }

        private string RuntimeDataSource => $"Data Source={System.IO.Path.Combine(_dbRuntimePath, _dbName)}";
        private string DesignDataSource => $"Data Source={System.IO.Path.Combine(_dbDesignPath, _dbName)}";

        public DbContextFactory(string dbRuntimePath, string dbDesignPath, string name)
        {
            _dbRuntimePath = dbRuntimePath;
            _dbDesignPath = dbDesignPath;
            _dbName = name;
            DirectoryExtensions.EnsureDatabaseFolderExists(_dbRuntimePath);
            // Must create directory for design database manually
            // DirectoryExtensions.EnsureDatabaseFolderExists(_dbDesignPath);
        }

        protected abstract T InternalCreateDbContext(DbContextOptions<T> optionsBuilder);
    }
}