using UnityEngine;

public class RabbitAgent : Agent
{
    #region Fields

    [SerializeField]
    private Vector2 _jumpForce;

    private Vector2 _velocity;

    private float _nextUpdate;

    #endregion

    #region Properties

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
        }
        else if(Time.time >= _nextUpdate)
        {
            _nextUpdate = Time.time + Random.Range(0.5f, 5f);
            _velocity = new Vector2(Random.value > 0.5f ? _jumpForce.x : -_jumpForce.x, _jumpForce.y);
        }

        if (!Mathf.Approximately(_velocity.x, 0f))
        {
            Graphics.flipX = Mathf.Sign(_velocity.x) < 0f;
        }

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

    #endregion
}