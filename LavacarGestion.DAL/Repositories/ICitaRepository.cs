using LavacarGestion.Entities;

namespace LavacarGestion.DAL.Repositories
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<IEnumerable<Cita>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado);
        Task<Cita?> GetByIdAsync(int id);
        Task<Cita> AddAsync(Cita cita);
        Task<Cita> UpdateAsync(Cita cita);
        Task<bool> DeleteAsync(int id);
        Task<bool> CambiarEstadoAsync(int citaId, EstadoCita nuevoEstado);
    }
}