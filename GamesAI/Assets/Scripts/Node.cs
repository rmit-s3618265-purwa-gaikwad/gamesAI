using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GamesAI
{
    public struct connection
    {
        public double cost;
        public Node toNode;

        public connection(double costValue, Node toNodeValue)
        {
            cost = costValue;
            toNode = toNodeValue;
        }
    }
    public class Node
    {

        public int nodeId;
        public List<connection> connections;
        public double costSoFar;
        public double estimatedTotalCost;
        public Node fromNode;

        public Node(int nodeIdValue, double costSoFarValue, double estimatedTotalCosValue)
        {
            nodeId = nodeIdValue;
            connections = new List<connection>();
            costSoFar = costSoFarValue;
            estimatedTotalCost = estimatedTotalCosValue;
        }
        public void setNodeId(int nodeIdValue)
        {
            nodeId = nodeIdValue;
        }
        public void setConnection(double costValue, Node toNodeValue)
        {
            connections.Add(new connection(costValue, toNodeValue));
        }
        public void setFromNode(Node fromNodeValue)
        {
            fromNode = fromNodeValue;
        }
        public void setCostSoFar(double costSoFarValue)
        {
            costSoFar = costSoFarValue;
        }
        public void setEstimatedTotalCost(double estimatedTotalCosValue)
        {
            estimatedTotalCost = estimatedTotalCosValue;
        }
        public int getNodeId()
        {
            return nodeId;
        }
        public List<connection> getConnection()
        {
            return connections;
        }
        public Node getFromNode()
        {
            return fromNode;
        }
        public double getCostSoFar()
        {
            return costSoFar;
        }
        public double getEstimatedTotalCost()
        {
            return estimatedTotalCost;
        }
    }
}

