using Microsoft.EntityFrameworkCore;
using EF4Unity.Scripts.Core;
using UnityEngine;

namespace Sample.Scripts
{
    public class PlayerDbContextFactory : SQLiteDbContextFactory<PlayerDBContext>
    {
        public PlayerDbContextFactory() : base(
            Application.persistentDataPath, 
            Application.dataPath, "data.db") { }

        protected override PlayerDBContext InternalCreateDbContext(DbContextOptions<PlayerDBContext> optionsBuilder)
        {
            return new PlayerDBContext(optionsBuilder);
        }
    }
}