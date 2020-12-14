using System;

namespace Personal.Finance.Domain.Utils
{
    [Flags]
    public enum RoleEnum
    {
        Owner = 4,
        Administrator = 8,
    }
}
