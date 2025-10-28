using LavacarGestion.DAL.Repositories;
using LavacarGestion.Entities;

namespace LavacarGestion.BLL.Services
{
    public class VehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IClienteRepository _clienteRepository;

        public VehiculoService(IVehiculoRepository vehiculoRepository, IClienteRepository clienteRepository)
        {
            _vehiculoRepository = vehiculoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Vehiculo>> ObtenerTodosAsync()
        {
            return await _vehiculoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Vehiculo>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _vehiculoRepository.GetByClienteIdAsync(clienteId);
        }

        public async Task<Vehiculo?> ObtenerPorIdAsync(int id)
        {
            return await _vehiculoRepository.GetByIdAsync(id);
        }

        public async Task<(bool exito, string mensaje, Vehiculo? vehiculo)> CrearVehiculoAsync(Vehiculo vehiculo)
        {
            // Validar que el cliente existe
            var cliente = await _clienteRepository.GetByIdAsync(vehiculo.ClienteId);
            if (cliente == null)
            {
                return (false, "El cliente seleccionado no existe.", null);
            }

            // Validar placa única
            if (await _vehiculoRepository.ExistePlacaAsync(vehiculo.Placa))
            {
                return (false, "Ya existe un vehículo con esta placa.", null);
            }

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(vehiculo.Placa))
            {
                return (false, "La placa es requerida.", null);
            }

            if (string.IsNullOrWhiteSpace(vehiculo.Marca))
            {
                return (false, "La marca es requerida.", null);
            }

            if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
            {
                return (false, "El modelo es requerido.", null);
            }

            if (vehiculo.Anio < 1900 || vehiculo.Anio > DateTime.Now.Year + 1)
            {
                return (false, $"El año debe estar entre 1900 y {DateTime.Now.Year + 1}.", null);
            }

            // Crear vehículo
            var vehiculoCreado = await _vehiculoRepository.AddAsync(vehiculo);
            return (true, "Vehículo creado exitosamente.", vehiculoCreado);
        }

        public async Task<(bool exito, string mensaje, Vehiculo? vehiculo)> ActualizarVehiculoAsync(Vehiculo vehiculo)
        {

            // Validar que el cliente existe
            var cliente = await _clienteRepository.GetByIdAsync(vehiculo.ClienteId);
            if (cliente == null)
            {
                return (false, "El cliente seleccionado no existe.", null);
            }

            // Validar placa única (excepto el mismo vehículo)
            if (await _vehiculoRepository.ExistePlacaAsync(vehiculo.Placa, vehiculo.VehiculoId))
            {
                return (false, "Ya existe otro vehículo con esta placa.", null);
            }

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(vehiculo.Placa))
            {
                return (false, "La placa es requerida.", null);
            }

            if (string.IsNullOrWhiteSpace(vehiculo.Marca))
            {
                return (false, "La marca es requerida.", null);
            }

            if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
            {
                return (false, "El modelo es requerido.", null);
            }

            if (vehiculo.Anio < 1900 || vehiculo.Anio > DateTime.Now.Year + 1)
            {
                return (false, $"El año debe estar entre 1900 y {DateTime.Now.Year + 1}.", null);
            }

            // Actualizar vehículo
            var vehiculoActualizado = await _vehiculoRepository.UpdateAsync(vehiculo);
            return (true, "Vehículo actualizado exitosamente.", vehiculoActualizado);
        }

        public async Task<(bool exito, string mensaje)> EliminarVehiculoAsync(int id)
        {
            var vehiculo = await _vehiculoRepository.GetByIdAsync(id);
            if (vehiculo == null)
            {
                return (false, "El vehículo no existe.");
            }

            // Verificar si tiene citas asociadas
            if (vehiculo.Citas != null && vehiculo.Citas.Any())
            {
                return (false, "No se puede eliminar el vehículo porque tiene citas asociadas.");
            }

            var resultado = await _vehiculoRepository.DeleteAsync(id);
            if (resultado)
            {
                return (true, "Vehículo eliminado exitosamente.");
            }

            return (false, "No se pudo eliminar el vehículo.");
        }
    }
}