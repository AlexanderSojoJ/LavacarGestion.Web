using LavacarGestion.Entities;

namespace LavacarGestion.DAL.Repositories
{
    public interface IVehiculoRepository
    {
        Task<IEnumerable<Vehiculo>> GetAllAsync();
        Task<IEnumerable<Vehiculo>> GetByClienteIdAsync(int clienteId);
        Task<Vehiculo?> GetByIdAsync(int id);
        Task<Vehiculo> AddAsync(Vehiculo vehiculo);
        Task<Vehiculo> UpdateAsync(Vehiculo vehiculo);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistePlacaAsync(string placa, int? vehiculoId = null);
    }
}