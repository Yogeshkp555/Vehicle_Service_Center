using Microsoft.EntityFrameworkCore;
using Vehicle_Service_Center.Data;
using Vehicle_Service_Center.Models;

namespace Vehicle_Service_Center.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly ApplicationDbContext _context;

        public BillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bill>> GetAllAsync()
        {
            return await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.ServiceRequest)
                    .ThenInclude(s => s!.Vehicle)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Bill?> GetByIdAsync(int id)
        {
            return await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.ServiceRequest)
                    .ThenInclude(s => s!.Vehicle)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Bill>> GetByCustomerIdAsync(
            string customerId)
        {
            return await _context.Bills
                .Include(b => b.ServiceRequest)
                    .ThenInclude(s => s!.Vehicle)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Bill bill)
        {
            await _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bill bill)
        {
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bill bill)
        {
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
        }
    }
}