using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {

        public int Pagina { get; set; } = 1;
        private int cantidadRegistrosPorPagina { get; set; } = 10;
        private readonly int cantidadMaximaRegistrosPagina = 50;


        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistrosPorPagina;
            set
            {
                cantidadRegistrosPorPagina = (value > cantidadMaximaRegistrosPagina) ? cantidadMaximaRegistrosPagina : value;
            }
        }
    }
}
