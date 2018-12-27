using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupScreen : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Animator[] _introAnimators;

    [SerializeField]
    private UIButton _nextScreen;

    [SerializeField]
    private UIButton _websiteLink;

    [SerializeField]
    private CanvasGroup[] _screens;

    private int _screenNum;

    #endregion

    #region Methods

    private void Awake()
    {
        _nextScreen.onClick.AddListener(OnNextScreenClick);
        _nextScreen.gameObject.SetActive(false);
        _websiteLink.onClick.AddListener(OnWebsiteLinkButtonClicked);
        foreach (var screen in _screens)
        {
            screen.alpha = 0f;
        }
    }

    private void OnWebsiteLinkButtonClicked()
    {
        Process.Start("http://beardphantom.com");
    }

    private void OnNextScreenClick()
    {
        if (_screenNum == _screens.Length)
        {
            SceneManager.LoadScene(1);
            return;
        }

        for (var i = 0; i < _screens.Length; i++)
        {
            var screen = _screens[i];
            if (i - 1 == _screenNum)
            {
                screen.DOFade(0f, 0.125f);
            }
            else if (i == _screenNum)
            {
                screen.DOFade(1f, 0.125f);
            }
            else
            {
                screen.alpha = 0f;
            }
        }

        _screenNum++;
    }

    private void Update()
    {
        foreach (var animator in _introAnimators)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime < 1f)
            {
                return;
            }
        }

        _nextScreen.gameObject.SetActive(true);
        enabled = false;
    }

    #endregion
}