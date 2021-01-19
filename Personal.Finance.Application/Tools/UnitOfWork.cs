using Microsoft.Extensions.Logging;
using Personal.Finance.Application.Interface;
using Personal.Finance.Application.Repository;
using Personal.Finance.Database;
using Personal.Finance.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Personal.Finance.Application.Tools
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        private RepositoryBase<Category> _categorie;
        private RepositoryBase<Transaction> _transactions;
        private RepositoryBase<UserBalance> _userBalance;

        public UnitOfWork(DataContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IRepositoryBase<Category> Categories =>
                    _categorie ??= new RepositoryBase<Category>(_context);

        public IRepositoryBase<Transaction> Transactions =>
                            _transactions ??= new RepositoryBase<Transaction>(_context);
        public IRepositoryBase<UserBalance> UserBalances =>
                    _userBalance ??= new RepositoryBase<UserBalance>(_context);

        public async Task<bool> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync().ConfigureAwait(true) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Unable to save changes to database, error: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
