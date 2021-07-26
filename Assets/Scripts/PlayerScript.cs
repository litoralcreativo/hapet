using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer player;
    public Button playbtn;
    public Image playPauseImgHolder;
    public Image volumeImgHolder;
    public List<Sprite> sprites;
    public float speedOfVideo;
    public FileManager manager;
    private bool muted = true;

    public void ChangeColorOfBtn(bool playBtnActive = false)
    {
        if (player == null)
        {
            player = GetComponent<UnityEngine.Video.VideoPlayer>();
        }
        var colors = playbtn.colors;

        if (player.isPlaying || player.isPaused || playBtnActive)
        {
            colors.normalColor = colors.pressedColor;
        }
        else 
        {
            colors.normalColor = colors.selectedColor;
        }
        
        playbtn.colors = colors;
    }

    public void TooglePlayPause()
    {
        if (player.isPlaying)
        {
            player.Pause();
            playPauseImgHolder.sprite = sprites[1];
        }
        else if (player.isPaused)
        {
            player.Play();
            playPauseImgHolder.sprite = sprites[0];
        }
        else // its stopped
        {
            playPauseImgHolder.sprite = sprites[0];
            player.Play();
        }
    }

    public void ToogleMuted()
    {
        if (muted)
        {
            player.SetDirectAudioMute(0, false);
            volumeImgHolder.sprite = sprites[2];
        }
        else
        {
            player.SetDirectAudioMute(0, true);
            volumeImgHolder.sprite = sprites[3];
        }
        muted = !muted;
    }



    public void OnValueChange(float value)
    {
        player.playbackSpeed = value / 2;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            TooglePlayPause();
        } 
        else if (Input.GetKeyDown("left"))
        {
            manager.StepBack();
        }
        else if (Input.GetKeyDown("right"))
        {
            player.StepForward();
        }
    }
}
