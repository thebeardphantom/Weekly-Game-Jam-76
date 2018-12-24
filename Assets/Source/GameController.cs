using System;
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
    }

    #endregion
}