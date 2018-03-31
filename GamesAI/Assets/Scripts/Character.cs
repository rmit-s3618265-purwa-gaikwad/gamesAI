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

        public struct ArriveResult
        {
            public Vector3 desiredVelocity;
            public float distance;
            public bool isSlowing;
        }

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


        public void Move(Vector3 desiredVelocity)
        {
            Vector3 steering = desiredVelocity - m_Rigidbody.velocity;
            steering = Truncate(steering, maxForce);
            m_Rigidbody.AddForce(steering, ForceMode.Impulse);
        }

        public ArriveResult Arrive(Vector3 destination)
        {
            ArriveResult res = new ArriveResult();
            Vector3 move = destination - transform.position;
            res.distance = move.magnitude;
            // Normalise only if length > 1
            if (res.distance > 1) move /= res.distance;
            res.desiredVelocity = move * maxSpeed;
            if (res.distance < slowingRadius)
            {
                res.desiredVelocity *= res.distance / slowingRadius;
                res.isSlowing = true;
            }
            else
            {
                res.isSlowing = false;
            }
            return res;
        }

        private static Vector3 Truncate(Vector3 vec, float max)
        {
            float scale = max/vec.magnitude;
            scale = scale < 1 ? scale : 1;
            return vec * scale;
        }
    }
}
