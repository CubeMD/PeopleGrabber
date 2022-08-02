using System;
using MLDebugTool.Scripts.Agent;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace Characters.Grabber
{
    [RequireComponent(typeof(Rigidbody),  typeof(CharacterVisualization))]
    public class GrabberAgent : CustomBaseAgent, ITeamConvertable
    {
        /// <summary>
        /// First agent is agent being killed and 2nd agent is the agent that killed it
        /// </summary>
        public static event Action<GrabberAgent, Team> OnAnyAgentDeath;
        
        [SerializeField]
        private Rigidbody body;
        [SerializeField]
        private CharacterVisualization characterVisualization;

        [Header("Movement")]
        [SerializeField]
        private float movementSpeed = 50;
        [SerializeField]
        private float rotationSpeed = 25;
       
        private Team currentTeam;
        private float verticalInput;
        private float horizontalInput;
        private Vector3 velocity;

        private void OnValidate()
        {
            if (body == null)
            {
                body = GetComponent<Rigidbody>();
            }

            if (characterVisualization == null)
            {
                characterVisualization = GetComponent<CharacterVisualization>();
            }
        }

        public void SetTeam(Team team)
        {
            currentTeam = team;
            currentTeam.OnTeamWalkersSizeChanged += HandleTeamWalkersSizeChanged;
            characterVisualization.SetCharacterColor(team.TeamColor);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (currentTeam != null)
            {
                currentTeam.OnTeamWalkersSizeChanged -= HandleTeamWalkersSizeChanged;
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            // collides with camera inputs 
            // only use WASD pls
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            Vector3 dirNormalized = new Vector3(horizontalInput, 0, verticalInput);
            Vector3 movementVector = dirNormalized * movementSpeed;
            body.velocity = Vector3.SmoothDamp(body.velocity, movementVector, ref velocity, Time.deltaTime);

            if (dirNormalized != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dirNormalized);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITeamConvertable convertable))
            {
                if (convertable.CanBeConverted(currentTeam))
                {
                    convertable.Convert(currentTeam);
                }
            }
        }
        
        private void HandleTeamWalkersSizeChanged(Team team, float change)
        {
            AddReward(change);
        }

        public bool CanBeConverted(Team team)
        {
            return team != this.currentTeam && this.currentTeam.WalkerCount < 1;
        }

        public void Convert(Team team)
        {
            OnAnyAgentDeath?.Invoke(this, team);
        }
    }
}
