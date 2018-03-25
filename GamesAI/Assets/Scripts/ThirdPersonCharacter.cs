using NUnit.Framework;
using UnityEngine;

namespace GamesAI
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThirdPersonCharacter : MonoBehaviour
    {
        public float slowingRadius = 1;
        public float maxSpeed = 100;
        public float maxForce = 0.2f;

        Rigidbody m_Rigidbody;

        void Start()
        {
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
