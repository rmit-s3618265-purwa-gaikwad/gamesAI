using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GamesAI
{
	public class PathFinding: MonoBehaviour
    {
		public GridPlane grid;
		/*void Awake() {
			grid = GetComponent<GridPlane> ();
		}*/

		public List<Node> process(Vector3 startVector, Vector3 targetVector)
        {
			Node startNode = grid.NodeFromWorldPoint(startVector);
			Node targetNode = grid.NodeFromWorldPoint(targetVector);

		    if (!startNode.getWalkable())
		    {
		        startNode = NearestWalkableNode(startNode);
            }
		    if (!targetNode.getWalkable())
		    {
		        targetNode = NearestWalkableNode(targetNode);
		    }

			PriorityQueue openlist = new PriorityQueue();
			PriorityQueue closedlist = new PriorityQueue();          
            List<Node> touchedList = new List<Node>();
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
                    if(!neighbor.getWalkable()) continue;

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
                    if (!touchedList.Contains(neighbor))
                    {
                        touchedList.Add(neighbor);
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
		    if (currentNode == null || !currentNode.Equals(targetNode))
		    {
                ResetTouched(touchedList);
		        return null;
		    }

		    var path = new List<Node>();
		    Node prevPrevNode = null;
		    Node prevNode = null;
		    while (!currentNode.Equals(startNode))
		    {
		        if (prevPrevNode == null)
		        {
		            if (prevNode == null)
		            {
                        path.Add(currentNode);
                    }
		            prevPrevNode = prevNode;
		            prevNode = currentNode;
                    currentNode = currentNode.getFromNode();
		        }
		        else
		        {
		            Vector2Int prevToPrevPrev = (prevPrevNode.Index - prevNode.Index).Sign();
		            Vector2Int currentToPrev = (prevNode.Index - currentNode.Index).Sign();
		            if (prevToPrevPrev != currentToPrev)
		            {
		                path.Add(prevNode);
		                prevPrevNode = prevNode;
		                prevNode = currentNode;
		                currentNode = currentNode.getFromNode();
		            }
		            else
		            {
                        prevNode = currentNode;
                        currentNode = currentNode.getFromNode();
                    }
		        }
		    }
            path.Add(startNode);
            path.Reverse();
		    grid.path = path.ToArray().ToList();
            ResetTouched(touchedList);
		    return path;
        }

	    private void ResetTouched(List<Node> touched)
	    {
	        foreach (Node node in touched)
	        {
	            node.reset();
	        }
	    }
        
        private Node NearestWalkableNode(Node startNode)
        {
            var openlist = new PriorityQueue();
            var closedlist = new PriorityQueue();
            var touchedList = new List<Node>();
            Node currentNode = null;
            openlist.addNode(startNode);
            while (!openlist.isEmpty())
            {
                currentNode = openlist.dequeue();
                //Debug.Log ("openlist size = " +  openlist.getSize());
                //Debug.Log ($"Current node is {currentNode.getIndexX()}, {currentNode.getIndexY()}");
                if (currentNode.getWalkable())
                {
                    break;
                }
                List<Node> neighbors = grid.GetNeighbours(currentNode);
                foreach (Node neighbor in neighbors)
                {
                    if (neighbor.getWalkable())
                    {
                        ResetTouched(touchedList);
                        return neighbor;
                    }

                    double neighborCost = 1;
                    double neighborHeuristics = neighbor.getEstimatedTotalCost();
                    if (closedlist.contains(neighbor))
                    {
                        if (neighbor.getCostSoFar() > neighborCost)
                        {
                            closedlist.removeNode(neighbor);
                        }
                        else
                            continue;
                    }
                    else if (openlist.contains(neighbor) && neighbor.getCostSoFar() <= neighborCost)
                    {
                        continue;
                    }
                    if (!touchedList.Contains(neighbor))
                    {
                        touchedList.Add(neighbor);
                    }
                    neighbor.setEstimatedTotalCost(neighborCost + neighborHeuristics);
                    if (!openlist.contains(neighbor))
                    {
                        openlist.enqueue(neighbor);
                    }
                }
                openlist.removeNode(currentNode);
                closedlist.addNode(currentNode);
            }
            ResetTouched(touchedList);
            return currentNode;
        }
    }
}