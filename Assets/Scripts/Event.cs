using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event<T> where T : Event<T>
{
    // This property is to prevent spamming of the event
    private bool _hasBeenFired;
    public delegate void EventListener(T info);
    private static event EventListener _listeners;

    public static void AddListener(EventListener listener) => _listeners += listener;
    public static void RemoveListener(EventListener listener) => _listeners -= listener;

    public void Broadcast()
    {
        if (_hasBeenFired)
            return;

        _hasBeenFired = true;

        if (_listeners != null)
            _listeners(this as T);
    }
}