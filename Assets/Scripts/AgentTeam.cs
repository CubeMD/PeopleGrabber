using System.Collections.Generic;
using Player;
using UnityEngine;
using Walker;

public class AgentTeam
{
    public readonly Color teamColor;
    public readonly GrabberAgent agent;
    public readonly List<NpcWalker> walkers = new List<NpcWalker>();

    public int WalkerCount => walkers.Count;

    public AgentTeam(GrabberAgent agent, Color teamColor)
    {
        this.agent = agent;
        this.teamColor = teamColor;
        walkers.Clear();
    }

    public void AddWalker(NpcWalker walker)
    {
        walkers.Add(walker);
    }

    public void RemoveWalker(NpcWalker walker)
    {
        if (walkers.Contains(walker))
        {
            walkers.Remove(walker);
        }
    }
}