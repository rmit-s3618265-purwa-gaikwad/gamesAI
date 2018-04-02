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

        public Rigidbody m_Rigidbody { get; private set; }

        public struct ArriveResult
        {
            public Vector3 desiredVelocity;
            public float distance;
            public bool isSlowing;
        }

        void Start()
        {
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
            var res = new ArriveResult();
            Vector3 move = (destination - transform.position).IgnoreY();
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
