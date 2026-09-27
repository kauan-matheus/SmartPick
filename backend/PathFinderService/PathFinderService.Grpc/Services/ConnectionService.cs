using Grpc.Core;
using PathFinderService.Grpc.Protos;

namespace PathFinderService.Grpc.Services
{
    public class ConnectionService : ConnectionTest.ConnectionTestBase
    {
        private readonly ILogger<ConnectionService> _logger;
        public ConnectionService(ILogger<ConnectionService> logger)
        {
            _logger = logger;
        }

        public override Task<ConnectionReply> TestConnection(ConnectionRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ConnectionReply
            {
                Message = "Conectado com sucesso"
            });
        }
    }
}
