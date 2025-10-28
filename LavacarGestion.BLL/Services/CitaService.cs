using LavacarGestion.DAL.Repositories;
using LavacarGestion.Entities;

namespace LavacarGestion.BLL.Services
{
    public class CitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IVehiculoRepository _vehiculoRepository;

        public CitaService(ICitaRepository citaRepository, IClienteRepository clienteRepository, IVehiculoRepository vehiculoRepository)
        {
            _citaRepository = citaRepository;
            _clienteRepository = clienteRepository;
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<IEnumerable<Cita>> ObtenerTodasAsync()
        {
            return await _citaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _citaRepository.GetByClienteIdAsync(clienteId);
        }

        public async Task<IEnumerable<Cita>> ObtenerPorEstadoAsync(EstadoCita estado)
        {
            return await _citaRepository.GetByEstadoAsync(estado);
        }

        public async Task<Cita?> ObtenerPorIdAsync(int id)
        {
            return await _citaRepository.GetByIdAsync(id);
        }

        public async Task<(bool exito, string mensaje, Cita? cita)> CrearCitaAsync(Cita cita)
        {
            // Validar que el cliente existe
            var cliente = await _clienteRepository.GetByIdAsync(cita.ClienteId);
            if (cliente == null)
            {
                return (false, "El cliente seleccionado no existe.", null);
            }

            // Validar que el vehículo existe
            var vehiculo = await _vehiculoRepository.GetByIdAsync(cita.VehiculoId);
            if (vehiculo == null)
            {
                return (false, "El vehículo seleccionado no existe.", null);
            }

            // Validar que el vehículo pertenece al cliente
            if (vehiculo.ClienteId != cita.ClienteId)
            {
                return (false, "El vehículo seleccionado no pertenece al cliente.", null);
            }

            // Validar fecha de cita
            if (cita.FechaCita < DateTime.Now.Date)
            {
                return (false, "La fecha de la cita no puede ser anterior a hoy.", null);
            }

            // Crear cita
            var citaCreada = await _citaRepository.AddAsync(cita);
            return (true, "Cita creada exitosamente.", citaCreada);
        }

        public async Task<(bool exito, string mensaje, Cita? cita)> ActualizarCitaAsync(Cita cita)
        {
            // Verificar que la cita existe
            var citaExistente = await _citaRepository.GetByIdAsync(cita.CitaId);
            if (citaExistente == null)
            {
                return (false, "La cita no existe.", null);
            }

            // Validar que el cliente existe
            var cliente = await _clienteRepository.GetByIdAsync(cita.ClienteId);
            if (cliente == null)
            {
                return (false, "El cliente seleccionado no existe.", null);
            }

            // Validar que el vehículo existe
            var vehiculo = await _vehiculoRepository.GetByIdAsync(cita.VehiculoId);
            if (vehiculo == null)
            {
                return (false, "El vehículo seleccionado no existe.", null);
            }

            // Validar que el vehículo pertenece al cliente
            if (vehiculo.ClienteId != cita.ClienteId)
            {
                return (false, "El vehículo seleccionado no pertenece al cliente.", null);
            }

            // Validar fecha de cita
            if (cita.FechaCita < DateTime.Now.Date)
            {
                return (false, "La fecha de la cita no puede ser anterior a hoy.", null);
            }

            // Actualizar cita
            var citaActualizada = await _citaRepository.UpdateAsync(cita);
            return (true, "Cita actualizada exitosamente.", citaActualizada);
        }

        public async Task<(bool exito, string mensaje)> CambiarEstadoAsync(int citaId, EstadoCita nuevoEstado)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null)
            {
                return (false, "La cita no existe.");
            }

            var resultado = await _citaRepository.CambiarEstadoAsync(citaId, nuevoEstado);
            if (resultado)
            {
                string nombreEstado = nuevoEstado switch
                {
                    EstadoCita.Ingresada => "Ingresada",
                    EstadoCita.Cancelada => "Cancelada",
                    EstadoCita.Concluida => "Concluida",
                    _ => "Desconocido"
                };
                return (true, $"Estado de la cita cambiado a '{nombreEstado}' exitosamente.");
            }

            return (false, "No se pudo cambiar el estado de la cita.");
        }

        public async Task<(bool exito, string mensaje)> EliminarCitaAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null)
            {
                return (false, "La cita no existe.");
            }

            var resultado = await _citaRepository.DeleteAsync(id);
            if (resultado)
            {
                return (true, "Cita eliminada exitosamente.");
            }

            return (false, "No se pudo eliminar la cita.");
        }
    }
}