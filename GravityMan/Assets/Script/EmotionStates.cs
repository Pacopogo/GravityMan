using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 0 = Happy
 1 = Excited
 2 = Love
 3 = Sad
 4 = Angry
 */
public class EmotionStates : MonoBehaviour
{

    [Header("The Display")]
    [SerializeField] private Image emotionImage;
    [SerializeField] private TMP_Text emotionText;
    [SerializeField] private TMP_Text strengthText;


    //these are the influeances to give the player a emotion
    [Header("The Sliders")]
    [SerializeField] private Slider[] emotionSlider;

    [Header("Values")]
    private float avarageEmotion;

    private void Start()
    {
        CheckEmotion();
    }

    public void CheckEmotion()
    {
        foreach (var emotion in emotionSlider)
        {
            avarageEmotion += emotion.value;
        }

        avarageEmotion = avarageEmotion / (emotionSlider.Length + 1);
        avarageEmotion = Mathf.Round(avarageEmotion);

        SetStrength();
    }

    private void SetStrength()
    {
        if (avarageEmotion > 75)
            strengthText.text = "Strong";

        if (avarageEmotion <= 75 && avarageEmotion >= 20)
            strengthText.text = "Avarage";

        if (avarageEmotion < 20)
            strengthText.text = "Mild";
    }

    public void ResetEmotion()
    {
        foreach(var emotion in emotionSlider)
        {
            emotion.value = emotion.maxValue * 0.5f;
        }

        CheckEmotion();
    }
}
