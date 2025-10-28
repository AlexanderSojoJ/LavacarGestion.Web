using LavacarGestion.BLL.Services;
using LavacarGestion.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LavacarGestion.Web.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoService _vehiculoService;
        private readonly ClienteService _clienteService;

        public VehiculoController(VehiculoService vehiculoService, ClienteService clienteService)
        {
            _vehiculoService = vehiculoService;
            _clienteService = clienteService;
        }

        // GET: Vehiculo
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Vehiculo/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehiculos = await _vehiculoService.ObtenerTodosAsync();
            return Json(new { data = vehiculos });
        }

        // GET: Vehiculo/GetClientes
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Json(clientes);
        }

        // GET: Vehiculo/GetById/5
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var vehiculo = await _vehiculoService.ObtenerPorIdAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            return Json(vehiculo);
        }

        // POST: Vehiculo/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vehiculo vehiculo)
        {
            var resultado = await _vehiculoService.CrearVehiculoAsync(vehiculo);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Vehiculo/Edit
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Vehiculo vehiculo)
        {
            var resultado = await _vehiculoService.ActualizarVehiculoAsync(vehiculo);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Vehiculo/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _vehiculoService.EliminarVehiculoAsync(id);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }
    }
}