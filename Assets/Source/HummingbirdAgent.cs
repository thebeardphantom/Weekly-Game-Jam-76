using System.Collections;
using UnityEngine;

public class HummingbirdAgent : Agent
{
    #region Fields

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _moveSmoothing;

    [SerializeField]
    private Bounds _targetBounds;

    private Vector2 _target;

    private Vector2 _velocity;

    #endregion

    #region Properties

    private bool HasReachedTarget => Vector2.Distance(transform.position, _target) < 0.1f;

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();
        _target = transform.position;
        StartCoroutine(FindNewTarget());
    }

    /// <inheritdoc />
    protected override void Update()
    {
        base.Update();
        if (!IsPlayer)
        {
            transform.position = Vector2.SmoothDamp(transform.position, _target, ref _velocity, _moveSmoothing, _moveSpeed);
        }

        Graphics.flipX = _velocity.x < 0f;
    }

    private IEnumerator FindNewTarget()
    {
        Vector2 GetRandomTarget()
        {
            return new Vector2(
                Random.Range(_targetBounds.min.x, _targetBounds.max.x), 
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
            _target = GetRandomTarget();
        }
    }

    #endregion
}