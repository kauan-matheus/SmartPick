using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderService.Domain.Services.Pathfinding.RouteOptimizer
{
    public static class HeldKarp
    {
        public static int[] FindBestRoute(
            int[,] distances,
            out long bestCost, out long[,] matrix)
        {
            int pointCount = distances.GetLength(0);

            if (distances.GetLength(1) != pointCount)
                throw new ArgumentException(
                    "A matriz de distâncias precisa ser quadrada.");

            if (pointCount < 2)
                throw new ArgumentException(
                    "A matriz precisa conter início e destino final.");

            
            int start = 0;
            int end = pointCount - 1;

            
            int pickupCount = pointCount - 2;

            
            int stateCount = 1 << pickupCount;

            
            long[,] dp = new long[stateCount, pickupCount];

            
            int[,] parent = new int[stateCount, pickupCount];

            for (int mask = 0; mask < stateCount; mask++)
            {
                for (int last = 0; last < pickupCount; last++)
                {
                    dp[mask, last] = long.MaxValue;
                    parent[mask, last] = -1;
                }
            }

            
            for (int pickup = 0; pickup < pickupCount; pickup++)
            {
                int mask = 1 << pickup;
                int point = pickup + 1;

                dp[mask, pickup] = distances[start, point];
            }

            
            for (int mask = 1; mask < stateCount; mask++)
            {
                for (int pickup = 0; pickup < pickupCount; pickup++)
                {
                    
                    if ((mask & (1 << pickup)) == 0)
                        continue;

                    long currentCost = dp[mask, pickup]; 

                    if (currentCost == long.MaxValue)
                        continue;

                    
                    for (int next = 0; next < pickupCount; next++)
                    {
                        
                        if ((mask & (1 << next)) != 0)
                            continue;
                       
                        int nextMask = mask | (1 << next);
                        int lastPoint = pickup + 1; 
                        int nextPoint = next + 1;

                        long newCost =
                            currentCost + distances[lastPoint, nextPoint]; 

                        if (newCost < dp[nextMask, next]) 
                        {
                            dp[nextMask, next] = newCost;
                            parent[nextMask, next] = pickup;
                        }
                    }
                }
            }

            
            int fullMask = stateCount - 1;
            bestCost = long.MaxValue;
            int bestLast = -1;

            for (int last = 0; last < pickupCount; last++)
            {
                long currentCost = dp[fullMask, last];

                if (currentCost == long.MaxValue)
                    continue;

                long totalCost =
                    currentCost + distances[last + 1, end];

                if (totalCost < bestCost)
                {
                    bestCost = totalCost;
                    bestLast = last;
                }
            }

            if (bestLast == -1)
                throw new InvalidOperationException(
                    "Não foi possível encontrar uma rota.");

            
            int[] route = new int[pointCount];
            route[0] = start;
            route[^1] = end;

            int currentMask = fullMask;
            int currentLast = bestLast;

            for (int position = pickupCount; position >= 1; position--)
            {
                
                route[position] = currentLast + 1;

                int previous = parent[currentMask, currentLast]; 

                currentMask ^= 1 << currentLast; 
                currentLast = previous; 
            }

            matrix = dp;
            return route;
        }
    }

}
