using System.Linq;
using UnityEngine;

namespace GamesAI
{
    public class Follower : NPC
    {
        private State state = State.FollowingPlayer;

        public float playerFollowWeight = 1.0f;
        public float separationWeight = 1.0f;
        public float cohesionWeight = 1.0f;

        protected override void FixedUpdate()
        {
            switch (state)
            {
                case State.PursuingEnemy:
                    // TODO: Get nearest enemy and path
                    base.FixedUpdate();
                    break;
                case State.FollowingPlayer:
                    FollowPlayer();
                    break;
            }
        }

        private void FollowPlayer()
        {
            CharacterControl player = GameManager.Instance.Player;
            GameObject playerGameObject = player.gameObject;
            Vector3? target = player.CurrentTarget;
            Vector3 move;
            if (target.HasValue)
            {
                Character.ArriveResult arrive = m_Character.Arrive(target.Value);
                move = arrive.desiredVelocity*playerFollowWeight;
            }
            else
            {
                move = Cohesion(GameManager.Instance.FollowerGameObjects.Concat(new[] { playerGameObject })) * cohesionWeight;
            }
            move += Separation(GameManager.Instance.FollowerGameObjects.Concat(new[] { playerGameObject })) * separationWeight;
            // TODO: Avoid player predicted future position
            m_Character.Move(move / ((target.HasValue ? playerFollowWeight : cohesionWeight) + separationWeight));
        }

        private enum State
        {
            FollowingPlayer,
            PursuingEnemy
        }
    }
}