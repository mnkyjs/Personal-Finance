using Personal.Finance.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Personal.Finance.Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<Transaction> Transactions { get; }
        IRepositoryBase<Category> Categories { get; }
        IRepositoryBase<UserBalance> UserBalances { get; }
        Task<bool> CompleteAsync();
    }
}
