using System.Linq;
using UnityEngine;

public class WormAgent : Agent
{
    #region Fields

    [SerializeField]
    private Sprite _altSprite;

    [SerializeField]
    private float _offset;

    [SerializeField]
    private int _segmentCount = 6;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private ParticleSystem _digParticles;

    private SpriteRenderer[] _segments;

    private Vector3[] _lastPositions;

    private Vector2 _lastDir;

    private float _nextUpdate;

    private Vector2 _segmentExtents;

    private float _updateSpeed;

    #endregion

    #region Methods

    private static bool Approx(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
    }

    private static Vector2 CalcExtents(Vector2[] spriteVertices)
    {
        var min = spriteVertices[0];
        var max = spriteVertices[0];
        foreach (var vert in spriteVertices)
        {
            min = Vector2.Min(min, vert);
            max = Vector2.Max(max, vert);
        }

        return new Vector2(max.x - min.x, max.y - min.y) / 2f;
    }

    protected override void Awake()
    {
        base.Awake();
        _updateSpeed = Random.Range(0.05f, 0.1f);

        var position = transform.position;
        var roundFactor = 1f / _moveSpeed;
        position.x = Mathf.Round(position.x * roundFactor) / roundFactor;
        position.y = Mathf.Round(position.y * roundFactor) / roundFactor;
        transform.position = position;

        _segments = new SpriteRenderer[_segmentCount];
        _segments[0] = Graphics;
        _lastPositions = new Vector3[_segmentCount];
        _lastPositions[0] = _segments[0].transform.position;
        for (var i = 1; i < _segments.Length; i++)
        {
            _segments[i] = Instantiate(Graphics);
            _segments[i].sprite = i % 2 == 0 ? _segments[0].sprite : _altSprite;
            _segments[i].transform.parent = transform;
            _segments[i].transform.position = transform.position + Vector3.right * _offset * i;
            _lastPositions[i] = _segments[i].transform.position;
        }

        _segmentExtents = CalcExtents(_segments[0].sprite.vertices);
    }

    protected override void Update()
    {
        base.Update();
        var direction = Vector2.zero;
        if (IsPlayer)
        {
            direction = GetDirectionalInput();
        }
        else if (Time.time >= _nextUpdate)
        {
            if (Mathf.Abs(_lastDir.x) > 0f || Mathf.Approximately(_lastDir.sqrMagnitude, 0f))
            {
                direction.y = Random.value > Random.value
                    ? 1f
                    : -1f;
            }
            else if (Mathf.Abs(_lastDir.y) > 0f)
            {
                direction.x = Random.value > Random.value
                    ? 1f
                    : -1f;
            }

            _nextUpdate = Time.time + _updateSpeed;
        }

        MoveInDirection(direction);
    }

    private void MoveInDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0f)
        {
            var isAtMaxHeight = transform.position.y >= 0f;
            if (isAtMaxHeight && direction.y > 0f)
            {
                return;
            }

            var newPosition = transform.position + (Vector3) direction * _moveSpeed;
            if (_segments.Any(p => Approx(newPosition, p.transform.position)))
            {
                return;
            }

            var sameDirX = Mathf.Approximately(Mathf.Abs(_lastDir.x), Mathf.Abs(direction.x));
            var sameDirY = Mathf.Approximately(Mathf.Abs(_lastDir.y), Mathf.Abs(direction.y));
            if (_lastDir.sqrMagnitude > 0f && (sameDirX || sameDirY) && !(sameDirX && isAtMaxHeight))
            {
                return;
            }

            var digEmission = _digParticles.emission;
            digEmission.enabled = newPosition.y < 0f;

            _lastDir = direction;
            transform.position = newPosition;
            for (var i = 1; i < _lastPositions.Length; i++)
            {
                _segments[i].transform.position = _lastPositions[i - 1];
            }

            for (var i = 0; i < _segments.Length; i++)
            {
                _lastPositions[i] = _segments[i].transform.position;
            }
        }
    }

    #endregion
}