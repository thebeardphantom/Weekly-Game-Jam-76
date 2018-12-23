using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private TextMeshProUGUI _hintText;

    [SerializeField]
    private RectTransform _hintBox;

    [FormerlySerializedAs("_canvasGroup")]
    [SerializeField]
    private CanvasGroup _hintBoxCanvasGroup;

    private float _hintBoxShowY;

    #endregion

    #region Properties

    public static UIController Instance { get; private set; }

    #endregion

    #region Methods

    public void PublishHintText(string hint)
    {
        if(_hintText.text != hint || !DOTween.IsTweening(_hintBox) && _hintBox.anchoredPosition.y < 0f)
        {
            ResetHintBox();
            _hintBoxCanvasGroup.DOFade(1f, 0.5f);
            _hintBox.DOAnchorPosY(_hintBoxShowY, 0.5f).SetEase(Ease.OutBack);
            _hintText.text = hint;
            StartCoroutine(FadeOutHintBox());
        }
    }

    private IEnumerator FadeOutHintBox()
    {
        yield return new WaitForSeconds(5f);
        _hintBox.DOAnchorPosY(-100f, 2f);
        _hintBoxCanvasGroup.DOFade(0f, 2f);
    }

    private void Awake()
    {
        _hintBoxShowY = _hintBox.anchoredPosition.y;
        ResetHintBox();
        Instance = this;
    }

    private void ResetHintBox()
    {
        StopAllCoroutines();
        _hintBox.DOKill();
        _hintBoxCanvasGroup.DOKill();
        _hintBox.anchoredPosition = new Vector2(0f, -100f);
        _hintBoxCanvasGroup.alpha = 0f;
    }

    #endregion
}