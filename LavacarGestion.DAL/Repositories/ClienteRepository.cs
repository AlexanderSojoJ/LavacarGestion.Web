using LavacarGestion.DAL.Data;
using LavacarGestion.Entities;
using Microsoft.EntityFrameworkCore;

namespace LavacarGestion.DAL.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Vehiculos)
                .FirstOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            cliente.FechaRegistro = DateTime.Now;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }
        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            // Usar AsNoTracking para evitar conflictos
            var clienteExistente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClienteId == cliente.ClienteId);

            if (clienteExistente != null)
            {
                // Preservar FechaRegistro del cliente existente
                cliente.FechaRegistro = clienteExistente.FechaRegistro;

                // Actualizar la entidad
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }

            return cliente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteIdentificacionAsync(string identificacion, int? clienteId = null)
        {
            if (clienteId.HasValue)
            {
                // Para edición: verificar si existe la identificación en otro cliente
                return await _context.Clientes
                    .AnyAsync(c => c.Identificacion == identificacion && c.ClienteId != clienteId.Value);
            }
            else
            {
                // Para creación: verificar si existe la identificación
                return await _context.Clientes
                    .AnyAsync(c => c.Identificacion == identificacion);
            }
        }

        public async Task<Cliente?> GetByIdentificacionAsync(string identificacion)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Identificacion == identificacion);
        }
    }
}