using UnityEngine;

public class GameCamera : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Agent _trackedAgent;

    [SerializeField]
    private float _positionLerpTime;

    [SerializeField]
    private float _zoomLerpTime;

    private Vector3 _positionVelocity;

    private Camera _camera;

    private float _zoomVelocity;

    #endregion

    #region Methods

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        if (_trackedAgent != null && _trackedAgent.CameraTrackTarget != null)
        {
            var target = _trackedAgent.CameraTrackTarget.position;
            target.z = transform.position.z;
            transform.position = target;
            _camera.orthographicSize = _trackedAgent.OrthoSize;
        }
    }

    private void LateUpdate()
    {
        if (_trackedAgent == null || _trackedAgent.CameraTrackTarget == null)
        {
            return;
        }

        var target = _trackedAgent.CameraTrackTarget.position;
        target.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _positionVelocity, _positionLerpTime);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _trackedAgent.OrthoSize, ref _zoomVelocity, _zoomLerpTime);
    }

    #endregion
}