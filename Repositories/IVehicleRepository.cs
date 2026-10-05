using Vehicle_Service_Center.Models;

namespace Vehicle_Service_Center.Repositories
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();

        Task<Vehicle?> GetByIdAsync(int id);

        Task<List<Vehicle>> GetByCustomerIdAsync(string customerId);

        Task AddAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(Vehicle vehicle);
    }
}