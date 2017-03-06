using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Helpers.Graph
{
    public class Cycle<T>
    {
        public List<Vertex<T>> Vertices { get; set; }

        public Cycle()
        {
            Vertices = new List<Vertex<T>>();
        }

        public void AddVertex(Vertex<T> v)
        {
            Vertices.Add(v);
        }
    }
}