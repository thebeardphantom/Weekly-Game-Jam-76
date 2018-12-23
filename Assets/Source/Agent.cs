using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Fields

    [Header("Camera")]
    [SerializeField]
    private Transform _cameraTrackTarget;

    [SerializeField]
    private float _orthoSize;

    #endregion

    #region Properties

    public Transform CameraTrackTarget => _cameraTrackTarget;

    public float OrthoSize => _orthoSize;

    #endregion

    public void RegisterIncorrectAction(string msg)
    {
        UIController.Instance.PublishHintText(msg);
    }
}