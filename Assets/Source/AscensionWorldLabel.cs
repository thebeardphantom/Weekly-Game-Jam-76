using TMPro;
using UnityEngine;

public class AscensionWorldLabel : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private TextMeshPro _ascensionLabel;

    private Agent _agent;

    private string _ascensionFormatString;

    #endregion

    #region Methods

    private void Awake()
    {
        _agent = GetComponentInParent<Agent>();
        EventBus.RegisterListener<AgentDiedEventBusData>(OnAgentDied);
        transform.SetParent(null, true);
        _ascensionFormatString = _ascensionLabel.text;
        _ascensionLabel.text = string.Format(_ascensionFormatString, _agent.AgentData.AscensionLevel);
    }

    private void OnAgentDied(AgentDiedEventBusData data)
    {
        if (_agent == null || data.Agent == _agent)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var renderers = GetComponentsInChildren<Renderer>(true);
        foreach(var renderer in renderers)
        {
            renderer.sortingLayerName = "WorldUI";
        }
    }

    #endregion
}