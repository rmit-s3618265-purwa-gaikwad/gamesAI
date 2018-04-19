using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamesAI{
	public class GridPlane : MonoBehaviour {

		public LayerMask unwalkableMask;
		public Vector2 gridWorldSize;
		Node[,] grid;
		public float nodeLength;
	    public float collisionRadius;
		int gridXSize, gridYSize;

        public bool debug = false;

		void Awake() {
			gridXSize = Mathf.RoundToInt(gridWorldSize.x/(nodeLength));
			gridYSize = Mathf.RoundToInt(gridWorldSize.y/(nodeLength));
			Debug.Log (string.Format("grid size x = {0}, y = {1}", gridXSize, gridYSize));
			//GenerateGrid();
		}

	    public void GenerateGrid() {
			grid = new Node[gridXSize,gridYSize];
			Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

			for (int i = 0; i < gridXSize; i++) 
			{
				for (int j = 0; j < gridYSize; j++) 
				{
					Vector3 worldLocation = bottomLeft + Vector3.right * (i * nodeLength + nodeLength/2) + Vector3.forward * (j * nodeLength + nodeLength/2);
					bool walkable = !(Physics.CheckSphere(worldLocation,collisionRadius,unwalkableMask));
					grid[i,j] = new Node(i, j, walkable, worldLocation);
				}
			}
		}

		public List<Node> GetNeighbours(Node node) {
			List<Node> neighbours = new List<Node>();

            for (int i = -1; i <= 1; i++) 
			{
				for (int j = -1; j <= 1; j++) 
				{
					if (i == 0 && j == 0)
						continue;
					int X = node.getIndexX() + i;
					int Y = node.getIndexY() + j;
					if (X >= 0 && X < gridXSize && Y >= 0 && Y < gridYSize) 
					{
                        neighbours.Add(grid[X, Y]);
                    }
				}
			}
			return neighbours;
		}
			
		public Node NodeFromWorldPoint(Vector3 worldPosition) 
		{
			float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
			float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);

			int x = Mathf.RoundToInt((gridXSize-1) * percentX);
			int y = Mathf.RoundToInt((gridYSize-1) * percentY);
			return grid[x,y];
		}


        /// <summary>
        /// Untested and likely broken ray-grid traversal for testing if two points can be walked between
        /// Planned to be used to smoothen the results from A* even further, and to replace the linecast being done by followers
        /// </summary>
        /// <param name="start3"></param>
        /// <param name="end3"></param>
        /// <returns></returns>
        public bool CanWalkBetween(Vector3 start3, Vector3 end3)
        {
            Node node = NodeFromWorldPoint(start3);
	        Node endNode = NodeFromWorldPoint(end3);
            var start = new Vector2(start3.x, start3.z);
            var end = new Vector2(end3.x, end3.z);
	        Vector2 diff = end - start;
            Vector2Int sign = diff.Sign();
            var tMax = new Vector2();
            var tDelta = new Vector2();
	        if (sign.x != 0)
	        {
	            tDelta.x = sign.x*nodeLength/diff.x;
	        }
	        else
	        {
	            tDelta.x = float.PositiveInfinity;
	        }
	        if (sign.x > 0)
	        {
	            tMax.x = tDelta.x*(1 - start.x + Mathf.Floor(start.x));
	        }
	        else
	        {
	            tMax.x = tDelta.x*(start.x + Mathf.Floor(start.x));
	        }

            if (sign.y != 0)
            {
                tDelta.y = sign.y * nodeLength / diff.y;
            }
            else
            {
                tDelta.y = float.PositiveInfinity;
            }
            if (sign.y > 0)
            {
                tMax.y = tDelta.y * (1 - start.y + Mathf.Floor(start.y));
            }
            else
            {
                tMax.y = tDelta.y * (start.y + Mathf.Floor(start.y));
            }

	        do
	        {
	            if (!node.getWalkable()) return false;
	            if (tMax.x < tMax.y)
	            {
	                tMax.x += tDelta.x;
	                node = grid[node.indexX + sign.x, node.indexY];
	            }
	            else
	            {
                    tMax.y += tDelta.y;
                    node = grid[node.indexX, node.indexY + sign.y];
                }
	        } while (!node.Equals(endNode));

	        return true;
        }

		public List<Node> path;
		void OnDrawGizmos() {
            if(!debug) return;

            Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

			if (grid != null) {
				//Debug.Log ("Grid != null");
				foreach (Node n in grid) {
					//Debug.Log (string.Format ("Checking node {0},{1}", n.getIndexX (), n.getIndexY ()));
					Gizmos.color = (n.getWalkable ()) ? Color.white : Color.red;
				    if (path != null && path.Contains (n)) {
				        //Debug.Log ("Drawing node");
				        Gizmos.color = Color.black;
				    }
				    Gizmos.DrawCube (n.getGridWorldPos (), Vector3.one * (nodeLength - .1f));
                }
            }
        }
	}
}