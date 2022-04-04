using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PopupEmoji : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextField;
    [SerializeField] private float TimeAlive = 2.5f;

    private Camera _mainCamera;
    private float TimePassed = 0.0f;    

    private void Awake()
    {
        _mainCamera = Camera.main;
        transform.localPosition = new Vector3(1.5f, 4.0f, -2.0f);
    }

    public void Setup(EmojiType emojiType)
    {
        TextField.text = $"<sprite={EmojiData.GetEmoji(emojiType)}>";
    }

    private void Update()
    {
        transform.LookAt(_mainCamera.transform);
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        TimePassed += Time.unscaledDeltaTime;

        if (TimePassed >= TimeAlive)
        {
            Destroy(gameObject);
        }
    }
}