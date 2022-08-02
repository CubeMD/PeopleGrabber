using System;
using System.Collections.Generic;
using MLDebugTool.Scripts.Agent;
using UnityEngine;

namespace MLDebugTool.Scripts
{
    public class AgentSelector : MonoBehaviour
    {
        public static event Action<AgentSelector, CustomBaseAgent> OnNewAgentSelected;
        
        private int currentIndex = -1;
        private readonly List<CustomBaseAgent> activeAgents = new List<CustomBaseAgent>();

        private void Awake()
        {
            CustomBaseAgent.OnAgentEnabled += HandleAgentEnabled;
            CustomBaseAgent.OnAgentDisabled += HandleAgentDisabled;
            CustomBaseAgent.OnAgentDestroyed += HandleAgentDestroyed;
        }

        private void OnDestroy()
        {
            CustomBaseAgent.OnAgentEnabled -= HandleAgentEnabled;
            CustomBaseAgent.OnAgentDisabled -= HandleAgentDisabled;
            CustomBaseAgent.OnAgentDestroyed -= HandleAgentDestroyed;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                int newIndex = currentIndex - 1;
                SetNewIndex(newIndex < 0 ? activeAgents.Count - 1 : newIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                int newIndex = currentIndex + 1;
                SetNewIndex(newIndex >= activeAgents.Count ? 0 : newIndex);
            }
        }

        private void SetNewIndex(int index)
        {
            if (index >= activeAgents.Count || activeAgents.Count == 0 || index < 0)
            {
                return;
            }

            currentIndex = index;
            OnNewAgentSelected?.Invoke(this, activeAgents[currentIndex]);
        }
        
        #region Handlers

        private void HandleAgentEnabled(CustomBaseAgent agent)
        {
            if (!activeAgents.Contains(agent))
            {
                activeAgents.Add(agent);
                if (currentIndex < 0)
                {
                    SetNewIndex(0);
                }
            }
        }

        private void HandleAgentDestroyed(CustomBaseAgent agent)
        {
            if (activeAgents.Contains(agent))
            {
                activeAgents.Remove(agent);
            }

            int newIndex = activeAgents.Count - 1 > currentIndex ? currentIndex : currentIndex - 1;
            SetNewIndex(newIndex);
        }
        
        private void HandleAgentDisabled(CustomBaseAgent agent)
        {
            if (activeAgents.Contains(agent))
            {
                activeAgents.Remove(agent);
            }
            
            int newIndex = activeAgents.Count - 1 > currentIndex ? currentIndex : currentIndex - 1;
            SetNewIndex(newIndex);
        }

        #endregion
    }
}