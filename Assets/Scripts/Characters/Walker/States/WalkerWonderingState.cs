using UnityEngine;
using Utilities.Extensions;

namespace Characters.Walker.States
{
    public class WalkerWonderingState : BaseWalkerState
    {
        private const float MIN_RANGE = 10;
        private const float MAX_RANGE = 40;
        private const float THRESHOLD_DISTANCE = 0.5f;
        
        public WalkerWonderingState(NpcWalker walker, float stoppingDistance, float speed) : base(walker, stoppingDistance, speed) { }

        public override void OnBeginState()
        {
            base.OnBeginState();
            SetWalkerColor(Color.grey);
            SetNewDestinationPosition(GetRandomPositionAround());
        }

        public override void Update()
        {
            base.Update();

            if (IsCloseToDestination())
            {
                SetNewDestinationPosition(GetRandomPositionAround());
            }
        }
        
        private Vector3 GetRandomPositionAround()
        {
            Vector2 randomPos = Random.insideUnitCircle * Random.Range(MIN_RANGE, MAX_RANGE);
            Vector3 randomPositionAroundWalker = WalkerPosition + new Vector3(randomPos.x, 0.1f, randomPos.y);

            if (NavMeshExtension.TryToGetNavMeshPosition(randomPositionAroundWalker, out Vector3 foundPosition, 20))
            {
                return foundPosition;
            }
         
            return WalkerPosition;
        }

        private bool IsCloseToDestination()
        {
            return Vector3.Distance(WalkerPosition, destination) <= THRESHOLD_DISTANCE;
        }
    }
}