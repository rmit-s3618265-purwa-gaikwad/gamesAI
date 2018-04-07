using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GamesAI
{
    public class Node
    {
		bool walkable;
		Vector3 gridWorldPosition;
        public int indexX, indexY;
        public double costSoFar;
        public double estimatedTotalCost;
        public Node fromNode;

		public Node(int iPosX, int iPosY, bool bWalkable, Vector3 vGridWorldPosition)
        {
            indexX = iPosX;
			indexY = iPosY;
			walkable = bWalkable;
			gridWorldPosition = vGridWorldPosition;
        }
        public void setIndexX(int iIndexX)
        {
			indexX = iIndexX;
        }
		public void setIndexY(int iIndexY)
		{
			indexY = iIndexY;
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
        public int getIndexX()
        {
			return indexX;
        }
		public int getIndexY()
		{
			return indexY;
		}

        public Vector2Int Index => new Vector2Int(indexX, indexY);
		public bool getWalkable()
		{
			return walkable;
		}
		public Vector3 getGridWorldPos()
		{
			return gridWorldPosition;
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
		public int getHeuristicValue(Node targetNode) {
			int dstX = Mathf.Abs(getIndexX() - targetNode.getIndexX());
			int dstY = Mathf.Abs(getIndexY() - targetNode.getIndexY());

			if (dstX > dstY)
				return 14*dstY + 10* (dstX-dstY);
			return 14*dstX + 10 * (dstY-dstX);
		}

        public void reset()
        {
            costSoFar = 0;
            estimatedTotalCost = 0;
            fromNode = null;
        }
    }
}