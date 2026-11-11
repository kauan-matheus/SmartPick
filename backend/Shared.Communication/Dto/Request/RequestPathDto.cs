using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Communication.Dto.Request
{
    public class Ponto
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class PickupDto
    {
        public required string Nome { get; set; }
        public required Ponto Posicao { get; set; }
    }

    public class RequestPathDto
    {
        public required string[][] Map { get; set; }
        public required Ponto Start { get; set; }
        public required Ponto End { get; set; }
        public required List<PickupDto> Pickups { get; set; }
    }
}