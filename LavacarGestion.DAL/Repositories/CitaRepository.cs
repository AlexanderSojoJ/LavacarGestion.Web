using LavacarGestion.DAL.Data;
using LavacarGestion.Entities;
using Microsoft.EntityFrameworkCore;

namespace LavacarGestion.DAL.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly ApplicationDbContext _context;

        public CitaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .OrderByDescending(c => c.FechaCita)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .Where(c => c.ClienteId == clienteId)
                .OrderByDescending(c => c.FechaCita)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .Where(c => c.Estado == estado)
                .OrderByDescending(c => c.FechaCita)
                .ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .FirstOrDefaultAsync(c => c.CitaId == id);
        }

        public async Task<Cita> AddAsync(Cita cita)
        {
            cita.FechaRegistro = DateTime.Now;
            cita.Estado = EstadoCita.Ingresada;
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task<Cita> UpdateAsync(Cita cita)
        {
            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int citaId, EstadoCita nuevoEstado)
        {
            var cita = await _context.Citas.FindAsync(citaId);
            if (cita == null)
                return false;

            cita.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}