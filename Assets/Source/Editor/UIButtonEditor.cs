using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIButton))]
public class UIButtonEditor : ButtonEditor
{
    #region Fields

    private SerializedProperty _colorTints;
    private SerializedProperty _spriteSwaps;

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(_colorTints, true);
        EditorGUILayout.PropertyField(_spriteSwaps, true);
        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _colorTints = serializedObject.FindProperty("_colorTints");
        _spriteSwaps = serializedObject.FindProperty("_spriteSwaps");
    }

    #endregion
}