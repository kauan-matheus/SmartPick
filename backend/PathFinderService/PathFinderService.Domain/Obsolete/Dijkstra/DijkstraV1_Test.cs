using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderService.Domain.Obsolete.Dijkstra
{
    [Obsolete]
    public class DijkstraV1_Test
    {
        //Classe para testar o algoritmo de Dijkstra, essa versão usa de um Grafo já definido, com vértices e arestas definidas,
        // e retorna esse grafo com as distância mínima ca de um vértice inicial para todos os outros vértices do grafo.

        //Essa versão opera com o envio de um grafo + arestas usando os objetos de valores do dominio.

        //Posterioremente essa versão será substituída por uma versão que opera com um mapa de caracteres no modelo array de strings, que pode ser enviado de forma dinâmica.

        public Dictionary<Letras, int> Dijkstra(
            Letras[] Vertices,
            Arestas[] Arestas,
            Letras Inicio)
        {

            Dictionary<Letras, int> distancia =
                Vertices.ToDictionary(
                    v => v,
                    v => int.MaxValue
                );

            HashSet<Letras> Visitados = new HashSet<Letras>();

            distancia[Inicio] = 0;

            while (true)
            {

                // Verificação básica para pegar a lista de vértices candidatos (ainda não vistos).
                var verticesNaoVistos = distancia
                    .Where(v => !Visitados.Contains(v.Key));

                // Se todos foram analisados, pode quebrar o loop.
                if (!verticesNaoVistos.Any())
                    break;

                // A escolha do vértice atual é dada por escolha do menor 'd'.
                // Logo, o algoritmo escolhe o vértice não visitado de menor distância conhecida.
                var verticeAtual =
                    verticesNaoVistos.MinBy(v => v.Value);

                // Valida se não ocorreu um erro de selecionar uma rota com distância infinita, ou seja, não há mais rotas possíveis.
                if (verticeAtual.Value == int.MaxValue)
                    break;

                // Esse loop é o que faz o algoritmo brilhar, pois o algoritmo escolhe sempre a melhor rota para cruzar o grafo,
                // mas, para saber o valor dos demais vértices, temos um loop que roda no meio do trajeto do vértice escolhido,
                // analisando o peso das arestas e os vizinhos conectados a ele.
                foreach (var arestaAdjacente in Arestas.Where(a => a.Inicio == verticeAtual.Key))
                {
                    // Faz a soma da distância do vértice atual + a distância para chegar no vértice vizinho.
                    // Aqui ele já é somado com o valor percorrido até agora pelo vértice atual.
                    var distanciaVizinha = verticeAtual.Value + arestaAdjacente.Distancia;

                    // Ao descobrir, salvamos o valor encontrado para o vértice vizinho através desse vértice analisado.
                    var valorVerticeVizinho = distancia[arestaAdjacente.Fim];

                    // Com essa informação, podemos ajustar as rotas, mostrando que podem existir rotas melhores
                    // para um determinado nó vizinho que ainda não exploramos.
                    if (distanciaVizinha < valorVerticeVizinho)
                    {
                        distancia[arestaAdjacente.Fim] = distanciaVizinha;
                    }
                }

                // Após completarmos a vistoria sobre um vértice, nós adicionamos ele à lista dos já visitados.
                Visitados.Add(verticeAtual.Key);
            }

            return distancia;
        }

       
    }

    public struct Arestas(Letras Inicio, Letras Fim, int distancia)
    {
        public Letras Inicio = Inicio;
        public Letras Fim = Fim;
        public int Distancia = distancia;
    }

    public enum Letras
    {
        A, B, C, D, E, F, G, H, I, J
    }
}
