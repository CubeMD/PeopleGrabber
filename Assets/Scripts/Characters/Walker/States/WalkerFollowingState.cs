using System.Collections;
using Characters.Grabber;
using UnityEngine;

namespace Characters.Walker.States
{
    public class WalkerFollowingState : BaseWalkerState
    {
        private const float COLLISION_WAIT = 0.5f;
        
        private readonly GrabberAgent agent;
        private readonly Team currentTeam;

        private Vector3 PlayerPosition => agent.transform.position;
        
        private Coroutine colliderRoutine;

        public WalkerFollowingState(NpcWalker walker, float stoppingDistance, float speed, Team team) : base(walker, stoppingDistance, speed)
        {
            currentTeam = team;
            agent = currentTeam.Agent;
        }

        public override void OnBeginState()
        {
            base.OnBeginState();
            currentTeam.AddWalker(walker);
            SetWalkerColor(currentTeam.TeamColor);
            walker.BroadcastAgentFollowBegun();
            colliderRoutine = walker.StartCoroutine(ColliderRoutineCheck());
        }

        public override void Update()
        {
            base.Update();
            SetNewDestinationPosition(PlayerPosition);
        }

        private IEnumerator ColliderRoutineCheck()
        {
            while (true)
            {
                CheckCollider();
                yield return new WaitForSeconds(COLLISION_WAIT);
            }
        }

        private void CheckCollider()
        {
            foreach (Collider collider in Physics.OverlapSphere(WalkerPosition, 2))
            {
                if (collider.TryGetComponent(out ITeamConvertable convertable))
                {
                    if (convertable.CanBeConverted(currentTeam))
                    {
                        convertable.Convert(currentTeam);
                        return;
                    }
                }
            }
        }

        public bool CanSwitchTeam(Team team)
        {
            return currentTeam != team && currentTeam.WalkerCount < team.WalkerCount;
        }

        public override void OnTerminateState()
        {
            base.OnTerminateState();

            if (walker != null)
            {
                currentTeam?.RemoveWalker(walker);

                if (colliderRoutine != null)
                {
                    walker.StopCoroutine(colliderRoutine);
                }
            }
        }
    }
}