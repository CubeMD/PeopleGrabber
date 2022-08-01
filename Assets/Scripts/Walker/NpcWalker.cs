using System;
using Grabber;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Walker.States;

namespace Walker
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

        private AgentTeam agentTeam;
        
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

        private void OnDisable()
        {
            agentTeam = null;
        }

        public void ForceTeam(AgentTeam team)
        {
            agentTeam = team;
            stateMachine.SetActiveState(new WalkerFollowingState(this, followingStoppingDistance, followingSpeed, agentTeam.agent));
        }
        
        public void BroadcastAgentFollowBegun()
        {
            OnAnyWalkerBegunAgentFollow?.Invoke(this);
        }

        public bool CanBeConverted(int walkersAmount)
        {
            return agentTeam == null || agentTeam.WalkerCount < walkersAmount;
        }

        public void Convert(AgentTeam team)
        {
            ForceTeam(team);
        }
    }
}