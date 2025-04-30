using System.ComponentModel.DataAnnotations;

namespace Sample.Scripts
{
    public class Player
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
    }
}