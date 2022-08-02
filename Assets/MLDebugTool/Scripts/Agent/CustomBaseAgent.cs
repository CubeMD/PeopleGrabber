using System;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;

namespace MLDebugTool.Scripts.Agent
{
    public abstract class CustomBaseAgent : Unity.MLAgents.Agent
    {
        public static event Action<CustomBaseAgent> OnAgentEnabled;
        public static event Action<CustomBaseAgent> OnAgentDisabled;
        public static event Action<CustomBaseAgent> OnAgentDestroyed;

        public event Action<CustomBaseAgent, ActionBuffers> OnAgentDecisionRequested;
        public event Action<CustomBaseAgent, Dictionary<string, string>> OnAgentObservationsCollected;

        protected readonly Dictionary<string, string> observations = new Dictionary<string, string>();

        protected override void OnEnable()
        {
            base.OnEnable();
            OnAgentEnabled?.Invoke(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnAgentDisabled?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            OnAgentDestroyed?.Invoke(this);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            OnAgentDecisionRequested?.Invoke(this, actions);
        }

        protected void BroadcastObservationsCollected()
        {
            OnAgentObservationsCollected?.Invoke(this, observations);
        }
    }
}