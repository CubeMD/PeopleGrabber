using System;
using System.Collections.Generic;
using Characters.Grabber;
using Characters.Walker;
using UnityEngine;

namespace Characters
{
    public class Team
    {
        public event Action<Team, float> OnTeamWalkersSizeChanged; 
        
        public GrabberAgent Agent { get; }
        public Color TeamColor { get; }
        
        private readonly List<NpcWalker> walkers = new List<NpcWalker>();
        public int WalkerCount => walkers.Count;

        public Team(GrabberAgent agent, Color teamColor)
        {
            Agent = agent;
            TeamColor = teamColor;
            walkers.Clear();
        }

        public void AddWalker(NpcWalker walker)
        {
            walkers.Add(walker);
            OnTeamWalkersSizeChanged?.Invoke(this, 1);
        }

        public void RemoveWalker(NpcWalker walker)
        {
            if (walkers.Contains(walker))
            {
                walkers.Remove(walker);
                OnTeamWalkersSizeChanged?.Invoke(this, -1);
            }
        }
    }
}