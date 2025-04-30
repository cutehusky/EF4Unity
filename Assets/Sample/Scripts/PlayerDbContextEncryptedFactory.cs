using Microsoft.EntityFrameworkCore;
using EF4Unity.Scripts.Core;
using UnityEngine;

namespace Sample.Scripts
{
    public class PlayerDbContextEncryptedFactory : SQLCipherDbContextFactory<PlayerDBContext>
    {
        public PlayerDbContextEncryptedFactory() : base(
            Application.persistentDataPath, 
            Application.dataPath, "data_encrypted.db", "your-password") { }

        protected override PlayerDBContext InternalCreateDbContext(DbContextOptions<PlayerDBContext> optionsBuilder)
        {
            return new PlayerDBContext(optionsBuilder);
        }
    }
}