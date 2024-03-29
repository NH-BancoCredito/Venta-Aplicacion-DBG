﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ConsultarProductosValidator : AbstractValidator<ConsultarProductosRequest>
    {
        public ConsultarProductosValidator()
        {
            RuleFor(item => item.filtroPorNombre).NotEmpty().WithMessage("Debe especificar un filtro");
        }
    }
}
