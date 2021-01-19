using Personal.Finance.Domain.Entities;

namespace Personal.Finance.Domain.Dtos
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


        public static UserRegisterDto MapToDto(User entity)
        {
            return new UserRegisterDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName,
                Email = entity.Email,
            };
        }

        public static User MapToEntity(UserRegisterDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
            };
        }
    }
}
