using UnityEngine;

public class GameCamera : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private float _positionLerpTime;

    [SerializeField]
    private float _zoomLerpTime;

    private Vector3 _positionVelocity;

    private Camera _camera;

    private float _zoomVelocity;

    #endregion

    #region Methods

    private void Start()
    {
        _camera = GetComponent<Camera>();
        var trackedAgent = GameController.Instance.ActiveAgent;
        if (trackedAgent != null && trackedAgent.CameraTrackTarget != null)
        {
            var target = trackedAgent.CameraTrackTarget.position;
            target.z = transform.position.z;
            transform.position = target;
            _camera.orthographicSize = trackedAgent.AgentData.OrthoSize;
        }
    }

    private void LateUpdate()
    {
        var trackedAgent = GameController.Instance.ActiveAgent;
        if (trackedAgent != null && trackedAgent.CameraTrackTarget != null)
        {
            var target = trackedAgent.CameraTrackTarget.position;
            target.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _positionVelocity, _positionLerpTime);
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, trackedAgent.AgentData.OrthoSize, ref _zoomVelocity, _zoomLerpTime);
        }
    }

    #endregion
}