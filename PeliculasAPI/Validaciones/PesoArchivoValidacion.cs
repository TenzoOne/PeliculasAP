using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Validaciones
{
    public class PesoArchivoValidacion : ValidationAttribute
    {
        private readonly int pesoMaximoMegaBytes;
        public PesoArchivoValidacion(int pesoMaximoMegaBytes)
        {
            this.pesoMaximoMegaBytes = pesoMaximoMegaBytes;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > pesoMaximoMegaBytes * 1024 * 1024 )
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaximoMegaBytes}mb");
            }

            return ValidationResult.Success;


        }
    }
}
