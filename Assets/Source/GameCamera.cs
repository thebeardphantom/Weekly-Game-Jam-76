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

    #endregion

    #region Methods

    private void Start()
    {
        _camera = GetComponent<Camera>();

        GetParameters(out var position, out _, out var targetOrthoSize, out _, out var lifetimePosition);

        transform.position = position;
        _camera.orthographicSize = targetOrthoSize;
        _fxMaterial.SetFloat("_Vignette", lifetimePosition);
        _fxMaterial.SetFloat("_Desaturation", lifetimePosition);
    }

    private void LateUpdate()
    {
        GetParameters(out var targetPosition, out var lerpTime, out var targetOrthoSize, out var zoomTime, out var lifetimePosition);

        _fxMaterial.SetFloat("_Vignette", Mathf.Lerp(0f, 1.5f, lifetimePosition));
        _fxMaterial.SetFloat("_Desaturation", lifetimePosition * 0.5f);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _positionVelocity, lerpTime);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthoSize, ref _zoomVelocity, zoomTime);
    }

    private void GetParameters(out Vector3 targetPosition, out float lerpTime, out float targetOrthoSize, out float zoomTime, out float lifetimePosition)
    {
        zoomTime = 1f;
        targetOrthoSize = _defaultOrthoSize;
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
            targetOrthoSize = agentData.OrthoSize.Evaluate(lifetimePosition);
        }
        targetPosition.z = transform.position.z;
    }

    #endregion
}