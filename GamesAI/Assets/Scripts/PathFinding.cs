using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GamesAI
{
    public class PathFinding
    {
        Node startNode;
        Node targetNode;
        Node node2, node3, node4;
        PriorityQueue openlist;
        PriorityQueue closedlist;
        public void init()
        {
            startNode = new Node(1, 0, 5);
            targetNode = new Node(5, 0, 0);
            node2 = new Node(2, 0, 0);
            node3 = new Node(3, 0, 0);
            node4 = new Node(4, 0, 0);

            // startNode "1" with connections (cost 1 to Node 2), (cost 3 to Node 3)
            startNode.setConnection(1, node2);
            startNode.setConnection(3, node3);

            // targetNode "5" with connections (cost 2 to Node 4)
            targetNode.setConnection(2, node4);

            // node2
            node2.setConnection(1, startNode);
            node2.setConnection(2, node4);

            //node3
            node3.setConnection(3, startNode);
            node3.setConnection(1, node4);

            //node4
            node4.setConnection(2, node2);
            node4.setConnection(1, node3);
            node4.setConnection(2, targetNode);

            // openlist
            openlist = new PriorityQueue();
            openlist.enqueue(startNode);

            //closedlist
            closedlist = new PriorityQueue();
        }
        public List<Node> process()
        {
            List<Node> path;
            Node currentNode = null;
            List<connection> currentNodeconnections;
            while (!openlist.isEmpty())
            {
                currentNode = openlist.dequeue();
                Console.WriteLine("Current Node = {0}", currentNode.getNodeId());
                if (currentNode.Equals(targetNode))
                {
                    break;
                }
                currentNodeconnections = currentNode.getConnection();
                foreach (connection currentNodeconnection in currentNodeconnections)
                {
                    Node endNode = currentNodeconnection.toNode;
                    Console.WriteLine("-- Processing connection to {0}", endNode.getNodeId());
                    double endNodeCost = currentNodeconnection.cost + currentNode.getCostSoFar();
                    double endNodeHeuristics = 0;
                    if (closedlist.contains(endNode))
                    {
                        if (endNode.getCostSoFar() > endNodeCost)
                        {
                            closedlist.removeNode(endNode);
                            endNodeHeuristics = endNode.getEstimatedTotalCost() - endNode.getCostSoFar(); // not sure
                        }
                        else
                            continue;
                    }
                    else if (openlist.contains(endNode))
                    {
                        if (endNode.getCostSoFar() > endNodeCost)
                        {
                            endNodeHeuristics = endNode.getEstimatedTotalCost() - endNode.getCostSoFar(); //not sure
                        }
                        else
                            continue;
                    }
                    else
                    {
                        endNodeHeuristics = 0;
                        if (endNode.getNodeId() == 1)
                        {
                            endNodeHeuristics = 5;
                        }
                        else if (endNode.getNodeId() == 2)
                        {
                            endNodeHeuristics = 4;
                        }
                        else if (endNode.getNodeId() == 3)
                        {
                            endNodeHeuristics = 3;
                        }
                        else if (endNode.getNodeId() == 4)
                        {
                            endNodeHeuristics = 2;
                        }
                        else if (endNode.getNodeId() == 5)
                        {
                            endNodeHeuristics = 0;
                        }
                    }
                    endNode.setCostSoFar(endNodeCost);
                    // update connections here
                    //endNode.setConnection(currentNodeConnection);
                    endNode.setEstimatedTotalCost(endNodeCost + endNodeHeuristics);
                    endNode.setFromNode(currentNode);
                    if (!openlist.contains(endNode))
                    {
                        openlist.enqueue(endNode);
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
                while (!currentNode.Equals(startNode))
                {
                    path.Add(currentNode);
                    currentNode = currentNode.getFromNode();
                }
                path.Reverse();
                return path;
            }
        }
    }
}

