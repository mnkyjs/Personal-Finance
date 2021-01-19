using Personal.Finance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Personal.Finance.Domain.Dtos
{
    public class CategorieDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsShared { get; set; }
        public int UserId { get; set; }

        public static CategorieDto MapToDto(Category entity)
        {
            return new CategorieDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsShared = entity.IsShared,
                UserId = entity.UserId,
            };
        }

        public static Category MapToEntity(CategorieDto dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                IsShared = dto.IsShared,
                UserId = dto.UserId,
            };
        }
    }
}
