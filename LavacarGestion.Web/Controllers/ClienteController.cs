using LavacarGestion.BLL.Services;
using LavacarGestion.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LavacarGestion.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: Cliente/GetAll (para DataTables)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return Json(new { data = clientes });
        }

        // GET: Cliente/GetById/5
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Json(cliente);
        }

        // POST: Cliente/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            var resultado = await _clienteService.CrearClienteAsync(cliente);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Cliente/Edit
[HttpPost]
public async Task<IActionResult> Edit([FromBody] Cliente cliente)
{
            var resultado = await _clienteService.ActualizarClienteAsync(cliente);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _clienteService.EliminarClienteAsync(id);

            if (resultado.exito)
            {
                return Json(new { success = true, message = resultado.mensaje });
            }

            return Json(new { success = false, message = resultado.mensaje });
        }
    }
}