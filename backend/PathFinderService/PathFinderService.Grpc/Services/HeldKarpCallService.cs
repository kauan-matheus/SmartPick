using Grpc.Core;
using PathFinderService.Domain.Services.Pathfinding.DistanceMatrix;
using PathFinderService.Domain.Services.Pathfinding.PathfindingAlgorithm.Dijkstra;
using PathFinderService.Domain.Services.Pathfinding.RouteOptimizer;
using Shared.Communication.Protos;

namespace PathFinderService.Grpc.Services
{
    public class HeldKarpCallService : HeldKarpCall.HeldKarpCallBase
    {
        private readonly ILogger<ConnectionService> _logger;
        public HeldKarpCallService(ILogger<ConnectionService> logger)
        {
            _logger = logger;
        }

        public override Task<CallReply> HeldCarpCall(CallRequest request, ServerCallContext context)
        {
            var matrizDistancias = new DistanceMatrix(
                new DijkstraV2_Test()
            );

            var matrixRequest = ConvertRequest(request);

            var matrizDistanciasResult = matrizDistancias.DistanceMatrixProvider(matrixRequest);

            var rota = HeldKarp.FindBestRoute(matrizDistanciasResult, out var result, out var matrix);

            var reply = new CallReply{};

            foreach (var val in rota)
            {
                reply.Path.Add(val);
            }

            return Task.FromResult(reply);
        }

        private Request ConvertRequest(CallRequest request)
        {
            int width = request.Map.Width;
            int height = request.Map.Height;

            if (request.Map.Cells.Count != width * height)
                throw new ArgumentException(
                    $"Mapa inválido: {request.Map.Cells.Count} células para {width}x{height}.");

            var map = new string[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map[y, x] = request.Map.Cells[y * width + x] == 1 ? "#" : ".";

            return new Request
            {
                Map = map,
                Start = (request.Start.X, request.Start.Y),
                End = (request.End.X, request.End.Y),
                Pickups = request.Pickups
                    .Select(p => (p.Name, (p.Position.X, p.Position.Y)))
                    .ToList()
            };
        }
    }
}
