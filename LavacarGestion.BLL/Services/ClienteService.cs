using LavacarGestion.DAL.Repositories;
using LavacarGestion.Entities;

namespace LavacarGestion.BLL.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<(bool exito, string mensaje, Cliente? cliente)> CrearClienteAsync(Cliente cliente)
        {
            // Validar edad (no menores de 18 años)
            var edad = CalcularEdad(cliente.FechaNacimiento);
            if (edad < 18)
            {
                return (false, "No puede registrar clientes menores de edad. Debe tener al menos 18 años.", null);
            }

            // Validar identificación única
            if (await _clienteRepository.ExisteIdentificacionAsync(cliente.Identificacion))
            {
                return (false, "Ya existe un cliente con esta identificación.", null);
            }

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(cliente.Identificacion))
            {
                return (false, "La identificación es requerida.", null);
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return (false, "El nombre es requerido.", null);
            }

            if (string.IsNullOrWhiteSpace(cliente.Apellidos))
            {
                return (false, "Los apellidos son requeridos.", null);
            }

            // Crear cliente
            var clienteCreado = await _clienteRepository.AddAsync(cliente);
            return (true, "Cliente creado exitosamente.", clienteCreado);
        }

        public async Task<(bool exito, string mensaje, Cliente? cliente)> ActualizarClienteAsync(Cliente cliente)
        {
            // Verificar que el cliente existe
            var clienteExistente = await _clienteRepository.GetByIdAsync(cliente.ClienteId);
            if (clienteExistente == null)
            {
                return (false, "El cliente no existe.", null);
            }

            // Validar edad (no menores de 18 años)
            var edad = CalcularEdad(cliente.FechaNacimiento);
            if (edad < 18)
            {
                return (false, "No puede registrar clientes menores de edad. Debe tener al menos 18 años.", null);
            }

            // Validar identificación única (excepto el mismo cliente)
            if (await _clienteRepository.ExisteIdentificacionAsync(cliente.Identificacion, cliente.ClienteId))
            {
                return (false, "Ya existe otro cliente con esta identificación.", null);
            }

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(cliente.Identificacion))
            {
                return (false, "La identificación es requerida.", null);
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return (false, "El nombre es requerido.", null);
            }

            if (string.IsNullOrWhiteSpace(cliente.Apellidos))
            {
                return (false, "Los apellidos son requeridos.", null);
            }

            // Actualizar cliente
            var clienteActualizado = await _clienteRepository.UpdateAsync(cliente);
            return (true, "Cliente actualizado exitosamente.", clienteActualizado);
        }

        public async Task<(bool exito, string mensaje)> EliminarClienteAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                return (false, "El cliente no existe.");
            }

            // Verificar si tiene vehículos asociados
            if (cliente.Vehiculos != null && cliente.Vehiculos.Any())
            {
                return (false, "No se puede eliminar el cliente porque tiene vehículos asociados.");
            }

            var resultado = await _clienteRepository.DeleteAsync(id);
            if (resultado)
            {
                return (true, "Cliente eliminado exitosamente.");
            }

            return (false, "No se pudo eliminar el cliente.");
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;

            // Ajustar si aún no ha cumplido años este año
            if (fechaNacimiento.Date > hoy.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }
    }
}