using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderService.Domain.Services.Pathfinding.DistanceMatrix
{
    public class DistanceMatrix
    {

        private readonly DijkstraV2_Test Dijkstra_alg;

        public DistanceMatrix(DijkstraV2_Test algoritmo)
        {
            Dijkstra_alg = algoritmo;
        }
        public int[,] DistanceMatrixProvider(Request order)
        {
            var pontos = InterestPoints(order, order.Map);

            int quantidadePontos = pontos.Count;

            int[,] matrizDistancias = new int[
            quantidadePontos,
            quantidadePontos
            ];

            for (int i = 0; i < quantidadePontos; i++)
            {
                for (int j = 0; j < quantidadePontos; j++)
                {
                    matrizDistancias[i, j] = int.MaxValue;
                }

                matrizDistancias[i, i] = 0;
            }

            for (int i = 0; i < quantidadePontos - 1; i++)
            {
                var origem = pontos[i].Posicao;

                var distancias = Dijkstra_alg.Dijkstra(order.Map, origem);

                for (int j = 0; j < quantidadePontos; j++)
                {
                    var destino = pontos[j].Posicao;

                    int distancia = distancias[destino];

                    matrizDistancias[i, j] = distancia;

                    // Grafo simétrico, logo podemos preencher a matriz de distâncias de forma simétrica
                    matrizDistancias[j, i] = distancia;
                }
            }

            return matrizDistancias; 
        }

        public List<(string Nome, (int x, int y) Posicao)> InterestPoints(Request order, string[,] Map)
        {
            var pontos = new List<(string Nome, (int x, int y) Posicao)>();
            pontos.Add(("Start", order.Start));

            foreach (var pickup in order.Pickups.OrderBy(p => p.Posicao))
            {
                pontos.Add(pickup);
            }
            pontos.Add(("End", order.End));

            foreach (var ponto in pontos)
            {
                int x = ponto.Posicao.x;
                int y = ponto.Posicao.y;

                if (x < 0 || x >= Map.GetLength(0) ||
                    y < 0 || y >= Map.GetLength(1) ||
                    Map[x, y] != ".")
                {
                    throw new ArgumentException(
                        $"O ponto {ponto.Nome} não está em uma célula navegável.");
                }
            }

            return pontos;
        }
    }

    public record Request()
    {
        public string[,] Map { get; set; }
        public (int x,int y) Start { get; set; }
        public (int x,int y) End { get; set; }
        public List<(string Nome, (int x, int y) Posicao)> Pickups { get; set; }
    }
}
