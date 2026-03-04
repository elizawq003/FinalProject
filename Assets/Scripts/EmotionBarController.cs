using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class EmotionBarController : MonoBehaviour
{
    [SerializeField] private Slider anxietySlider;
    [SerializeField] private Slider sadnessSlider;
    [SerializeField] private Slider repulsionSlider;

    private Story inkStory;

    public void Initialize(Story story)
    {
        inkStory = story;

        // Configure Anxiety slider
        anxietySlider.minValue = 0;
        anxietySlider.maxValue = 3;
        anxietySlider.wholeNumbers = true;
        anxietySlider.value = (int)inkStory.variablesState["Anxiety"];


        // Configure Sadness slider
        sadnessSlider.minValue = 0;
        sadnessSlider.maxValue = 3;
        sadnessSlider.wholeNumbers = true;
        sadnessSlider.value = (int)inkStory.variablesState["Sadness"];

        // Configure Repulsion slider
        repulsionSlider.minValue = 0;
        repulsionSlider.maxValue = 3;
        repulsionSlider.wholeNumbers = true;
        repulsionSlider.value = (int)inkStory.variablesState["Repulsion"];

        // Push slider changes into Ink variables
        anxietySlider.onValueChanged.AddListener(v =>
        {
            if (inkStory != null)
            {
                inkStory.variablesState["Anxiety"] = (int)v;
                Debug.Log($"Anxiety set to {(int)v}");
            }
        });

        sadnessSlider.onValueChanged.AddListener(v =>
        {
            if (inkStory != null)
            {
                inkStory.variablesState["Sadness"] = (int)v;
                Debug.Log($"Sadness set to {(int)v}");
            }
        });

        repulsionSlider.onValueChanged.AddListener(v =>
        {
            if (inkStory != null)
            {
                inkStory.variablesState["Repulsion"] = (int)v;
                Debug.Log($"Repulsion set to {(int)v}");
            }
        });
    }

    
}
