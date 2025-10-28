using LavacarGestion.Entities;

namespace LavacarGestion.DAL.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente> AddAsync(Cliente cliente);
        Task<Cliente> UpdateAsync(Cliente cliente);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExisteIdentificacionAsync(string identificacion, int? clienteId = null);
        Task<Cliente?> GetByIdentificacionAsync(string identificacion);
    }
}