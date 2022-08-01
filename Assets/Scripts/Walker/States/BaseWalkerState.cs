using Grabber;
using Player;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Walker.States
{
    public abstract class BaseWalkerState : State
    {
        protected readonly NpcWalker walker;
        protected readonly NavMeshAgent navMesh;
        protected readonly CharacterVisualization characterVisualization;
        private readonly float stoppingDistance;
        private readonly float speed;
        
        protected Vector3 destination;

        protected Vector3 WalkerPosition => walker == null ? Vector3.zero : walker.transform.position;
        
        public BaseWalkerState(NpcWalker walker, float stoppingDistance, float speed)
        {
            this.walker = walker;
            navMesh = walker.NavMeshAgent;
            characterVisualization = walker.CharacterVisualization;
            this.stoppingDistance = stoppingDistance;
            this.speed = speed;
        }

        public override void OnBeginState()
        {
            base.OnBeginState();
            navMesh.stoppingDistance = stoppingDistance;
            navMesh.speed = speed;
        }

        protected void SetNewDestinationPosition(Vector3 newDestination)
        {
            destination = newDestination;
            navMesh.SetDestination(destination);
        }

        protected void SetWalkerColor(Color color)
        {
            characterVisualization.SetCharacterColor(color);
        }

        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(destination, 2);
            Gizmos.DrawLine(WalkerPosition, destination);
        }
    }
}