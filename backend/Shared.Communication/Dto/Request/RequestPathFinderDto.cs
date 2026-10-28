using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Communication.Dto.Request
{
    public class RequestPathFinderDto
    {
        public string[,] Map { get; set; }
        public (int x,int y) Start { get; set; }
        public (int x,int y) End { get; set; }
        public List<(string Nome, (int x, int y) Posicao)> Pickups { get; set; }
    }
}