using Personal.Finance.Domain.Utils;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Finance.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }
}
