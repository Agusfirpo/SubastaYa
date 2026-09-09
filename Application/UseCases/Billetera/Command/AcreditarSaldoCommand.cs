using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Billetera.Command
{
    public class AcreditarSaldoCommand
    {
        public int UsuarioId { get; set; }

        public decimal Monto { get; set; }
    }
}
