using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Agent), true)]
public class AgentEditor : Editor
{
    #region Methods

    /// <inheritdoc />
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying && GUILayout.Button("Activate"))
        {
            GameController.Instance.ActiveAgent = target as Agent;
        }
    }

    #endregion
}