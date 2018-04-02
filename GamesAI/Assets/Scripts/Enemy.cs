using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamesAI
{
    public class Enemy : NPC
    {
        public float wanderReferenceDistance = 0.1f;
        [Range(0, 360)] public float maxWanderAngleChange = 1f;

        private State state = State.Wander;
        private float wanderAngle = 0.0f;

        protected override void FixedUpdate()
        {
            switch (state)
            {
                case State.Wander:
                    WanderGrouped();
                    break;
                case State.PursueFollowerOrPlayer:
                    break;
            }
        }

        private void WanderGrouped()
        {
            Vector3 move = Wander();
            move += Cohesion(GameManager.Instance.EnemyGameObjects);
            move += Separation(GameManager.Instance.EnemyGameObjects);
            move += Alignment(GameManager.Instance.EnemyGameObjects);
            m_Character.Move(move);
        }

        private Vector3 Wander()
        {
            Vector3 circleCenter = m_Character.m_Rigidbody.velocity.normalized * wanderReferenceDistance;
            Vector3 displacement = Quaternion.Euler(0, wanderAngle, 0) * Vector3.forward;
            wanderAngle += Random.Range(-1f, 1f)*maxWanderAngleChange;
            return circleCenter + displacement;
        }

        private enum State
        {
            Wander,
            PursueFollowerOrPlayer
        }
    }
}
