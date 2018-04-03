using System.Linq;
using UnityEngine;

namespace GamesAI
{
    public class Follower : NPC
    {
        protected override void FixedUpdate()
        {
            Retaliate();
            FollowPlayer();
        }

        private void Retaliate()
        {
            Helper.ClosestResult target = GameManager.Instance.Enemies.ClosestTo(transform.position, sqrAttackRadius);
            if (target.Character != null)
            {
                Attack(target.Character);
            }
        }

        private void FollowPlayer()
        {
            Character player = GameManager.Instance.Player;
            GameObject playerGameObject = player.gameObject;
            Vector3? target = player.CurrentTarget;
            Vector3 move;
            if (target.HasValue)
            {
                CharacterMotor.ArriveResult arrive = Motor.Arrive(target.Value);
                move = arrive.desiredVelocity;
            }
            else
            {
                move = Cohesion(GameManager.Instance.FollowerGameObjects.Concat(new[] { playerGameObject }));
            }
            move += Separation(GameManager.Instance.FollowerGameObjects.Concat(new[] { playerGameObject }));
            Motor.Move(move);
        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            if (health <= 0)
            {
                GameManager.Instance.Followers.Destroy(this);
            }
            else
            {
                UpdateHealth();
            }
        }
    }
}