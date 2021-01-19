using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Finance.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsShared { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
