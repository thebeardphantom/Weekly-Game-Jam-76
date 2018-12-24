using UnityEngine;

[CreateAssetMenu(menuName = "CUSTOM/AgentData")]
public class AgentData : ScriptableObject
{
    #region Fields

    [SerializeField]
    [Header("Camera Settings")]
    private float _orthoSize;

    [SerializeField]
    private float _lifetime = 120f;

    [SerializeField]
    private int _ascensionLevel;

    [SerializeField]
    [TextArea(5, 20)]
    private string _helpText;

    #endregion

    #region Properties

    public float OrthoSize => _orthoSize;

    public string HelpText => _helpText;

    public float Lifetime => _lifetime;

    public int AscensionLevel => _ascensionLevel;

    #endregion
}