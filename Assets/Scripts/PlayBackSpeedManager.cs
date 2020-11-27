using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBackSpeedManager : MonoBehaviour
{
    public Text text;
    public Slider slider;

    public void Start()
    {
        PlayBackSpeedText();
    }
    public void PlayBackSpeedText()
    {
        text.text = slider.value.ToString("0.00") + " x";
    }
}
