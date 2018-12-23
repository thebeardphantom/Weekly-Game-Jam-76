using System.Linq;
using UnityEngine;

public class WormAgent : Agent
{
    [SerializeField]
    private SpriteRenderer _headSegment;

    [SerializeField]
    private Sprite _altSprite;

    [SerializeField]
    private float _offset;

    [SerializeField]
    private int _segmentCount = 6;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _maxYPosition;

    private SpriteRenderer[] _segments;

    private Vector3[] _lastPositions;

    private Vector2 _lastDir;

    private int _errCount;

    private void Awake()
    {
        _segments = new SpriteRenderer[_segmentCount];
        _segments[0] = _headSegment;
        _lastPositions = new Vector3[_segmentCount];
        _lastPositions[0] = _segments[0].transform.position;
        for (var i = 1; i < _segments.Length; i++)
        {
            _segments[i] = Instantiate(_headSegment);
            _segments[i].sprite = i % 2 == 0 ? _segments[0].sprite : _altSprite;
            _segments[i].transform.parent = transform;
            _segments[i].transform.position = transform.position + Vector3.right * _offset * i;
            _lastPositions[i] = _segments[i].transform.position;
        }
    }

    private void Update()
    {
        var direction = Vector2.zero;
        if (KeyDown(KeyCode.W, KeyCode.UpArrow))
        {
            direction.y += 1f;
        }
        else if (KeyDown(KeyCode.S, KeyCode.DownArrow))
        {
            direction.y += -1f;
        }
        else if (KeyDown(KeyCode.D, KeyCode.RightArrow))
        {
            direction.x += 1f;
        }
        else if (KeyDown(KeyCode.A, KeyCode.LeftArrow))
        {
            direction.x += -1f;
        }
        if (direction.sqrMagnitude > 0f)
        {
            if (transform.position.y >= _maxYPosition && direction.y > 0f)
            {
                _errCount++;
                if (_errCount > 3)
                {
                    _errCount = 0;
                    RegisterIncorrectAction("You're a worm, not a bird!");
                }
                return;
            }

            var newPosition = transform.position + (Vector3)direction * _moveSpeed;
            if (_segments.Any(p => Approx(newPosition, p.transform.position)))
            {
                _errCount++;
                if (_errCount > 3)
                {
                    _errCount = 0;
                    RegisterIncorrectAction("You can't move through yourself!");
                }
                return;
            }

            if (_lastDir.sqrMagnitude > 0f
                && (Mathf.Approximately(Mathf.Abs(_lastDir.x), Mathf.Abs(direction.x))
                    || Mathf.Approximately(Mathf.Abs(_lastDir.y), Mathf.Abs(direction.y))))
            {
                _errCount++;
                if(_errCount > 3)
                {
                    _errCount = 0;
                    RegisterIncorrectAction("Wiggle like a worm: move one axis at a time!");
                }
                return;
            }

            _errCount = 0;
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

    private bool KeyDown(params KeyCode[] keys)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }

        return false;
    }

    private static bool Approx(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
    }
}