using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private UIButton _infoButton;

    [SerializeField]
    private Image _previewImage;

    [SerializeField]
    private GameObject _wormCountRoot;

    [SerializeField]
    private TextMeshProUGUI _wormCounter;

    [SerializeField]
    private CanvasGroup _fader;

    [SerializeField]
    private ParticleSystem _successParticles;

    [SerializeField]
    private RectTransform _nestIndicator;

    [SerializeField]
    private RectTransform _nestIndicatorArrowAnchor;

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
        Instance = this;
        EventBus.RegisterListener<ActiveAgentChangedEventBusData>(OnAgentChanged);
        _infoButton.interactable = false;
        _infoButton.onClick.AddListener(OnInfoButtonClick);
        _infoPopupCloseButton.onClick.AddListener(OnInfoPopupCloseButtonClick);
        _infoPopupCanvasGroup.alpha = 0f;
        _infoPopupCanvasGroup.blocksRaycasts = false;

        _nestIndicator.gameObject.SetActive(false);
        _wormCountRoot.SetActive(false);

        _fader.alpha = 1f;
        BeginFader(false);
        enabled = false;
    }

    private Tween BeginFader(bool fadeIn, EventBusData sender = null)
    {
        EventBus.FireEvent(new FaderBeginEventBusData(fadeIn)
        {
            Sender = sender
        });
        _fader.blocksRaycasts = false;
        var tween = _fader.DOFade(fadeIn ? 1f : 0f, 1f);
        tween.onComplete += () =>
        {
            EventBus.FireEvent(new FaderCompleteEventBusData(fadeIn)
            {
                Sender = sender
            });
            _fader.blocksRaycasts = fadeIn;
        };
        return tween;
    }

    private void OnAgentChanged(ActiveAgentChangedEventBusData data)
    {
        _infoButton.interactable = GameController.Instance.ActiveAgent != null;
        var death = data.ExistsUpstream<AgentDiedEventBusData>();
        if (death != null)
        {
            var tween = BeginFader(GameController.Instance.ActiveAgent == null, data);
            if (death.DeadAgent.Succeeded)
            {
                tween.onComplete += () =>
                {
                    _successParticles.Play(true);
                };
            }
        }

        if (GameController.Instance.ActiveAgent != null)
        {
            var isRabbit = GameController.Instance.ActiveAgent is RabbitAgent;
            _nestIndicator.gameObject.SetActive(isRabbit);
            _wormCountRoot.gameObject.SetActive(isRabbit);
            enabled = true;
        }
    }

    private void Update()
    {
        if (GameController.Instance.ActiveAgent is RabbitAgent rabbit)
        {
            _wormCounter.text = rabbit.CollectedWorms.ToString();

            var nestScreenReal = (Vector2)GameController.Instance.MainCamera.WorldToScreenPoint(
                GameController.Instance.Nest.transform.position);
            if (nestScreenReal.x <= Screen.width 
                && nestScreenReal.y <= Screen.height 
                && nestScreenReal.x >= 0f 
                && nestScreenReal.y >= 0f)
            {
                _nestIndicator.gameObject.SetActive(false);
                return;
            }

            _nestIndicator.gameObject.SetActive(true);
            var nestScreen = nestScreenReal;
            nestScreen.x = Mathf.Clamp(nestScreen.x, 150f, Screen.width - 150f);
            nestScreen.y = Mathf.Clamp(nestScreen.y, 150f, Screen.height - 150f);
            _nestIndicator.anchoredPosition = nestScreen;
            _nestIndicatorArrowAnchor.right = (nestScreenReal - _nestIndicator.anchoredPosition).normalized;
        }
    }

    private void OnInfoPopupCloseButtonClick()
    {
        _infoPopupCanvasGroup.DOFade(0f, 0.5f);
        Time.timeScale = 1f;
        _infoPopupCanvasGroup.blocksRaycasts = false;
    }

    private void OnInfoButtonClick()
    {
        var agentData = GameController.Instance.ActiveAgent.AgentData;
        var helpText = agentData.HelpText;
        helpText = TextFormatter.Format(helpText);

        Time.timeScale = 0f;
        _infoPopupText.text = helpText;
        _previewImage.sprite = agentData.PreviewSprite;

        _infoPopupCanvasGroup.DOFade(1f, 0.5f);
        _infoPopupCanvasGroup.blocksRaycasts = true;
    }

    #endregion
}