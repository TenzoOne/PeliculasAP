using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Validaciones
{
    public class TipoArchivoValidaciones : ValidationAttribute
    {
        private readonly string[] tiposValidos;
        public TipoArchivoValidaciones(string[] tiposValidos)
        {
            this.tiposValidos = tiposValidos;
        }

        public TipoArchivoValidaciones(GrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen) 
            {
                tiposValidos = new string[] { "imagen/jpeg","image/png","image/gif"};
            };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile is null)
            {
                return ValidationResult.Success;
            }

            if (!tiposValidos.Contains(formFile.ContentType))
            {
             
                return new ValidationResult($"El tipo del archivo debe ser uno de los siguientes: {string.Join(", ",tiposValidos)}");
            }

            return ValidationResult.Success;
        }
    }
}
