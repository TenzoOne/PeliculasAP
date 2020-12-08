using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Context;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : CustomBaseController
    {
        //private readonly ApplicationDBContext _context;
        //private readonly IMapper _mapper;
        public GenerosController(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {
            //this._context = context;
            //this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {

            return await Get<Genero, GeneroDTO>();
            //se quita porque se implementa CustomBaseController
            //var entidades = await _context.Generos.ToListAsync();
            //return _mapper.Map<List<GeneroDTO>>(entidades);
        }

        [HttpGet("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {

            return await Get<Genero, GeneroDTO>(id);

            //se implementa metodo generico
            //var entidad = await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            //if (entidad == null)
            //{
            //    return NotFound();
            //}

            //return _mapper.Map<GeneroDTO>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacion)
        {

            return await Post<GeneroCreacionDTO,Genero,GeneroDTO>(generoCreacion, "ObtenerGenero");

            //var entidad = _mapper.Map<Genero>(generoCreacion);
            //_context.Add(entidad);
            //await _context.SaveChangesAsync();

            //var generoDTO = _mapper.Map<GeneroDTO>(entidad);

            //return new CreatedAtRouteResult("ObtenerGenero", new { id = generoDTO.Id }, generoDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Put<GeneroCreacionDTO, Genero>(id,generoCreacionDTO);
            //var entidad = _mapper.Map<Genero>(generoCreacionDTO);
            //entidad.Id = id;
            //_context.Entry(entidad).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            //return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            return await Delete<Genero>(id);
            //var existe = await _context.Generos.AnyAsync(x => x.Id == id);

            //if (!existe)
            //{
            //    NotFound();
            //}

            //_context.Remove(new Genero { Id = id });
            //await _context.SaveChangesAsync();
            //return NoContent();
        }



    }
}
