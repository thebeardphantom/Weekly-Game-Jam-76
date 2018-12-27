using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Fields

    private readonly Stack<Type> _agentTypeStack = new Stack<Type>();

    public Agent ActiveAgent;

    #endregion

    #region Properties

    public static GameController Instance { get; private set; }

    #endregion

    #region Methods

    public void SetActiveAgent(Agent agent)
    {
        if (ActiveAgent != null)
        {
            _agentTypeStack.Push(ActiveAgent.GetType());
        }

        ActiveAgent = agent;
    }

    private void Awake()
    {
        Instance = this;
        EventBus.RegisterListener<AgentDiedEventBusData>(OnAgentDied);
        StartCoroutine(BeginFirstAgentSearch());
    }

    private void OnAgentDied(AgentDiedEventBusData data)
    {
        ActiveAgent = null;
    }

    private IEnumerator BeginFirstAgentSearch()
    {
        while (ActiveAgent == null)
        {
            for (var i = 0; i < Agent.AllAgents.Count; i++)
            {
                var agent = Agent.AllAgents[i];
                if (agent.AgentData.AscensionLevel == 1)
                {
                    ActiveAgent = agent;
                    break;
                }

                yield return null;
            }

            yield return null;
        }
    }

    #endregion
}