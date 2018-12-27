using UnityEngine;

[CreateAssetMenu(menuName = "CUSTOM/AgentData")]
public class AgentData : ScriptableObject
{
    #region Fields

    [SerializeField]
    private int _ascensionLevel;

    [SerializeField]
    private AnimationCurve _lifetimeScaleCurve = AnimationCurve.Constant(0f, 1f, 1f);

    [SerializeField]
    private Gradient _lifetimeTint;

    [SerializeField]
    private float _lifetime = 120f;

    [SerializeField]
    [Header("Camera Settings")]
    private AnimationCurve _orthoSize;

    [SerializeField]
    private AnimationCurve _cameraLerpTime;

    [SerializeField]
    private AnimationCurve _cameraZoomTime;

    [SerializeField]
    [TextArea(5, 20)]
    private string _helpText;

    #endregion

    #region Properties

    public AnimationCurve OrthoSize => _orthoSize;

    public string HelpText => _helpText;

    public float Lifetime => _lifetime;

    public int AscensionLevel => _ascensionLevel;

    public AnimationCurve LifetimeScaleCurve => _lifetimeScaleCurve;

    public Gradient LifetimeTint => _lifetimeTint;

    public AnimationCurve CameraLerpTime => _cameraLerpTime;

    public AnimationCurve CameraZoomTime => _cameraZoomTime;

    #endregion
}