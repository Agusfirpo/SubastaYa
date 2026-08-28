using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Enums;

namespace Dominio.Entities
{
    public class TransaccionLedger
    {
        public int Id { get; set; }

        public int BilleteraId {  get; set; }

        public int? SubastaId {  get; set; }

        public TipoTransaccion Tipo {  get; set; }

        public decimal Monto { get; set; }  

        public DateTime Fecha { get; set; } 

        public Billetera Billetera { get; set; } 

        public Subasta? Subasta { get; set; }
    }
}
