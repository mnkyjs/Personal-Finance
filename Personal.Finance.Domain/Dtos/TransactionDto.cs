using Personal.Finance.Domain.Entities;
using Personal.Finance.Domain.Utils;
using System;

namespace Personal.Finance.Domain.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }


        public static TransactionDto MapToDto(Transaction entity)
        {
            return new TransactionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Value = entity.Value,
                Date = entity.Date,
                UserId = entity.UserId,
                CategoryId = entity.CategoryId,
                TransactionType = entity.TransactionType,
            };
        }

        public static Transaction MapToEntity(TransactionDto dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Value = dto.Value,
                Date = dto.Date,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                TransactionType = dto.TransactionType,
            };
        }
    }
}