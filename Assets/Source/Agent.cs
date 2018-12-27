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

    protected virtual void Awake()
    {
        SpawnTime = Time.time;
        StartCoroutine(Kill());
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

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(_data.Lifetime);
        EventBus.FireEvent(new AgentDiedEventBusData(this));
        Destroy(gameObject);
    }

    #endregion
}