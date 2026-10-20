using PathFinderService.Domain.Obsolete.Dijkstra;
using PathFinderService.Domain.Services.Pathfinding.DistanceMatrix;
using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderService.Domain.Tests.PathFinder.Dijkstra
{
    public class DijkstraTests
    {
        [Fact]
        public void Dijkstra_DeveRetornarDistanciasValidas_ParaGrafoValido()
        {
            // Arrange
            var algorithm = new DijkstraV1_Test();

            Letras[] vertices =
            {
                Letras.A,
                Letras.B,
                Letras.C,
                Letras.D,
                Letras.E
            };

            Arestas[] arestas =
            {
                new(Letras.A, Letras.B, 5),
                new(Letras.A, Letras.C, 1),

                new(Letras.B, Letras.C, 4),
                new(Letras.B, Letras.D, 2),
                new(Letras.B, Letras.E, 4),

                new(Letras.C, Letras.B, 2),
                new(Letras.C, Letras.D, 5),
                new(Letras.C, Letras.E, 5),

                new(Letras.D, Letras.E, 2)
            };


            // Act
            var resultado = algorithm.Dijkstra(
                vertices, arestas, Letras.A);


            // Assert: 
            Assert.All(vertices, vertice =>
            {
                Assert.True(resultado.ContainsKey(vertice));
            });

            // Assert: 
            Assert.Equal(0, resultado[Letras.A]);

            // Assert: 
            Assert.All(resultado.Values, distancia =>
            {
                Assert.True(
                    distancia >= 0,
                    $"Distância inválida encontrada: {distancia}");
            });
        }



        [Fact]
        public void DijkstraV2_DeveRetornarDistanciasValidas_ParaGrafoValido()
        {
            // Arrange
            var algorithm = new DijkstraV2_Test();

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

            var start = (x: 1, y: 1);

            // Act
            var resultado = algorithm.Dijkstra(galpao, start);

            // Assert:
            Assert.All(resultado.Values, distancia =>
            {
                Assert.True(
                    distancia >= 0,
                    $"Distância inválida encontrada: {distancia}");
            });

            // Assert:
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
        }

        [Fact]
        public void DijkstraV2_ExisteCaminhoInalcancavel_No_Grafo()
        {
            // Arrange
            var algorithm = new DijkstraV2_Test();

            string[,] galpao =
            {
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" },
                { "#", ".", ".", ".", "#", ".", ".", ".", ".", ".", ".", "#" },
                { "#", ".", "#", ".", "#", ".", "#", "#", "#", ".", "#", "#" },
                { "#", ".", "#", ".", ".", ".", ".", ".", "#", ".", ".", "#" },
                { "#", ".", "#", "#", "#", "#", ".", "#", "#", ".", "#", "#" },
                { "#", ".", ".", ".", ".", ".", ".", "#", ".", "#", ".", "#" },
                { "#", "#", "#", "#", "#", ".", "#", ".", ".", "#", ".", "#" },
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" }
            };

            var start = (x: 1, y: 1);

            // Act
            var resultado = algorithm.Dijkstra(galpao, start);

            // Assert:
            Assert.Contains(resultado.Values, distancia =>
                distancia == int.MaxValue);
        }
    }
}
