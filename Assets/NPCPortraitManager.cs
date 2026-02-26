using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCPortraitManager : MonoBehaviour
{
    public Image portraitImage;

    [System.Serializable]
    public struct PortraitEntry
    {
        public string tag;
        public Sprite sprite;
    }

    public List<PortraitEntry> portraits = new List<PortraitEntry>();
    private Dictionary<string, Sprite> portraitDict = new Dictionary<string, Sprite>();


    void Awake()
    {
        foreach (var entry in portraits)
            portraitDict[entry.tag] = entry.sprite;
    }

    public void SetExpression(string tag)
    {
        if (portraitDict.TryGetValue(tag, out Sprite sprite))
        {
            portraitImage.sprite = sprite;
            portraitImage.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        portraitImage.gameObject.SetActive(false);
    }

}
