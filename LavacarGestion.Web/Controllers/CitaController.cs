using LavacarGestion.BLL.Services;
using LavacarGestion.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LavacarGestion.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly CitaService _citaService;
        private readonly ClienteService _clienteService;
        private readonly VehiculoService _vehiculoService;

        public CitaController(CitaService citaService, ClienteService clienteService, VehiculoService vehiculoService)
        {
            _citaService = citaService;
            _clienteService = clienteService;
            _vehiculoService = vehiculoService;
        }

        // GET: Cita
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Cita/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var citas = await _citaService.ObtenerTodasAsync();
            return Json(new { data = citas });
        }

        // GET: Cita/GetClientes
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Json(clientes);
        }

        // GET: Cita/GetVehiculosByCliente/5
        [HttpGet]
        public async Task<IActionResult> GetVehiculosByCliente(int clienteId)
        {
            var vehiculos = await _vehiculoService.ObtenerPorClienteAsync(clienteId);
            return Json(vehiculos);
        }

        // GET: Cita/GetById/5
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _citaService.ObtenerPorIdAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            return Json(cita);
        }

        // POST: Cita/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cita cita)
        {
            var resultado = await _citaService.CrearCitaAsync(cita);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Cita/Edit
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Cita cita)
        {
            var resultado = await _citaService.ActualizarCitaAsync(cita);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Cita/CambiarEstado
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int citaId, int estado)
        {
            var resultado = await _citaService.CambiarEstadoAsync(citaId, (EstadoCita)estado);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Cita/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _citaService.EliminarCitaAsync(id);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }
    }
}