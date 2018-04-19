using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamesAI
{
    public class Follower : NPC
    {
        public float allowedCohesionVariance = 1f;
        private float sqrAllowedCohesionVariance;

        private Vector3? playerPreviousTarget;

        protected override void Start()
        {
            base.Start();
            sqrAllowedCohesionVariance = allowedCohesionVariance;
        }

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
                if (target == playerPreviousTarget && targets.Count > 0)
                {
                    FollowPath();
                    return;
                }
                playerPreviousTarget = target;
                if (Physics.Linecast(transform.position, target.Value, GameManager.Instance.GridPlane.unwalkableMask))
                {
                    List<Node> nodes = GameManager.Instance.PathFinding.process(transform.position, target.Value);
                    if (nodes == null)
                    {
                        move = Motor.Arrive(target.Value).desiredVelocity;
                    }
                    else
                    {
                        targets = new Queue<Vector3>(nodes.Select(node => node.getGridWorldPos()));
                        FollowPath();
                        return;
                    }
                }
                else
                {
                    targets.Clear();
                    move = Motor.Arrive(target.Value).desiredVelocity;
                }
            }
            else
            {
                Vector3 cohesionPos =
                    CohesionPosition(GameManager.Instance.FollowerGameObjects.Concat(new[] {playerGameObject}));
                if (((player.transform.position - cohesionPos).sqrMagnitude > sqrAllowedCohesionVariance) ||
                    Physics.Linecast(transform.position, player.transform.position,
                        GameManager.Instance.GridPlane.unwalkableMask))
                {
                    if (player.transform.position == playerPreviousTarget && targets.Count > 0)
                    {
                        FollowPath();
                        return;
                    }
                    playerPreviousTarget = player.transform.position;
                    List<Node> nodes = GameManager.Instance.PathFinding.process(transform.position,
                        player.transform.position);
                    if (nodes == null)
                    {
                        move = Motor.Arrive(player.transform.position).desiredVelocity;
                    }
                    else
                    {
                        targets = new Queue<Vector3>(nodes.Select(node => node.getGridWorldPos()));
                        FollowPath();
                        return;
                    }
                }
                else
                {
                    move = Cohesion(cohesionPos);
                    targets.Clear();
                }
            }
            move += Separation(GameManager.Instance.FollowerGameObjects.Concat(new[] {playerGameObject}));
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