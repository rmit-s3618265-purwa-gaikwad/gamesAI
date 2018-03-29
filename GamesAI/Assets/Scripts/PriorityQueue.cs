using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GamesAI
{
    public class PriorityQueue
    {
        private List<Node> data;

        public PriorityQueue()
        {
            this.data = new List<Node>();
        }

        public void enqueue(Node newNode)
        {
            int index = 0;
            foreach (Node node in data)
            {
                if (node.getEstimatedTotalCost() > newNode.getEstimatedTotalCost())
                {
                    break;
                }
                index++;
            }
            this.data.Insert(index, newNode);
        }

        public Node dequeue()
        {
            return this.data[0];
        }

        public bool isEmpty()
        {
            if (data.Count == 0)
                return true;
            return false;
        }

        public bool contains(Node node)
        {
            return data.Contains(node);
        }

        public void removeNode(Node node)
        {
            data.Remove(node);
        }
        public void addNode(Node node)
        {
            data.Add(node);
        }
    }
}

