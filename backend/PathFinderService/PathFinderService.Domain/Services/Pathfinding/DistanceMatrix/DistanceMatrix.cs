using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;

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
            // Pega os pontos de interesse (Start, Pickups e End) do pedido
            var pontos = InterestPoints(order, order.Map);

            // Pega a quantidade de pontos de interesse
            int quantidadePontos = pontos.Count;

            // Cria uma nova matriz de distâncias com o tamanho da quantidade de pontos de interesse
            int[,] matrizDistancias = new int[
            quantidadePontos,
            quantidadePontos
            ];

            // Inicializa a matriz de distâncias com valores máximos, exceto na diagonal principal, que representa a distância de um ponto para ele mesmo
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
                // Pega a posição do ponto de interesse atual (x,y)
                var origem = pontos[i].Posicao;


                // Roda um dijkstra com a origem sendo o ponto de interesse atual, e pega as distâncias para todos os outros pontos de interesse
                var distancias = Dijkstra_alg.Dijkstra(order.Map, origem);
                // O retorno é um dicionário com a posição do ponto de interesse como chave e a distância (peso) como valor


                // Preenche a matriz de distâncias com as distâncias calculadas pelo Dijkstra
                for (int j = 0; j < quantidadePontos; j++)
                {
                    // Salvo a posição do ponto de interesse destino (x,y) condizente a coluna da matriz de distâncias
                    var destino = pontos[j].Posicao;

                    // Tendo a matriz distancia gerada pelo dijkstra já com os pesos do ponto de origem para todos os outros pontos,
                    // pego a distância (peso) do ponto de interesse destino condizente a coluna da matriz de distâncias
                    int distancia = distancias[destino];

                    // Vou na nossa matriz distância e preencho a posição condizente a linha do ponto de interesse origem e a coluna
                    // do ponto de interesse destino com a distância (peso) calculada pelo dijkstra
                    matrizDistancias[i, j] = distancia;

                    // Grafo simétrico, logo podemos preencher a matriz de distâncias de forma simétrica ou seja com a linha do ponto
                    // de interesse destino e a coluna do ponto de interesse origem com a distância (peso) calculada pelo dijkstra.

                    // Devemos ressaltar que justamente por no "for" das linhas estarmos indo até quantidadePontos - 1, a última linha
                    // da matriz de distâncias não será preenchida com a posição [i, j], mas isso não é um problema, pois a última linha da matriz de distâncias
                    // representa o ponto de interesse "End" e não há necessidade de calcular a distância do ponto de interesse "End". Mas
                    // com a simetria ou seja usando o "for" de colunas conseguimos criar essa quarta última linha, deixando a matriz quadrada.
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
        public (int x, int y) Start { get; set; }
        public (int x, int y) End { get; set; }
        public List<(string Nome, (int x, int y) Posicao)> Pickups { get; set; }
    }
}
