using Vehicle_Service_Center.Models;

namespace Vehicle_Service_Center.Repositories
{
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllAsync();

        Task<ServiceRequest?> GetByIdAsync(int id);

        Task<List<ServiceRequest>> GetByCustomerIdAsync(string customerId);

        Task<List<ServiceRequest>> GetByMechanicIdAsync(string mechanicId);

        Task AddAsync(ServiceRequest serviceRequest);

        Task UpdateAsync(ServiceRequest serviceRequest);

        Task DeleteAsync(ServiceRequest serviceRequest);
    }
}