namespace yourInvoice.Link.Infrastructure.Persistence.IRepositories
{
    public interface IUnitOfWorkLink
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
