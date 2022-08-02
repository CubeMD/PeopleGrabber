using System;
using UnityEngine;

namespace MLDebugTool.Scripts.Camera
{
    public class OffsetBaseAgentCameraFollow : BaseAgentCameraFollow
    {
        public enum OffsetType
        {
            World,
            Local
        }

        [SerializeField]
        private bool trackOnUpdate;

        [SerializeField]
        private OffsetType offsetType;
        [SerializeField]
        private Vector3 cameraOffset;

        [SerializeField]
        private bool overrideXAxis;
        [SerializeField]
        private bool overrideYAxis;
        [SerializeField]
        private bool overrideZAxis;
        [SerializeField]
        private Vector3 overrideAxis = Vector3.zero;

        private void Update()
        {
            if (trackOnUpdate)
            {
                UpdatePosition();
            }
        }

        protected override void UpdatePosition()
        {
            Vector3 newPosition = offsetType == OffsetType.Local ? 
                AgentPosition + GetReferenceOrientation(Vector3.up) * cameraOffset :
                AgentPosition + cameraOffset;

            if (overrideXAxis)
            {
                newPosition.x = overrideAxis.x;
            }
            
            if (overrideYAxis)
            {
                newPosition.y = overrideAxis.y;
            }
            
            if (overrideZAxis)
            {
                newPosition.z = overrideAxis.z;
            }

            transform.position = newPosition;
        }
    }
}