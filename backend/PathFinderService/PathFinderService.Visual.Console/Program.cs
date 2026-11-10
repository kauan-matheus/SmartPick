using PathFinderService.Domain.Services.Pathfinding.DistanceMatrix;
using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;
using PathFinderService.Domain.Services.Pathfinding.RouteOptimizer;

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
                ("P2",(6, 7)),
                ("P3",(5, 4)),
            }
        };

        var matrizDistancias = new DistanceMatrix(
               new DijkstraV2_Test()
           );

        var matrizDistanciasResult = matrizDistancias.DistanceMatrixProvider(request);
       // PrintaMatrizDistancia(request);


        var rst = HeldKarp.FindBestRoute(matrizDistanciasResult, out var result, out var matrix);

        foreach ( var r in rst[1..^1])
        {
            //Console.WriteLine(r);
            Console.WriteLine(request.Pickups[r-1]);
        }


    }

    static void PrintMatrix(long[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] > 10000)
                {
                    Console.Write(" inf ");
                    continue;
                }

                Console.Write($"{matrix[i, j],4}");
            }

            Console.WriteLine();
        }
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