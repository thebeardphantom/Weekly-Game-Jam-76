using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HummingbirdAgent : Agent<HummingbirdAgent>
{
    #region Fields

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _moveSmoothing;

    [SerializeField]
    private Bounds _targetBounds;

    private Vector2? _target;

    private Vector2 _velocity;

    #endregion

    #region Properties

    private bool HasReachedTarget => !_target.HasValue || Vector2.Distance(transform.position, _target.Value) < 0.1f;

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();
        EventBus.RegisterListener<ActiveAgentChangedEventBusData>(OnActiveAgentChanged);
        _target = transform.position;
        StartCoroutine(FindNewTarget());
    }

    private void OnActiveAgentChanged(ActiveAgentChangedEventBusData data)
    {
        if (GameController.Instance.ActiveAgent == this)
        {
            _target = null;
        }
    }

    /// <inheritdoc />
    protected override void Update()
    {
        base.Update();
        if (IsPlayer)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                world.z = 0f;
                if (world.y > 0f)
                {
                    _target = world;
                }
            }
        }
        
        if(_target.HasValue)
        {
            transform.position = Vector2.SmoothDamp(transform.position,
                _target.Value,
                ref _velocity,
                _moveSmoothing,
                _moveSpeed);
        }

        Graphics.flipX = _velocity.x < 0f;
    }

    private IEnumerator FindNewTarget()
    {
        Vector2 GetRandomTarget()
        {
            return new Vector2(Random.Range(_targetBounds.min.x, _targetBounds.max.x),
                Random.Range(_targetBounds.min.y, _targetBounds.max.y));
        }

        yield return new WaitForSeconds(Random.Range(0f, 3f));
        _target = GetRandomTarget();
        while (true)
        {
            while (!HasReachedTarget)
            {
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(1f, 5f));
            if (IsPlayer)
            {
                yield break;
            }
            _target = GetRandomTarget();
        }
    }

    #endregion
}