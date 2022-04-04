using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EmojiType
{
    Undefined = 0,
    Angry = 1,
    Heartbreak = 2,
    Happy = 3,
    Confounded = 4,
    Tongue = 5,
    Flushed = 6,
    Amazed = 7,
    Heart = 8,
    Blush = 9,
}

[System.Serializable]
public struct Emoji
{
    public EmojiType EmojiType;
    public string EmojiText;
}

[CreateAssetMenu(fileName = "EmojiData", menuName = "Custom/EmojiData")]
public class EmojiData : ScriptableObject
{
    [SerializeField] public List<Emoji> Emojis;
}


public class EmojiDatabase
{
    private EmojiData Data;
    private Dictionary<EmojiType, string> EmojiDictionary;

    public static EmojiDatabase Instance;

    public static void Create(EmojiData data)
    {
        Instance = new EmojiDatabase(data);
    }

    public EmojiDatabase(EmojiData data)
    {
        if (Instance != null)
        {
            Debug.LogError("Duplicate EmojiDatabase");
            return;
        }
        Instance = this;
        Data = data;

        EmojiDictionary = new Dictionary<EmojiType, string>();
        foreach (var e in Data.Emojis)
        {
            if (EmojiDictionary.ContainsKey(e.EmojiType))
                Debug.LogError($"Duplicate entry: '{e.EmojiType}'");
            else
                EmojiDictionary.Add(e.EmojiType, e.EmojiText);
        }
    }

    public static string GetEmoji(EmojiType type)
    {
        return Instance.EmojiDictionary[type];
    }
}