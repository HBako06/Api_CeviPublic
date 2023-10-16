using Microsoft.AspNetCore.Mvc;
using ApiCevicheria.Models;



namespace ApiCevicheria.Controllers
{
    [ApiController]
    [Route("ordenes")]
    public class OrdenesController : ControllerBase
    {
        private readonly DataContext _context;

        public OrdenesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("select/{id:int}")]
        public async Task<ActionResult> GetSelectLoading(int id)
        {
            var ordenes = await _context.Ordenes
                .Include(p => p.PlatilloObjeto)
                .Include(p => p.EstadoObjeto)
                .Where(p => p.ID == id)
                .Select(p => new
                {
                    Id = p.ID,
                    RegistroId = p.RegistroID,
                    PlatilloID = p.PlatilloObjeto.NombrePlatillo,
                    Cantidad = p.Cantidad,
                    HoraRegistrada = p.HoraRegistrada,
                    EstadoID = p.EstadoObjeto.NombreEstado,
                    NumeroMesa = p.NumeroMesa
                })
                .ToListAsync();

            if (ordenes == null || !ordenes.Any())
            {
                var errorResponse = new // estoy haciendo que devuelva un JSON en vez de un string
                {
                    Message = $"No se encontró ninguna orden con ID {id}."
                };
                return NotFound(errorResponse);
            }

            return Ok(ordenes);
        }

        [HttpGet]
        [Route("ListaCompleta")]
        public async Task<ActionResult> ListaCompleta()
        {
            var ordenes = await _context.Ordenes
                .Include(p => p.PlatilloObjeto)
                .Include(p => p.EstadoObjeto)
                .Select(p => new
                {
                    Id = p.ID,
                    RegistroId = p.RegistroID,
                    PlatilloID = p.PlatilloObjeto.NombrePlatillo,
                    Cantidad = p.Cantidad,
                    HoraRegistrada = p.HoraRegistrada,
                    EstadoID = p.EstadoObjeto.NombreEstado,
                    NumeroMesa = p.NumeroMesa
                })
                .ToListAsync();

            if (ordenes == null || !ordenes.Any())
            {
                var errorResponse = new // estoy haciendo que devuelva un JSON en vez de un string
                {
                    Message = $"No se encontró ninguna orden"
                };
                return NotFound(errorResponse);
            }

            return Ok(ordenes);
        }

        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> UpdateOrder([FromBody] Orden inputModel)
        {
            var id = inputModel.ID;
            var registroId = inputModel.RegistroID;

            var orden = await _context.Ordenes
                .Where(p => p.ID == id && p.RegistroID == registroId)
                .FirstOrDefaultAsync();

            if (orden == null)
            {
                var errorResponse = new
                {
                    Message = $"No se encontró ninguna orden con ID {id} y RegistroID {registroId}."
                };
                return NotFound(errorResponse);
            }

            // Utiliza el operador de coalescencia nula para asignar valores no nulos de inputModel a orden
            orden.HoraRegistrada = inputModel.HoraRegistrada ?? orden.HoraRegistrada;
            // orden.NumeroMesa = inputModel.NumeroMesa ?? orden.NumeroMesa; // creo que no es necesario cambiar la mesa
            // Actualiza las demás propiedades de la orden
            orden.EstadoID = inputModel.EstadoID;

            // Guarda los cambios en la base de datos
            _context.Entry(orden).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Orden con ID {id} y RegistroID {registroId} actualizada correctamente.");
        }

        [HttpPost]
        [Route("agregar")]
        public async Task<IActionResult> AgregarOrden([FromBody] Orden inputModel)
        {
            try
            {
                // Verifica si la orden ya existe en la base de datos
                var ordenExistente = await _context.Ordenes
                    .Where(p => p.ID == inputModel.ID && p.RegistroID == inputModel.RegistroID)
                    .FirstOrDefaultAsync();

                if (ordenExistente != null)
                {
                    var errorResponse = new
                    {
                        Message = $"Ya existe una orden con ID {inputModel.ID} y RegistroID {inputModel.RegistroID}."
                    };
                    return Conflict(errorResponse);
                }

                // Crea una nueva instancia de Orden y asigna los valores del inputModel
                var nuevaOrden = new Orden
                {
                    ID = inputModel.ID,
                    RegistroID = inputModel.RegistroID,
                    PlatilloID = inputModel.PlatilloID,
                    Cantidad = inputModel.Cantidad,
                    HoraRegistrada = inputModel.HoraRegistrada,
                    EstadoID = inputModel.EstadoID,
                    NumeroMesa = inputModel.NumeroMesa
                };

                // Agrega la nueva orden al contexto y guarda los cambios en la base de datos
                _context.Ordenes.Add(nuevaOrden);
                await _context.SaveChangesAsync();

                var mssge = new
                {
                    Message = $"Agregado Correctamente!"
                };
                return Ok(mssge);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("eliminar/{id:int}/{registroId:int}")]
        public async Task<IActionResult> EliminarOrden(int id, int registroId)
        {
            var orden = await _context.Ordenes
                .Where(p => p.ID == id && p.RegistroID == registroId)
                .FirstOrDefaultAsync();

            if (orden == null)
            {
                var errorResponse = new
                {
                    Message = $"No se encontró ninguna orden con ID {id} y RegistroID {registroId}."
                };
                return NotFound(errorResponse);
            }

            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();

            var successResponse = new
            {
                Message = $"Orden con ID {id} y RegistroID {registroId} eliminada correctamente."
            };
            return Ok(successResponse);
        }

        [HttpDelete]
        [Route("eliminar/{id:int}")]
        public async Task<IActionResult> EliminarOrdenesPorId(int id)
        {
            var ordenes = await _context.Ordenes
                .Where(p => p.ID == id)
                .ToListAsync();

            if (ordenes == null || !ordenes.Any())
            {
                var errorResponse = new
                {
                    Message = $"No se encontraron órdenes con ID {id}."
                };
                return NotFound(errorResponse);
            }

            _context.Ordenes.RemoveRange(ordenes);
            await _context.SaveChangesAsync();

            var successResponse = new
            {
                Message = $"Todas las órdenes con ID {id} fueron eliminadas correctamente."
            };
            return Ok(successResponse);
        }

    }
}
