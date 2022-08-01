using System;
using Characters.Walker.States;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Walker
{
    [RequireComponent(typeof(StateMachine.StateMachine), typeof(NavMeshAgent), typeof(CharacterVisualization))]
    public class NpcWalker : MonoBehaviour, ITeamConvertable
    {
        public static event Action<NpcWalker> OnAnyWalkerBegunAgentFollow;
        
        [SerializeField]
        private StateMachine.StateMachine stateMachine;
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        public NavMeshAgent NavMeshAgent => navMeshAgent;

        [SerializeField]
        private CharacterVisualization characterVisualization;
        public CharacterVisualization CharacterVisualization => characterVisualization;

        [Header("Wondering parameters")]
        [SerializeField]
        private float wonderingStoppingDistance = 2f;
        [SerializeField]
        private float wonderingSpeed = 5f;

        [Header("Following parameters")]
        [SerializeField]
        private float followingStoppingDistance = 3f;
        [SerializeField]
        private float followingSpeed = 8f;

        private void OnValidate()
        {
            if (stateMachine == null)
            {
                stateMachine = GetComponent<StateMachine.StateMachine>();
            }

            if (navMeshAgent == null)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
            }
            
            if (characterVisualization == null)
            {
                characterVisualization = GetComponent<CharacterVisualization>();
            }
        }

        private void OnEnable()
        {
            stateMachine.SetActiveState(new WalkerWonderingState(this, wonderingStoppingDistance, wonderingSpeed));
        }

        public void ForceTeam(Team team)
        {
            stateMachine.SetActiveState(new WalkerFollowingState(this, followingStoppingDistance, followingSpeed, team));
        }
        
        public void BroadcastAgentFollowBegun()
        {
            OnAnyWalkerBegunAgentFollow?.Invoke(this);
        }

        public bool CanBeConverted(Team team)
        {
            State activeState = stateMachine.GetActiveState();
            
            if (activeState is WalkerFollowingState followingState)
            {
                return followingState.CanSwitchTeam(team);
            }

            return true;
        }

        public void Convert(Team team)
        {
            ForceTeam(team);
        }
    }
}