using Microsoft.EntityFrameworkCore;
using Vehicle_Service_Center.Data;
using Vehicle_Service_Center.Models;

namespace Vehicle_Service_Center.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _context.ServiceRequests
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Include(s => s.Mechanic)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _context.ServiceRequests
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Include(s => s.Mechanic)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<ServiceRequest>> GetByCustomerIdAsync(
            string customerId)
        {
            return await _context.ServiceRequests
                .Include(s => s.Vehicle)
                .Include(s => s.Mechanic)
                .Where(s => s.CustomerId == customerId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ServiceRequest>> GetByMechanicIdAsync(
            string mechanicId)
        {
            return await _context.ServiceRequests
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Where(s => s.MechanicId == mechanicId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(ServiceRequest serviceRequest)
        {
            await _context.ServiceRequests.AddAsync(serviceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Update(serviceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();
        }
    }
}