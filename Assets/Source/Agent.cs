using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Agent : MonoBehaviour
{
    #region Fields

    public static readonly List<Agent> AllAgents = new List<Agent>();

    [SerializeField]
    [FormerlySerializedAs("_graphics")]
    protected SpriteRenderer Graphics;

    protected bool Succeeded;

    [SerializeField]
    private Transform _cameraTrackTarget;

    [SerializeField]
    private AgentData _data;

    private string _ascensionFormatString;

    #endregion

    #region Properties

    protected float SpawnTime { get; private set; }

    public bool IsPlayer => GameController.Instance.ActiveAgent == this;

    public Transform CameraTrackTarget => _cameraTrackTarget;

    public AgentData AgentData => _data;

    public float LifetimePosition => Mathf.Clamp01((Time.time - SpawnTime) / _data.Lifetime);

    #endregion

    #region Methods

    protected static Vector2 GetDirectionalInput()
    {
        var direction = new Vector2();
        if (InputManager.Instance.AnyDown("MOVE_UP"))
        {
            direction.y = 1f;
        }
        else if (InputManager.Instance.AnyDown("MOVE_DOWN"))
        {
            direction.y = -1f;
        }
        else if (InputManager.Instance.AnyDown("MOVE_RIGHT"))
        {
            direction.x = 1f;
        }
        else if (InputManager.Instance.AnyDown("MOVE_LEFT"))
        {
            direction.x = -1f;
        }

        return direction;
    }

    public virtual void Kill(Agent source)
    {
        var deadAgentData = new DeadAgentData(
            GetInstanceID(), 
            GetType(), 
            Succeeded, 
            AgentData.AscensionLevel, 
            IsPlayer);
        EventBus.FireEvent(new AgentDiedEventBusData(deadAgentData));
        Destroy(gameObject);
    }

    protected virtual void Awake()
    {
        SpawnTime = Time.time;
        StartCoroutine(LifetimeRoutine());
    }

    protected virtual void OnEnable()
    {
        AllAgents.Add(this);
    }

    protected virtual void OnDisable()
    {
        AllAgents.Remove(this);
    }

    protected virtual void Update()
    {
        transform.localScale = Vector3.one * _data.LifetimeScaleCurve.Evaluate(LifetimePosition);
        Graphics.color = _data.LifetimeTint.Evaluate(LifetimePosition);
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(_data.Lifetime);
        Kill(this);
    }

    #endregion
}

public abstract class Agent<T> : Agent where T : Agent<T>
{
    #region Fields

    public static readonly List<T> All = new List<T>();

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void OnEnable()
    {
        base.OnEnable();
        All.Add((T) this);
    }

    /// <inheritdoc />
    protected override void OnDisable()
    {
        base.OnDisable();
        All.Remove((T) this);
    }

    #endregion
}