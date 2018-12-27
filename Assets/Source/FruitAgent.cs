using UnityEngine;

public class FruitAgent : Agent<FruitAgent>
{
    #region Fields

    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private SpringJoint2D _anchor;

    [SerializeField]
    private float _wiggleForce;

    private int _requiredWiggles;

    private int _wiggles;

    private float _aiFallTime;

    #endregion

    #region Properties

    public bool HasFallen { get; private set; }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Kill(Agent source)
    {
        if (source.GetType() == typeof(WormAgent))
        {
            Succeeded = true;
        }

        base.Kill(source);
    }

    protected override void Awake()
    {
        base.Awake();
        _requiredWiggles = Random.Range(10, 20);
        _aiFallTime = Random.Range(10f, 40f);
        Graphics.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        Graphics.sortingOrder = 2;
        _anchor.connectedAnchor = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (HasFallen)
        {
            return;
        }

        if (IsPlayer)
        {
            var dir = GetDirectionalInput();
            if (dir.sqrMagnitude > 0f)
            {
                _wiggles++;
                _rigidbody.AddForce(dir * _wiggleForce);
                if (_wiggles >= _requiredWiggles)
                {
                    BreakAnchor();
                }
            }
        }
        else if (Time.time - SpawnTime > _aiFallTime)
        {
            BreakAnchor();
        }
    }

    private void BreakAnchor()
    {
        _anchor.enabled = false;
        HasFallen = true;
        Graphics.sortingOrder = 0;
    }

    #endregion
}