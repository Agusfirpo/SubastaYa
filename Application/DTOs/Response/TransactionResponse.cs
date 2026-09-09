using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public int? SubastaId { get; set; }


    }
}
