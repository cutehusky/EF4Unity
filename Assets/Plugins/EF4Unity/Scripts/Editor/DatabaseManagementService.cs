using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using Utils;

namespace EF4Unity.Scripts.Editor
{
    public class DatabaseManagementService
    {
        public string MigrationPath => $"{Application.dataPath}/Migrations/";
        public void UpdateDatabase(DbContext dbContext)
        {
            dbContext.Database.Migrate();
        }
        
        private class CurrentDbContext: ICurrentDbContext
        {
            public DbContext Context { get; set; }
        }

        private IMigrationsScaffolder InitDesigner(DbContext dbContext)
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkSqlite();
            var currentDbContext = new CurrentDbContext {Context = dbContext};
            services.AddSingleton<IModel>(dbContext.Model);
            services.AddSingleton<IDbContextOptions>(dbContext.GetService<IDbContextOptions>());
            services.AddSingleton<ICurrentDbContext>(currentDbContext);
            services.AddEntityFrameworkDesignTimeServices();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMigrationsScaffolder>();
        }
        
        public void AddMigration(DbContext dbContext, string name)
        {
            var scaffolder = InitDesigner(dbContext);
            var dbContextType = dbContext.GetType();
            var dbContextName = dbContextType.Name;
            var finalMigrationName = name;

            var scaffoldedMigration = scaffolder.ScaffoldMigration(
                migrationName: finalMigrationName,
                rootNamespace: dbContextType.Namespace
            );

            var outputPath = $"{MigrationPath}{dbContextName}/"; 
            DirectoryExtensions.EnsureDatabaseFolderExists(outputPath);
            File.WriteAllText($"{outputPath}{scaffoldedMigration.MigrationId}.cs", scaffoldedMigration.MigrationCode);
            File.WriteAllText($"{outputPath}{scaffoldedMigration.MigrationId}.Designer.cs", scaffoldedMigration.MetadataCode);
            File.WriteAllText($"{outputPath}{scaffoldedMigration.SnapshotName}.cs", scaffoldedMigration.SnapshotCode);
        }
        
        public void RemoveMigration(DbContext dbContext)
        {
            var scaffolder = InitDesigner(dbContext);
            var dbContextType = dbContext.GetType();
            var dbContextName = dbContextType.Name;
            var outputPath = $"{MigrationPath}{dbContextName}/"; 
            scaffolder.RemoveMigration(
                projectDir: outputPath,
                rootNamespace: dbContextType.Namespace!,
                force: true,
                language: null
            );
        }
        
        public void CreateDatabase(DbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
        }
    }
}