using System.Collections.Generic;
using System.Threading.Tasks;
using RpgApp.Shared.Types;

namespace RpgApp.Shared.Services
{
    public class InventoryService
    {
        private RpgDbContext _context;

        public InventoryService(RpgDbContext context)
        {
            _context = context;
        }
        public async Task<List<Equipment>> GetUnder30GpInventory()
        {
            var equipment = await _context.Equipment.Include(x => x.Effects).Where(x => x.GoldCost <= 30).ToListAsync();
            return equipment;
        }
        public async Task AddNewEquipment(Equipment equipment)
        {
            await _context.Equipment.AddAsync(equipment);
            await _context.SaveChangesAsync();
        }
    }
}
