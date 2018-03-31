using UnityEngine;

namespace GamesAI
{
    public class Follower : NPC
    {
        private Transform player;
        private readonly State state = State.FollowingPlayer;

        protected override void Start()
        {
            base.Start();
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

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
            Character.ArriveResult arrive = m_Character.Arrive(player.position);
            Vector3 move = arrive.desiredVelocity;
            move += Separation("Follower", true);
            // TODO: Avoid player predicted future position
            m_Character.Move(move);
        }

        private enum State
        {
            FollowingPlayer,
            PursuingEnemy
        }
    }
}