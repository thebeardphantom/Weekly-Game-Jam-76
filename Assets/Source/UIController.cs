using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private UIButton _infoButton;

    [SerializeField]
    [Header("Info Popup")]
    [FormerlySerializedAs("_hintText")]
    private TextMeshProUGUI _infoPopupText;

    [SerializeField]
    private UIButton _infoPopupCloseButton;

    [SerializeField]
    [FormerlySerializedAs("_hintBoxCanvasGroup")]
    private CanvasGroup _infoPopupCanvasGroup;

    #endregion

    #region Properties

    public static UIController Instance { get; private set; }

    #endregion

    #region Methods

    private void Awake()
    {
        _infoButton.onClick.AddListener(OnInfoButtonClick);
        _infoPopupCloseButton.onClick.AddListener(OnInfoPopupCloseButtonClick);
        _infoPopupCanvasGroup.alpha = 0f;
        _infoPopupCanvasGroup.blocksRaycasts = false;
        Instance = this;
    }

    private void OnInfoPopupCloseButtonClick()
    {
        _infoPopupCanvasGroup.DOFade(0f, 0.5f);
        _infoPopupCanvasGroup.blocksRaycasts = false;
    }

    private void OnInfoButtonClick()
    {
        _infoPopupCanvasGroup.DOFade(1f, 0.5f);
        _infoPopupCanvasGroup.blocksRaycasts = true;
        var helpText = GameController.Instance.ActiveAgent.AgentData.HelpText;
        helpText = TextFormatter.Format(helpText);
        _infoPopupText.text = helpText;
    }

    #endregion
}