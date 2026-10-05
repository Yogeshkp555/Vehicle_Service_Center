using Vehicle_Service_Center.Models;

namespace Vehicle_Service_Center.Repositories
{
    public interface IBillRepository
    {
        Task<List<Bill>> GetAllAsync();

        Task<Bill?> GetByIdAsync(int id);

        Task<List<Bill>> GetByCustomerIdAsync(string customerId);

        Task AddAsync(Bill bill);

        Task UpdateAsync(Bill bill);

        Task DeleteAsync(Bill bill);
    }
}