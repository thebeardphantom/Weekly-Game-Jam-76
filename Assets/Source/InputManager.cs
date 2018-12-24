using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    #region Types

    [Serializable]
    public class InputBinding
    {
        #region Fields

        public string Id;

        [FormerlySerializedAs("Name")]
        public string DisplayName;

        public KeyCode[] Bindings;

        #endregion

        #region Methods

        public bool IsDown()
        {
            foreach (var binding in Bindings)
            {
                if (Input.GetKeyDown(binding))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }

    #endregion

    #region Fields

    [SerializeField]
    private InputBinding[] _bindings;

    private Dictionary<string, InputBinding> _bindingLookup
        = new Dictionary<string, InputBinding>();

    #endregion

    #region Properties

    public static InputManager Instance { get; private set; }

    #endregion

    #region Methods

    public bool AnyDown(params string[] ids)
    {
        foreach (var id in ids)
        {
            var binding = _bindingLookup[id];
            if (binding.IsDown())
            {
                return true;
            }
        }

        return false;
    }

    public bool AllDown(params string[] ids)
    {
        foreach (var id in ids)
        {
            var binding = _bindingLookup[id];
            if (!binding.IsDown())
            {
                return false;
            }
        }

        return true;
    }

    public InputBinding GetBinding(string id)
    {
        return _bindingLookup[id];
    }

    private void Awake()
    {
        Instance = this;
        _bindingLookup = _bindings.ToDictionary(b => b.Id, b => b);
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            _bindingLookup = _bindings.ToDictionary(b => b.Id, b => b);
        }
    }

    #endregion
}