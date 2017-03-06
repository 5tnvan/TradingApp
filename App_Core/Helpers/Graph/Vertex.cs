using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.DbModels;

namespace Application.Helpers.Graph
{
    public class Vertex<T>
    {
        public T Value { get; set; }
        public List<Vertex<T>> AdjacencyList { get; set; }
        public int Index { get; set; }

        public Vertex(T value)
        {
            Value = value;
            AdjacencyList = new List<Vertex<T>>();
            Index = -1;
        }

        public void AddToVertex(Vertex<T> vertex)
        {
            AdjacencyList.Add(vertex);
        }
    }
}