using UnityEngine;
using UnityEngine.AI;

namespace GamesAI
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMotor : MonoBehaviour
    {
        public float slowingRadius = 1;
        public float maxSpeed = 100;
        public float maxForce = 0.2f;

        public Rigidbody Rigidbody { get; private set; }

        public struct ArriveResult
        {
            public Vector3 desiredVelocity;
            public float distance;
            public bool isSlowing;
        }

        void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        void OnEnable()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }


        public void Move(Vector3 desiredVelocity)
        {
            Vector3 steering = desiredVelocity - Rigidbody.velocity;
            steering = Truncate(steering, maxForce);
            Rigidbody.velocity = Truncate(Rigidbody.velocity + steering, maxSpeed);
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
