using Application.Core;
using Application.DbModels;
using Application.Helpers.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Helpers.Matches
{
    public static class Matches
    {

        public static List<Cycle<long>> GetCyclesOfUsers()
        {

            //get data from db

            var buyRequests = AppCore.Db.BuyRequests.Where(x => x.IsLive == true).ToList();

            //initialize graph of buy requests
            var graph = new Graph<long>();

            //add vertices (my buy requests)
            foreach (var req in buyRequests)
            {
                var vertex = new Vertex<long>(req.UserId);
                if (!graph.Vertices.Any(x => x.Value == vertex.Value))
                {
                    graph.AddVertex(vertex);
                }                
            }

            //add neighbours
            foreach (var req in buyRequests)
            {                
                var neighbour = graph.Vertices.Where(x => x.Value == req.Product.UserId).FirstOrDefault();
                var vertex = graph.Vertices.Where(x => x.Value == req.UserId).First();
                
                if (neighbour != null && !vertex.AdjacencyList.Any(x => x.Value == neighbour.Value))
                {
                    vertex.AdjacencyList.Add(neighbour);
                }
            }

            //get cycles in graph
            var res = new List<Cycle<long>>();
            var root = graph.Vertices.Where(x => x.Value == AppCore.User.Id).FirstOrDefault();

            if (root != null)
            {
                res = graph.GetCyclesContainingVertex(root);
            }           

            return res;
        }

        public static List<List<BuyRequest>> GetCyclesOfBuyRequests(List<Cycle<long>> cyclesOfUsers)
        {
            var res = new List<List<List<BuyRequest>>>();

            foreach (var cycle in cyclesOfUsers)
            {
                var temp = new List<List<BuyRequest>>();
                int i = 0;

                //foreach user in cycle of users
                foreach (var vertex in cycle.Vertices)
                {
                    var next = cycle.Vertices.First();

                    if (vertex != cycle.Vertices.Last())
                    {
                        next = cycle.Vertices[i + 1];
                    }

                    var buyRequest = AppCore.Db.BuyRequests.Where(x => x.UserId == vertex.Value && x.Product.UserId == next.Value && x.IsLive == true).ToList();
                    temp.Add(buyRequest);

                    i++;
                }

                // get all combinations of temp
                res.Add(GetAllCombinationsOf(temp));
            }

            return res.SelectMany(x => x).ToList();
        }

        public static List<List<T>> GetAllCombinationsOf<T>(List<List<T>> sets)
        {
            // need array bounds checking etc for production
            var combinations = new List<List<T>>();

            // prime the data
            foreach (var value in sets[0])
                combinations.Add(new List<T> { value });

            foreach (var set in sets.Skip(1))
                combinations = AddExtraSet(combinations, set);

            return combinations;
        }

        private static List<List<T>> AddExtraSet<T>(List<List<T>> combinations, List<T> set)
        {
            var newCombinations = from value in set
                                  from combination in combinations
                                  select new List<T>(combination) { value };

            return newCombinations.ToList();
        }

    }

}