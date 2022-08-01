using Player;
using UnityEngine;

namespace Walker.States
{
    public class WalkerFollowingState : BaseWalkerState
    {
        private readonly GrabberAgent agent;
        private readonly AgentTeam team;

        private Vector3 PlayerPosition => agent.transform.position;
        
        private float timer;

        public WalkerFollowingState(NpcWalker walker, float stoppingDistance, float speed, GrabberAgent agentToFollow) : base(walker, stoppingDistance, speed)
        {
            agent = agentToFollow;
            team = agent.AgentTeam;
        }

        public override void OnBeginState()
        {
            base.OnBeginState();
            SetWalkerColor(agent.AgentTeam.teamColor);
            walker.BroadcastAgentFollowBegun();
        }

        public override void Update()
        {
            base.Update();
            SetNewDestinationPosition(PlayerPosition);
            /*CheckCollider();*/
        }

        private void HandleTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITeamConvertable teamConvertable))
            {
                if (teamConvertable.CanBeConverted(team.WalkerCount))
                {
                    teamConvertable.Convert(team);
                }
            }
        }
        
        
    }
}