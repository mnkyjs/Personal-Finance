using System;
using System.Collections.Generic;
using System.Text;

namespace Personal.Finance.Domain.Dtos
{
    public class AuthUserDto
    {
        public string Token { get; set; }
        public UserLoginDto User { get; set; }
    }
}
