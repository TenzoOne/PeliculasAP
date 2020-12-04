using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDBContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this._context = context;
            this._mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)  // parametro para paginacion/
        {

            var queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable,paginacionDTO.CantidadRegistrosPorPagina);

            var actores = await queryable.Paginar(paginacionDTO).ToListAsync(); //debe implementarse paginacion
            return _mapper.Map<List<ActorDTO>>(actores);
        }

        [HttpGet("{id}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidad = await this._context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                NotFound();
            }

            return _mapper.Map<ActorDTO>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Creacion([FromForm] ActorCreacionDTO actorCreacion)
        {
            var entidad = _mapper.Map<Actor>(actorCreacion);

            if (actorCreacion.Foto != null)
            {
                using (var memoryString = new MemoryStream())
                {
                    await actorCreacion.Foto.CopyToAsync(memoryString);
                    var contenido = memoryString.ToArray();
                    var extension = Path.GetExtension(actorCreacion.Foto.FileName);

                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, actorCreacion.Foto.ContentType);
                }
            }


            _context.Add(entidad);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument is null)
            {
                return BadRequest();
            }


            var entidaDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (entidaDB is null)
            {
                return NotFound();
            }

            var entidadDTO = _mapper.Map<ActorPatchDTO>(entidaDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esvalido = TryValidateModel(entidadDTO);

            if (!esvalido)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(entidadDTO, entidaDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacion)
        {
            //solo se actualiza lo necesario
            var actorDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);


            if (actorDB == null)
            {
                return NotFound();
            }

            actorDB = _mapper.Map(actorCreacion,actorDB);

            if (actorCreacion.Foto != null)
            {
                using (var memoryString = new MemoryStream())
                {
                    await actorCreacion.Foto.CopyToAsync(memoryString);
                    var contenido = memoryString.ToArray();
                    var extension = Path.GetExtension(actorCreacion.Foto.FileName);

                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,actorDB.Foto, actorCreacion.Foto.ContentType);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entidad = await _context.Actores.AnyAsync(x => x.Id == id);

            if (!entidad)
            {
                return NotFound();
            }

            _context.Remove( new Actor {Id = id });

            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}
