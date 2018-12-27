using UnityEngine;

public class GameCamera : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float _defaultOrthoSize;

    [SerializeField]
    private Material _fxMaterial;

    private Vector3 _positionVelocity;

    private Camera _camera;

    private float _zoomVelocity;

    private bool _isUsingAgentOrtho;

    private float _userTargetOrtho;

    #endregion

    #region Methods

    private void Awake()
    {
        EventBus.RegisterListener<FaderCompleteEventBusData>(OnFaderComplete);
        EventBus.RegisterListener<FaderBeginEventBusData>(OnFaderBegin);
        _userTargetOrtho = _defaultOrthoSize;
    }

    private void OnFaderBegin(FaderBeginEventBusData data)
    {
        enabled = false;
        _userTargetOrtho = _defaultOrthoSize;
        _isUsingAgentOrtho = true;
    }

    private void OnFaderComplete(FaderCompleteEventBusData data)
    {
        enabled = true;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();

        GetParameters(out var position, out var _, out var targetOrthoSize, out var _, out var lifetimePosition);

        transform.position = position;
        _camera.orthographicSize = targetOrthoSize;
        _fxMaterial.SetFloat("_Vignette", lifetimePosition);
        _fxMaterial.SetFloat("_Desaturation", lifetimePosition);
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0f)
        {
            if (_isUsingAgentOrtho)
            {
                _isUsingAgentOrtho = false;
                _userTargetOrtho = _camera.orthographicSize;
            }

            _userTargetOrtho -= Input.mouseScrollDelta.y * Time.deltaTime * 10f;
            _userTargetOrtho = Mathf.Max(0.5f, _userTargetOrtho);
        }

        GetParameters(out var targetPosition, out var lerpTime, out var targetOrthoSize, out var zoomTime, out var lifetimePosition);

        _fxMaterial.SetFloat("_Vignette", Mathf.Lerp(0f, 1.5f, lifetimePosition));
        _fxMaterial.SetFloat("_Desaturation", lifetimePosition * 0.75f);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _positionVelocity, lerpTime);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthoSize, ref _zoomVelocity, zoomTime);
    }

    private void GetParameters(out Vector3 targetPosition, out float lerpTime, out float targetOrthoSize, out float zoomTime, out float lifetimePosition)
    {
        zoomTime = 1f;
        targetOrthoSize = _userTargetOrtho;
        lerpTime = 1f;
        targetPosition = Vector3.zero;
        lifetimePosition = 0f;
        var trackedAgent = GameController.Instance.ActiveAgent;
        if (trackedAgent != null && trackedAgent.CameraTrackTarget != null)
        {
            var agentData = trackedAgent.AgentData;
            lifetimePosition = trackedAgent.LifetimePosition;
            lerpTime = agentData.CameraLerpTime.Evaluate(lifetimePosition);
            targetPosition = trackedAgent.CameraTrackTarget.position;
            zoomTime = agentData.CameraZoomTime.Evaluate(lifetimePosition);
            targetOrthoSize = _isUsingAgentOrtho
                ? agentData.OrthoSize.Evaluate(lifetimePosition)
                : _userTargetOrtho;
        }

        zoomTime = _isUsingAgentOrtho ? zoomTime : 0.1f;
        targetPosition.z = transform.position.z;
    }

    #endregion
}