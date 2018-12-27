using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject _nest;

    private Camera _mainCamera;

    #endregion

    #region Properties

    public static GameController Instance { get; private set; }

    public Agent ActiveAgent { get; private set; }

    public GameObject Nest => _nest;

    public Camera MainCamera
    {
        get
        {
            _mainCamera = _mainCamera == null
                ? Camera.main
                : _mainCamera;
            return _mainCamera;
        }
    }

    #endregion

    #region Methods

    public void SetActiveAgent(Agent agent, EventBusData sender = null)
    {
        var previous = ActiveAgent;
        ActiveAgent = agent;
        EventBus.FireEvent(new ActiveAgentChangedEventBusData(previous)
        {
            Sender = sender
        });
    }

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        EventBus.RegisterListener<AgentDiedEventBusData>(OnAgentDied);
        EventBus.RegisterListener<FaderCompleteEventBusData>(OnFaderComplete);
        StartCoroutine(BeginFirstAgentSearch());
    }

    private void OnFaderComplete(FaderCompleteEventBusData data)
    {
        var diedData = data.ExistsUpstream<AgentDiedEventBusData>();
        if (data.FadingIn && diedData != null)
        {
            var succeeded = diedData.DeadAgent.Succeeded;
            var spawner = Find<IAgentSpawner>(a =>
            {
                return succeeded
                    ? a.Prefab.AgentData.AscensionLevel == diedData.DeadAgent.AscensionLevel + 1
                    : a.Prefab.GetType() == diedData.DeadAgent.AgentType;
            });
            SetActiveAgent(spawner.SpawnOne(), data);
        }
    }

    private T Find<T>(Func<T, bool> predicate) where T : class
    {
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var matching = scene.GetRootGameObjects()
                .SelectMany(s => s.GetComponentsInChildren<T>())
                .FirstOrDefault(predicate);
            if (matching != null)
            {
                return matching;
            }
        }

        return null;
    }

    private void OnAgentDied(AgentDiedEventBusData data)
    {
        if (data.DeadAgent.IsPlayer)
        {
            SetActiveAgent(null, data);
        }
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
                    SetActiveAgent(agent);
                    break;
                }

                yield return null;
            }

            yield return null;
        }
    }

    #endregion
}