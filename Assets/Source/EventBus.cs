﻿using System;
using System.Collections.Generic;
using System.Reflection;

public static class EventBus
{
    #region Types

    public delegate void OnEventFired<T>(T data);

    private class RegisteredListener<T> : IRegisteredListener
    {
        #region Fields

        public readonly OnEventFired<T> Callback;

        #endregion

        #region Properties

        /// <inheritdoc />
        public object Target => Callback.Target;

        /// <inheritdoc />
        public MethodBase Method => Callback.Method;

        #endregion

        public RegisteredListener(OnEventFired<T> callback)
        {
            Callback = callback;
        }

        #region Methods

        /// <inheritdoc />
        public void FireListener(object data)
        {
            Callback((T) data);
        }

        #endregion
    }

    private interface IRegisteredListener
    {
        #region Properties

        object Target { get; }

        MethodBase Method { get; }

        #endregion

        #region Methods

        void FireListener(object data);

        #endregion
    }

    #endregion

    #region Fields

    private static readonly Dictionary<Type, List<IRegisteredListener>> _listenerMap
        = new Dictionary<Type, List<IRegisteredListener>>();

    #endregion

    #region Methods

    public static void RegisterListener<T>(OnEventFired<T> callback) where T : EventBusData
    {
        var map = GetMap(typeof(T));
        map.Add(new RegisteredListener<T>(callback));
    }

    public static void RemoveListener<T>(OnEventFired<T> callback) where T : EventBusData
    {
        var map = GetMap(typeof(T));
        map.RemoveAll(m => ((RegisteredListener<T>) m).Callback == callback);
    }

    public static void RemoveTarget(object target)
    {
        foreach (var list in _listenerMap.Values)
        {
            list.RemoveAll(l => l.Target == target);
        }
    }

    public static void FireEvent(EventBusData data)
    {
        var map = GetMap(data.GetType());
        map.RemoveAll(m => m.Target == null && !m.Method.IsStatic);
        var copy = new IRegisteredListener[map.Count];
        map.CopyTo(copy);
        foreach (var listener in copy)
        {
            listener.FireListener(data);
        }
    }

    private static List<IRegisteredListener> GetMap(Type type)
    {
        if (!_listenerMap.TryGetValue(type, out var list))
        {
            list = new List<IRegisteredListener>();
            _listenerMap[type] = list;
        }

        return list;
    }

    #endregion
}