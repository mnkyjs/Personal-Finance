using System;
using System.Collections.Generic;
using System.Text;

namespace Personal.Finance.Domain.Utils
{
    [Flags]
    public enum TransactionTypeEnum
    {
        Income,
        Expense,
    }
}
