using PathFinderService.Domain.Services.Pathfinding.DistanceMatrix;
using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;

internal class Program
{
    private static void Main(string[] args)
    {
        

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


        PrintaMatrizDistancia(request);


    }

    public static void PrintaMatrizDistancia(Request rqst)
    {
        Console.Write("\t");

        var matrizDistancias = new DistanceMatrix(
                new DijkstraV2_Test()
            );


        var pontos = matrizDistancias.InterestPoints(rqst, rqst.Map);

        var matrizDistanciasResult = matrizDistancias.DistanceMatrixProvider(rqst);

        foreach (var ponto in pontos)
            Console.Write($"{ponto.Nome}\t");

        Console.WriteLine();

        for (int i = 0; i < pontos.Count; i++)
        {
            Console.Write($"{pontos[i].Nome}\t");

            for (int j = 0; j < pontos.Count; j++)
            {
                int distancia = matrizDistanciasResult[i, j];

                string valor = distancia == int.MaxValue
                    ? "INF"
                    : distancia.ToString();

                Console.Write($"{valor}\t");
            }

            Console.WriteLine();
        }
    }
}