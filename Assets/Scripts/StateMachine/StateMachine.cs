using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// GameObject Component that manages states
    /// States can be queued to be executed in order
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        /// <summary>
        /// Called when a state resolves, either from an early termination or finishing its process
        /// Is not called when an active state is terminated by replacing it with a new state
        /// </summary>
        public event System.Action<State> OnActiveStateTerminatedEarly;
        public event System.Action<State> OnNewStateBegun;

        public event System.Action<StateMachine> OnStateMachineUpdate;
        public event System.Action<StateMachine> OnStateMachineFixedUpdate;
        public event System.Action<StateMachine> OnStateMachineLateUpdate;
        
        private State activeState;

        private void Awake()
        {
            if(activeState == null)
            {
                SetActiveState(new EmptyState());
            }
        }

        /// <summary>
        /// Gets the currently active State for this StateMachine
        /// </summary>
        /// <returns></returns>
        public State GetActiveState()
        {
            return activeState;
        }
        
        /// <summary>
        /// Terminates the active State, sets the given State to active immediately and clears the State queue.
        /// </summary>
        /// <param name="state">State to set active</param>
        public virtual void SetActiveState(State state)
        {
            TransitionState(state, true);
        }

        /// <summary>
        /// Terminates the current active State and makes the given State active
        /// </summary>
        /// <param name="state">State to transition to</param>
        /// <param name="terminateActiveState">If true, calls OnTerminateState on the active State before the transition</param>
        private void TransitionState(State state, bool terminateActiveState)
        {
            if (activeState != null)
            {
                if (terminateActiveState)
                {
                    activeState.OnTerminateState();
                }

                activeState.OnSelfTerminate -= HandleActiveStateSelfTerminate;
                activeState = null;
            }

            if (state != null)
            {
                activeState = state;
                activeState.OnSelfTerminate += HandleActiveStateSelfTerminate;
                activeState.OnBeginState();
                OnNewStateBegun?.Invoke(state);
            }
        }
        
        protected virtual void Update()
        {
            activeState?.Update();
            OnStateMachineUpdate?.Invoke(this);
        }

        protected virtual void FixedUpdate()
        {
            activeState?.FixedUpdate();
            OnStateMachineFixedUpdate?.Invoke(this);
        }

        protected virtual void LateUpdate()
        {
            activeState?.LateUpdate();
            OnStateMachineLateUpdate?.Invoke(this);
        }

        private void HandleActiveStateSelfTerminate(State terminatedState)
        {
            TransitionState(new EmptyState(), false);
            
            if (OnActiveStateTerminatedEarly != null)
            {
                OnActiveStateTerminatedEarly(terminatedState);
            }
        }

        private void OnDrawGizmosSelected()
        {
            activeState?.OnDrawGizmosSelected();
        }
    }
}