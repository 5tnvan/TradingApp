using Application.DbModels;
using Application.Helpers.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Helpers.Graph
{
    public class Graph<T>
    {
        public List<Vertex<T>> Vertices { get; set; }
        private Stack<Vertex<T>> Stack { get; set; }
        private Vertex<T> Root { get; set; }
        public List<Cycle<T>> Cycles { get; set; }

        public Graph()
        {
            Vertices = new List<Vertex<T>>();
            Stack = new Stack<Vertex<T>>();
            Cycles = new List<Cycle<T>>();
        }

        public void AddVertex(Vertex<T> vertex)
        {
            Vertices.Add(vertex);
        }

        public List<Cycle<T>> GetCyclesContainingVertex(Vertex<T> v)
        {
            //set root
            v.Index = 0;
            Root = v;
            Stack.Push(v);

            GetCycles(v);

            return Cycles;
        }
        public void GetCycles(Vertex<T> v)
        {

            //Consider children of v
            foreach (var c in v.AdjacencyList)
            {
                int temp = c.Index;
                c.Index = v.Index + 1;

                if (Stack.Count() > c.Index) ClearStack(c.Index);

                if (Stack.Contains(c) && c != Root)
                {
                    c.Index = temp;
                    continue;
                }
                else if (c != Root)
                {
                    Stack.Push(c);
                    GetCycles(c);
                }
                else
                {
                    // Cycle found
                    c.Index = 0;
                    Cycles.Add(PrintCycle());
                }
            }

        }

        public void ClearStack(int index)
        {
            do
            {
                Stack.Pop();
            }
            while (Stack.Count() >= index + 1);
        }

        public Cycle<T> PrintCycle()
        {   
            var cycle = new Cycle<T>();
            Vertex<T>[] temp = Stack.ToArray();
            Array.Reverse(temp);

            foreach (var vertex in temp)
            {
                cycle.AddVertex(vertex);
            }

            return cycle;
        }

    }
}