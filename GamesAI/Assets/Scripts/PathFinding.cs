using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GamesAI
{
	public class PathFinding: MonoBehaviour
    {
		GridPlane grid;
		public Transform start;
		public Transform target;
		public List<Node> path;
		int val = 1;
		void Awake() {
			grid = GetComponent<GridPlane> ();
		}

		void Update() {
			if (val == 1) {
				process (start.position, target.position);
				val = 2;
			}
		}

		public List<Node> process(Vector3 startVector, Vector3 targetVector)
        {
			Node startNode = grid.NodeFromWorldPoint(startVector);
			Node targetNode = grid.NodeFromWorldPoint(targetVector);

			if (startNode == null)
				Debug.Log ("startNode is null");
			if (targetNode == null)
				Debug.Log ("targetNode is null");
			
			PriorityQueue openlist = new PriorityQueue();
			PriorityQueue closedlist = new PriorityQueue();          
            Node currentNode = null;
			openlist.addNode (startNode);
            while (!openlist.isEmpty())
            {
                currentNode = openlist.dequeue();
				//Debug.Log ("openlist size = " +  openlist.getSize());
				//Debug.Log (string.Format("Current node is {0}, {1}", currentNode.getIndexX(), currentNode.getIndexY()));
                if (currentNode.Equals(targetNode))
                {
                    break;
                }
				List<Node> neighbors = grid.GetNeighbours(currentNode);
                foreach (Node neighbor in neighbors)
                {
                    double neighborCost = 1 + currentNode.getCostSoFar();
                    double neighborHeuristics = 0;
					if (closedlist.contains(neighbor))
                    {
						if (neighbor.getCostSoFar() > neighborCost)
                        {
							closedlist.removeNode(neighbor);
							neighborHeuristics = neighbor.getEstimatedTotalCost() - neighbor.getCostSoFar();
                        }
                        else
                            continue;
                    }
					else if (openlist.contains(neighbor))
                    {
						if (neighbor.getCostSoFar() > neighborCost)
                        {
							neighborHeuristics = neighbor.getEstimatedTotalCost() - neighbor.getCostSoFar();
                        }
                        else
                            continue;
                    }
                    else
                    {
						neighborHeuristics = neighbor.getHeuristicValue(targetNode);
                    }
					neighbor.setCostSoFar(neighborCost);
					neighbor.setEstimatedTotalCost(neighborCost + neighborHeuristics);
					neighbor.setFromNode(currentNode);
					if (!openlist.contains(neighbor))
                    {
						openlist.enqueue(neighbor);
                    }
                }
                openlist.removeNode(currentNode);
                closedlist.addNode(currentNode);
            }
           	if (!currentNode.Equals(targetNode))
            {
                return null;
            }
            else
            {
                path = new List<Node>();
				grid.path = new List<Node>();
                while (!currentNode.Equals(startNode))
                {
					Debug.Log (string.Format("Current node is {0}, {1}", currentNode.getIndexX(), currentNode.getIndexY()));
					grid.path.Add (currentNode);
                    path.Add(currentNode);
                    currentNode = currentNode.getFromNode();
                }
				grid.path.Reverse ();
                path.Reverse();
                return path;
            }
        }
    }
}