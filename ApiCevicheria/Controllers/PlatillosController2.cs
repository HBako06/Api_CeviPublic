using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCevicheria.Data;
using ApiCevicheria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ApiCevicheria.Controllers
{
    [ApiController]
    [Route("platillos")]
    public class PlatillosController2 : ControllerBase
    {
        private readonly DataContext _context;

        public PlatillosController2(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult ListarPlatillos()
        {
            try
            {
                var platillos = _context.Platillos.ToList();
                return Ok(platillos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("ListaCompleta")]
        public IActionResult ListarPlatillosConCategorias()
        {
            try
            {
                var platillos = _context.Platillos
                    .Include(p => p.CategoriaObjeto) // Carga la entidad Categoria asociada
                    .Select(p => new
                    {
                        ID = p.ID,
                        NombrePlatillo = p.NombrePlatillo,
                        CategoriaID = p.CategoriaObjeto.NombreCategoria, // Accede al nombre de la categoría
                        EsMenuDelDia = p.EsMenuDelDia,
                        Precio = p.Precio
                    })
                    .ToList();

                return Ok(platillos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpPost]
        [Route("Agregar")]
        public IActionResult AgregarPlatillo([FromBody] PlatilloAgregar platillo)
        {
            try
            {
                if (ModelState.IsValid) // Verificar si el modelo es válido
                {
                    // Crear una nueva entidad Platillo y asignar valores desde platillo
                    var nuevoPlatillo = new Platillo
                    {
                        NombrePlatillo = platillo.NombrePlatillo,
                        CategoriaID = platillo.CategoriaID,
                        EsMenuDelDia = false,
                        Precio = 0
                    };

                    // Agregar la nueva entidad al contexto y guardar cambios
                    _context.Platillos.Add(nuevoPlatillo);
                    _context.SaveChanges();

                    return Ok("Platillo agregado correctamente.");
                }
                else
                {
                    // Si el modelo no es válido, devolver errores de validación
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(string.Join(" ", errors));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult EliminarPlatillo(int id)
        {
            try
            {
                var platillo = _context.Platillos.Find(id);
                if (platillo == null)
                {
                    return NotFound($"No se encontró ningún platillo con ID {id}.");
                }

                _context.Platillos.Remove(platillo);
                _context.SaveChanges();

                return Ok($"Platillo con ID {id} eliminado correctamente.");
            }
            catch (DbUpdateException ex)
            {
                // Verificar si la excepción es debido a una restricción de clave foránea
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                {
                    // Clave foránea violada, el registro está referenciado por otra tabla
                    return BadRequest($"No puedes eliminar este platillo porque está referenciado por otra tabla.");
                }
                else
                {
                    // Otra excepción de base de datos
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Otras excepciones
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarPlatillo(int id, [FromBody] PlatilloActualizar platillo)
        {
            try
            {
                var platilloExistente = _context.Platillos.Find(id);

                if (platilloExistente == null)
                {
                    return NotFound($"No se encontró ningún platillo con ID {id}.");
                }

                // Actualiza las propiedades del platillo existente con los nuevos valores
                platilloExistente.NombrePlatillo = platillo.NombrePlatillo;
                platilloExistente.CategoriaID = platillo.CategoriaID;
                platilloExistente.EsMenuDelDia = platillo.EsMenuDelDia;
                platilloExistente.Precio = platillo.Precio;

                _context.Entry(platilloExistente).State = EntityState.Modified;
                _context.SaveChanges();


                return Ok($"Platillo con ID {id} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }




        [HttpGet]
        [Route("BuscarID/{id}")]
        public IActionResult BuscarPlatilloID(int id)
        {
            var platillo = _context.Platillos
                .Where(p => p.ID == id)
                .Select(p => new
                {
                    ID = p.ID,
                    NombrePlatillo = p.NombrePlatillo,
                    CategoriaNombre = p.CategoriaObjeto.NombreCategoria, // Si necesitas el nombre de la categoría
                    CategoriaID = p.CategoriaID,
                    EsMenuDelDia = p.EsMenuDelDia,
                    Precio = p.Precio
                })
                .FirstOrDefault();
                

            if (platillo == null)
            {
                return NotFound($"No se encontró ningún platillo con ID {id}.");
            }

            return Ok(platillo);
        }



        [HttpGet]
        [Route("BuscarNombre/{nombre}")]
        public IActionResult BuscarPlatilloNombre(string nombre)
        {
            var platillo = _context.Platillos
                .Where(p => p.NombrePlatillo == nombre)
                .Select(p => new
                {
                    ID = p.ID,
                    NombrePlatillo = p.NombrePlatillo,
                    CategoriaNombre = p.CategoriaObjeto.NombreCategoria, // Si necesitas el nombre de la categoría
                    CategoriaID = p.CategoriaID,
                    EsMenuDelDia = p.EsMenuDelDia,
                    Precio = p.Precio
                })
                .FirstOrDefault();


            if (platillo == null)
            {
                return NotFound($"No se encontró ningún platillo con nombre {nombre}.");
            }

            return Ok(platillo);
        }



    }
}
