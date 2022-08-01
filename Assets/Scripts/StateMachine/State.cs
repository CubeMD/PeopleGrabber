namespace StateMachine
{
    /// <summary>
    /// Base class for all States.  Use EmptyState to represent a vacant state
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Called when this state terminates itself, either through error handling or completing its process
        /// </summary>
        public event System.Action<State> OnSelfTerminate;
    
        /// <summary>
        /// Called by the StateMachine when this State is active.  Mirrors Unity's identically named function
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called by the StateMachine when this State is active.  Mirrors Unity's identically named function
        /// </summary>
        public virtual void FixedUpdate() { }
        
        /// <summary>
        /// Called by the StateMachine when this State is active.  Mirrors Unity's identically named function
        /// </summary>
        public virtual void LateUpdate() { }

        /// <summary>
        /// Called by the StateMachine when the State begins.  Initialization logic should be put here
        /// </summary>
        public virtual void OnBeginState() { }

        /// <summary>
        /// Called by the StateMachine when this State terminates through any means.  Destruction logic should be put here
        /// </summary>
        public virtual void OnTerminateState() { }
    
        /// <summary>
        /// Should be called to initiate termination of this State (either error handling or completion of the State's process)
        /// </summary>
        public void TerminateState()
        {
            OnTerminateState();

            if (OnSelfTerminate != null)
            {
                OnSelfTerminate(this);
            }
        }

        /// <summary>
        /// Acts the same way Unity's OnDrawGizmos does, but is only called when this state is active
        /// </summary>
        public virtual void OnDrawGizmos() { }
    
        /// <summary>
        /// Acts the same way Unity's OnDrawGizmosSelected does, but is only called when this state is active
        /// </summary>
        public virtual void OnDrawGizmosSelected() { }
    }
}