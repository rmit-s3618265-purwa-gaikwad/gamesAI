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
			GenerateGrid();
		}

	    void GenerateGrid() {
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

		    foreach (Vector2Int pos in new[] { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) })
		    {
                int X = node.getIndexX() + pos.x;
                int Y = node.getIndexY() + pos.y;
                if (X >= 0 && X < gridXSize && Y >= 0 && Y < gridYSize)
                {
                    Node neighbour = grid[X, Y];
                    if (neighbour.getWalkable())
                    {
                        neighbours.Add(grid[X, Y]);
                    }
                }
            }
            /*
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
						neighbours.Add(grid[X,Y]);
					}
				}
			}
            */
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