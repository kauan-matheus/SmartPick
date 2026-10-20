using PathFinderService.Domain.Services.Pathfinding.DistanceMatrix;
using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderService.Domain.Tests.PathFinder.DistanceMatrixTestes
{
    public class DistanceMatrixTests
    {

        [Fact]
        public void GerarMatrizDistancia_com_RequestValida()
        {
            // Arrange
            var algorithm = new DistanceMatrix(
                    new DijkstraV2_Test()
                );


            string[,] galpao =
            {
                 //  0    1    2    3    4    5    6    7    8    9   10   11
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" }, // 0
                { "#", ".", ".", ".", "#", ".", ".", ".", ".", ".", ".", "#" }, // 1
                { "#", ".", "#", ".", "#", ".", "#", "#", "#", ".", "#", "#" }, // 2
                { "#", ".", "#", ".", ".", ".", ".", ".", "#", ".", ".", "#" }, // 3
                { "#", ".", "#", "#", "#", "#", ".", "#", "#", ".", "#", "#" }, // 4
                { "#", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#" }, // 5
                { "#", "#", "#", "#", "#", ".", "#", ".", ".", ".", ".", "#" }, // 6
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" }  // 7
            };



            var request = new Request
            {
                Map = galpao,
                Start = (x: 1, y: 1),
                End = (x: 1, y: 10),
                Pickups = new List<(string Nome, (int x, int y) Posicao)>
                {
                    ("P1",(3, 3)),
                    ("P2",(6, 7))
                }
            };

            // Act
            var resultado = algorithm.DistanceMatrixProvider(request);

           
            // Assert:
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
        }
    }
}
