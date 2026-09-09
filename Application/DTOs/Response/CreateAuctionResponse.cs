using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.DTOs.Response
{
    public class CreateAuctionResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; } 

        public string Estado { get; set; } 
    }
}