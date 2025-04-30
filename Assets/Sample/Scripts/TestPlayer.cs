using System;
using System.Linq;
using EF4Unity.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sample.Scripts
{
    public class TestPlayer: MonoBehaviour
    {
        private DbContextFactory<PlayerDBContext> _dbContextFactory;
        private PlayerDBContext _dbContext;

        private void Awake()
        {
            //_dbContextFactory = new PlayerDbContextFactory();
            _dbContextFactory = new PlayerDbContextEncryptedFactory();
            _dbContext = _dbContextFactory.CreateDbContext();
            _dbContext.Player.Add(new Player() { Name = "Player " + Random.Range(0, Int32.MaxValue) });
            _dbContext.SaveChanges();

            var res = _dbContext.Player.ToList();
            foreach (var player in res)
            {
                Debug.Log(player.Name);
            }
        }
    }
}