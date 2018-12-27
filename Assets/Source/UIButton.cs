using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : Button
{
    #region Types

    [Serializable]
    public class ColorTint
    {
        #region Fields

        public Graphic Graphic;

        public ColorBlock Colors = ColorBlock.defaultColorBlock;

        #endregion

        #region Methods

        public void DoTransition(int state, bool instant)
        {
            if (Graphic == null)
            {
                return;
            }

            Color color;
            switch (state)
            {
                case 0:
                {
                    color = Colors.normalColor;
                    break;
                }
                case 1:
                {
                    color = Colors.highlightedColor;
                    break;
                }
                case 2:
                {
                    color = Colors.pressedColor;
                    break;
                }
                case 3:
                {
                    color = Colors.disabledColor;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            Graphic.CrossFadeColor(color, instant ? 0f : Colors.fadeDuration, true, true, true);
        }

        #endregion
    }

    [Serializable]
    public class SpriteSwap
    {
        #region Fields

        public Image Graphic;

        public SpriteState States;

        #endregion

        #region Methods

        public void DOTransition(int state, bool instant)
        {
            if (Graphic == null)
            {
                return;
            }

            Sprite sprite;
            switch (state)
            {
                case 0:
                {
                    sprite = null;
                    break;
                }
                case 1:
                {
                    sprite = States.highlightedSprite;
                    break;
                }
                case 2:
                {
                    sprite = States.pressedSprite;
                    break;
                }
                case 3:
                {
                    sprite = States.disabledSprite;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            Graphic.overrideSprite = sprite;
        }

        #endregion
    }

    #endregion

    #region Fields

    [SerializeField]
    private ColorTint[] _colorTints = new ColorTint[0];

    [SerializeField]
    private SpriteSwap[] _spriteSwaps = new SpriteSwap[0];

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        foreach (var colorTint in _colorTints)
        {
            colorTint.DoTransition((int) state, instant);
        }

        foreach (var spriteSwap in _spriteSwaps)
        {
            spriteSwap.DOTransition((int) state, instant);
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        transition = Transition.None;
        if (navigation.mode != Navigation.Mode.None)
        {
            navigation = new Navigation();
        }

        foreach (var colorTint in _colorTints)
        {
            if (colorTint.Colors.colorMultiplier < 1f)
            {
                colorTint.Colors = ColorBlock.defaultColorBlock;
            }
        }
    }
#endif

#endregion
}