using UnityEngine;
using UnityEngine.AI;

namespace GamesAI
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        public float slowingRadius = 1;
        public float maxSpeed = 100;
        public float maxForce = 0.2f;

        Rigidbody m_Rigidbody;

        void Start()
        {
			Debug.Log ("Starting the game");
			/*List<NavMeshSurface> listSurfaces = NavMeshSurface.activeSurfaces;*/
			NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation ();

			int[] nodeAreaIndices = navMeshTriangulation.indices;
			Vector3[] nodeVectors = navMeshTriangulation.vertices;
			int[] nodeAreas = navMeshTriangulation.areas;

			Debug.Log (string.Format("vertices size = {0}", nodeVectors.Length));
			Debug.Log (string.Format("indices size = {0}", nodeAreaIndices.Length));
			Debug.Log (string.Format("areas size = {0}", navMeshTriangulation.areas.Length));

			foreach(Vector3 v in nodeVectors) {
				Debug.Log (string.Format("Vertices in triangulation is {0}, {1}, {2}", v.x, v.y, v.z));
			}
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }


        public void Move(Vector3 move)
        {
            float distance = move.magnitude;
            // Normalise only if length > 1
            if (distance > 1) move /= distance;
            Vector3 desiredVelocity = move * maxSpeed;
            if (distance < slowingRadius)
            {
                desiredVelocity *= distance/slowingRadius;
            }
            Vector3 steering = desiredVelocity - m_Rigidbody.velocity;
            steering = Truncate(steering, maxForce);
            m_Rigidbody.AddForce(steering, ForceMode.Impulse);
        }

        private static Vector3 Truncate(Vector3 vec, float max)
        {
            float scale = max/vec.magnitude;
            scale = scale < 1 ? scale : 1;
            return vec * scale;
        }
    }
}
