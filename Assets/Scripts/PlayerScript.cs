using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer player;
    public Button playbtn;
    public Button pausebtn;
    public float speedOfVideo;

    public void ChangeColorOfBtn(bool playBtnActive = false)
    {
        if (player == null)
        {
            player = GetComponent<UnityEngine.Video.VideoPlayer>();
        }
        var colors = playbtn.colors;
        var colors2 = pausebtn.colors;

        if (player.isPlaying || playBtnActive)
        {
            colors.normalColor = colors.pressedColor;
        }
        else
        {
            colors.normalColor = colors.selectedColor;
        }

        if (player.isPaused)
        {
            colors2.normalColor = colors2.pressedColor;
        }
        else
        {
            colors2.normalColor = colors2.selectedColor;
        }
        

        playbtn.colors = colors;
        pausebtn.colors = colors2;

    }

    public void OnValueChange(float value)
    {
        player.playbackSpeed = value / 2;
    }
}
