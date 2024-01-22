using System.ComponentModel.DataAnnotations;

namespace Timelogger.Entities
{
    public class Freelancer
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
    }
}