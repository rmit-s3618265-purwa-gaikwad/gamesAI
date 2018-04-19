using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamesAI
{
    public class Enemy : NPC
    {
        public float wanderReferenceDistance = 0.1f;
        [Range(0, 360)] public float maxWanderAngleChange = 1f;
        public float pursueSearchDistance = 10f;
        private float sqrPursueSearchDistance;
        
        private float wanderAngle;

        protected override void Start()
        {
            base.Start();
            sqrPursueSearchDistance = pursueSearchDistance*pursueSearchDistance;
            wanderAngle = Random.Range(-180f, 180f);
        }

        protected override void FixedUpdate()
        {
            Helper.ClosestResult target =
                GameManager.Instance.Followers.Concat(new[] { GameManager.Instance.Player })
                    .ClosestTo(transform.position, sqrPursueSearchDistance);
            if (target.Character == null)
            {
                WanderGrouped();
            }
            else if (target.SqrDistance <= sqrAttackRadius)
            {
                Attack(target.Character);
            }
            else
            {
                Pursue(target);
            }
        }

        private void WanderGrouped()
        {
            Vector3 move = Wander();
            move += Cohesion(GameManager.Instance.EnemyGameObjects);
            move += Separation(GameManager.Instance.EnemyGameObjects);
            move += Alignment(GameManager.Instance.EnemyGameObjects);
            Motor.Move(move);
        }

        private Vector3 Wander()
        {
            Vector3 circleCenter = Motor.Rigidbody.velocity.normalized * wanderReferenceDistance;
            Vector3 displacement = Quaternion.Euler(0, wanderAngle, 0) * Vector3.forward;
            wanderAngle += Random.Range(-1f, 1f)*maxWanderAngleChange;
            return circleCenter + displacement;
        }

        private void Pursue(Helper.ClosestResult target)
        {
            Vector3 targetPos = target.Character.transform.position;
            float t = Mathf.Sqrt(target.SqrDistance)/target.Character.Motor.maxSpeed;
            targetPos += t*target.Character.Motor.Rigidbody.velocity;
            Vector3 move = Motor.Arrive(targetPos).desiredVelocity;
            move += Separation(GameManager.Instance.EnemyGameObjects);
            move += Cohesion(GameManager.Instance.EnemyGameObjects);
            Motor.Move(move);
        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            if (health <= 0)
            {
                Vector3 pos = transform.position;
                Vector3 velocity = Motor.Rigidbody.velocity;
                GameManager.Instance.Enemies.Destroy(this);
                Follower follower = GameManager.Instance.Followers.Instantiate();
                follower.transform.position = pos;
                follower.Motor.Rigidbody.velocity = velocity;
            }
            else
            {
                UpdateHealth();
            }
        }
    }
}
