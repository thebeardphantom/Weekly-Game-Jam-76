using UnityEngine;

public class RabbitAgent : Agent<RabbitAgent>
{
    #region Fields

    [SerializeField]
    private Vector2 _jumpForce;

    [SerializeField]
    private Transform _wormCollectPoint;

    [SerializeField]
    private float _wormCollectDst = 0.1f;

    private Vector2 _velocity;

    private float _nextUpdate;

    private bool _facingRight = true;

    #endregion

    #region Properties

    public int CollectedWorms { get; private set; }

    private bool IsGrounded => _velocity.y <= 0f && Mathf.Approximately(transform.position.y, 0f);

    #endregion

    #region Methods

    protected override void Update()
    {
        base.Update();
        if (IsPlayer)
        {
            if (InputManager.Instance.AnyDown("JUMP") && IsGrounded)
            {
                if (InputManager.Instance.AnyDown("MOVE_RIGHT"))
                {
                    _velocity = _jumpForce;
                }
                else if (InputManager.Instance.AnyDown("MOVE_LEFT"))
                {
                    _velocity = new Vector2(-_jumpForce.x, _jumpForce.y);
                }
            }

            if (_wormCollectPoint.childCount == 0)
            {
                WormAgent closestWorm = null;
                var minDist = float.MaxValue;
                for (var i = 0; i < WormAgent.All.Count; i++)
                {
                    var worm = WormAgent.All[i];
                    var dst = Vector2.Distance(_wormCollectPoint.position, worm.transform.position);
                    if (dst < minDist && dst < _wormCollectDst)
                    {
                        minDist = dst;
                        closestWorm = worm;
                    }
                }

                if (closestWorm != null)
                {
                    closestWorm.enabled = false;
                    closestWorm.transform.SetParent(_wormCollectPoint);
                    closestWorm.transform.localPosition = Vector2.zero;
                }
            }
        }
        else if (Time.time >= _nextUpdate)
        {
            _nextUpdate = Time.time + Random.Range(0.5f, 5f);
            _velocity = new Vector2(Random.value > 0.5f ? _jumpForce.x : -_jumpForce.x, _jumpForce.y);
        }

        if (!Mathf.Approximately(_velocity.x, 0f))
        {
            _facingRight = Mathf.Sign(_velocity.x) > 0f;
        }

        var scale = transform.localScale;
        var facingSign = _facingRight ? 1 : -1;
        if ((int) Mathf.Sign(scale.x) != facingSign)
        {
            scale.x *= -1f;
        }

        transform.localScale = scale;

        var position = transform.position;

        position += (Vector3) _velocity * Time.deltaTime;
        position.y = Mathf.Max(0f, position.y);
        transform.position = position;
        _velocity.y -= 9.87f * Time.deltaTime;
        if (IsGrounded)
        {
            _velocity = Vector2.zero;
        }
    }

    /// <inheritdoc />
    public override void Kill(Agent source)
    {
        Succeeded = CollectedWorms >= 3;
        base.Kill(source);
    }

    #endregion

    public void OnNestCollision()
    {
        if(_wormCollectPoint.childCount > 0)
        {
            _wormCollectPoint.GetComponentInChildren<WormAgent>().Kill(this);
            CollectedWorms++;
        }
    }
}