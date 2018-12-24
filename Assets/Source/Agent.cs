using TMPro;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private TextMeshPro _ascensionLabel;

    [SerializeField]
    private Transform _cameraTrackTarget;

    [SerializeField]
    private AgentData _data;

    private string _ascensionFormatString;

    #endregion

    #region Properties

    public bool IsPlayer => GameController.Instance.ActiveAgent == this;

    public Transform CameraTrackTarget => _cameraTrackTarget;

    public AgentData AgentData => _data;

    #endregion

    #region Methods

    protected virtual void Awake()
    {
        _ascensionFormatString = _ascensionLabel.text;
        _ascensionLabel.text = string.Format(_ascensionFormatString, _data.AscensionLevel);
    }

    #endregion
}