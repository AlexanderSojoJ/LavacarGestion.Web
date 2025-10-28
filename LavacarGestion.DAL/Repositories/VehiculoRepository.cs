using LavacarGestion.DAL.Data;
using LavacarGestion.Entities;
using Microsoft.EntityFrameworkCore;

namespace LavacarGestion.DAL.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ApplicationDbContext _context;

        public VehiculoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehiculo>> GetAllAsync()
        {
            return await _context.Vehiculos
                .Include(v => v.Cliente)
                .OrderBy(v => v.Placa)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehiculo>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Vehiculos
                .Where(v => v.ClienteId == clienteId)
                .OrderBy(v => v.Placa)
                .ToListAsync();
        }

        public async Task<Vehiculo?> GetByIdAsync(int id)
        {
            return await _context.Vehiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.VehiculoId == id);
        }

        public async Task<Vehiculo> AddAsync(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<Vehiculo> UpdateAsync(Vehiculo vehiculo)
        {
            // Buscar sin tracking
            var vehiculoExistente = await _context.Vehiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehiculoId == vehiculo.VehiculoId);

            if (vehiculoExistente != null)
            {
                // Detach cualquier entidad rastreada
                var tracked = _context.ChangeTracker.Entries<Vehiculo>()
                    .FirstOrDefault(e => e.Entity.VehiculoId == vehiculo.VehiculoId);

                if (tracked != null)
                {
                    tracked.State = EntityState.Detached;
                }

                // Ahora actualizar
                _context.Vehiculos.Update(vehiculo);
                await _context.SaveChangesAsync();
            }

            return vehiculo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                return false;

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistePlacaAsync(string placa, int? vehiculoId = null)
        {
            if (vehiculoId.HasValue)
            {
                return await _context.Vehiculos
                    .AnyAsync(v => v.Placa == placa && v.VehiculoId != vehiculoId.Value);
            }
            else
            {
                return await _context.Vehiculos
                    .AnyAsync(v => v.Placa == placa);
            }
        }
    }
}