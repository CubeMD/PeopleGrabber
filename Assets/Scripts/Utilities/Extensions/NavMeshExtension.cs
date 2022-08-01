using UnityEngine;
using UnityEngine.AI;

namespace Utilities.Extensions
{
    public static class NavMeshExtension 
    {
        private const int MAX_DISTANCE = 10;

        public static bool TryToGetNavMeshPosition(Vector3 origin, out Vector3 foundPosition, float distance = MAX_DISTANCE)
        {
            if (NavMesh.SamplePosition(origin, out NavMeshHit navMeshHit, distance, 1))
            {
                foundPosition = navMeshHit.position;
                return true;
            }

            foundPosition = origin;
            return false;
        }
    }
}