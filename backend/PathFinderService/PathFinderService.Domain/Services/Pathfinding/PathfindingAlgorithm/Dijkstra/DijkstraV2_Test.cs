using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra
{
    public class DijkstraV2_Test
    {

        //Classe para testar o algoritmo de Dijkstra, essa versão pede como parâmetro um ponto start sendo esse uma tupla (X e Y) e agora com a diferença que precisa
        //de um mapa real no formato de matriz de strings considerando "#" como paredes e "." como caminho andavel.

        //O retorno é um dicionário (grafo) representando as distâncias mínimas referente ao ponto incial ás demais células da matriz.


        //Posterioremente essa versão poderá substituída pelo algoritmo A*.

        public Dictionary<(int x, int y), int> Dijkstra(
        string[,] map,
        (int x, int y) start)
        {
            var distancias = MatrixToGraph(map);

            if (!distancias.ContainsKey(start))
            {
                throw new ArgumentException(
                    $"A origem {start} não é uma célula navegável.");
            }

            distancias[start] = 0;

            var celulasVisitadas = new HashSet<(int x, int y)>();


            var direcoes = new (int dx, int dy)[]
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };

            while (true)
            {
                var verticesNaoVisitados = distancias
                    .Where(v => !celulasVisitadas.Contains(v.Key))
                    .ToList();

                if (verticesNaoVisitados.Count == 0)
                    break;

                var verticeAtual = verticesNaoVisitados
                    .MinBy(v => v.Value);

               
                if (verticeAtual.Value == int.MaxValue)
                    break;

                celulasVisitadas.Add(verticeAtual.Key);

                foreach (var direcao in direcoes)
                {
                    int nx = verticeAtual.Key.x + direcao.dx;
                    int ny = verticeAtual.Key.y + direcao.dy;

                    
                    if (nx < 0 || nx >= map.GetLength(0) ||
                        ny < 0 || ny >= map.GetLength(1))
                    {
                        continue;
                    }

                   
                    if (map[nx, ny] != ".")
                        continue;

                    var vizinho = (x: nx, y: ny);

                    int novaDistancia = verticeAtual.Value + 1;

                    if (novaDistancia < distancias[vizinho])
                        distancias[vizinho] = novaDistancia;
                }
            }

            return distancias;
        }


        Dictionary<(int x, int y), int> MatrixToGraph(string[,] map)
        {
            var graph = new Dictionary<(int x, int y), int>();

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == ".")
                        graph.Add((x, y), int.MaxValue);
                }
            }

            return graph;
        }
    }
}
