using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Finance.Domain.Entities
{
    public class UserBalance
    {
        public int Id { get; set; }
        public double Balance { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
