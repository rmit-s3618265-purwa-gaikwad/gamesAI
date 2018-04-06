using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GamesAI
{
    [RequireComponent(typeof(CharacterMotor))]
    public class Character : MonoBehaviour
    {
        public float attackCooldown = 0.1f;

        public float attackRadius = 0.6f;
        public float attackStrength = 1f;
        protected float health = 10f;

        public float maxHealth = 10f;

        private float previousAttack;
        protected float sqrAttackRadius;
        protected LastQueue<Vector3> targets = new LastQueue<Vector3>(); // List of targets to head towards
        public CharacterMotor Motor { get; private set; } // A reference to the Character on the object

        public Vector3? CurrentTarget => targets.Count > 0 ? new Vector3?(targets.Peek()) : null;

        protected virtual void Start()
        {
            sqrAttackRadius = attackRadius*attackRadius;
        }

        protected virtual void OnEnable()
        {
            health = maxHealth;
            
            // Done in OnEnable() because this is called earlier than Start(), which seems to be called to late for use after Instantiate()
            Motor = GetComponent<CharacterMotor>();
        }

        // Fixed update is called in sync with physics
        /*protected virtual void FixedUpdate()
        {
            if (targets.Count > 0)
            {
                Vector3 target = targets.Peek();
				PathFinding pathfinding = GetComponent<PathFinding> ();
				List<Node> listNodes = pathfinding.process((transform.position).IgnoreY(), target.IgnoreY());
				Vector3 intermediateNode;
				while(listNodes.Count > 0)
				{
					intermediateNode = listNodes[0].getGridWorldPos();
					CharacterMotor.ArriveResult arrive = Motor.Arrive(intermediateNode);
					listNodes.RemoveAt(0);
					Motor.Move(arrive.desiredVelocity);
				}
				targets.Dequeue();
            }
        }*/
		
		protected virtual void FixedUpdate()
        {
            if (targets.Count > 0)
            {
                Vector3 target = targets.Peek();
                CharacterMotor.ArriveResult arrive = Motor.Arrive(target);
                if (arrive.isSlowing)
                {
                    if (targets.Count == 1 && arrive.distance < 0.25)
                    {
                        targets.Dequeue();
                    }
                    else if (targets.Count > 1 && arrive.distance < 1)
                    {
                        targets.Dequeue();
                    }
                }
                Motor.Move(arrive.desiredVelocity);
            }
        }


        public virtual void Damage(float damage)
        {
            health -= damage;
        }

        protected void Attack(Character target)
        {
            float time = Time.time;
            if (time - previousAttack < attackCooldown) return;

            target.Damage(attackStrength);
            previousAttack = time;
        }
    }
}