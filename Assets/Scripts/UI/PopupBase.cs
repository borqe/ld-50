using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PopupBase: MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _timeToClick = 5.0f;
    [SerializeField] private float _timePassed = 0.0f;
    
    [Header("Radial Image")]
    [SerializeField] private Image _radialImage;
    
    public Action onTimerEnd;
    public Action onPopupClicked;

    private void Awake()
    {
        _mainCamera = Camera.main;
        transform.localPosition = new Vector3(1.5f, 4.0f, -2.0f);
    }

    private void OnDisable()
    {
        ClearCaches();
    }

    private void OnDestroy()
    {
        ClearCaches();
    }

    private void ClearCaches()
    {
        onTimerEnd = null;
        onPopupClicked = null;
    }

    private void Update()
    {
        transform.LookAt(_mainCamera.transform);
        UpdateTimer();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        _radialImage.fillAmount = 1.0f - _timePassed / _timeToClick;
    }

    private void UpdateTimer()
    {
        _timePassed += Time.unscaledDeltaTime;

        if (_timeToClick <= _timePassed)
        {
            onTimerEnd?.Invoke();
            Destroy(gameObject);
        }
    }
}