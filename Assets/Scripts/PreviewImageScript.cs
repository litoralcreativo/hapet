using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PreviewImageScript : MonoBehaviour
{
    public VideoPlayer mainVideoPLayer;
    private VideoPlayer secondaryVideoPlayer;
    private RawImage img;
    [Range(100, 400)]
    public float scale = 200;
    private int videoWith;
    private int videoHeight;
    double length = 0;


    private void Start()
    {
        secondaryVideoPlayer = GetComponent<VideoPlayer>();
        img = GetComponent<RawImage>();
        GetVideo();
        //secondaryVideoPlayer.Play();
    }

    public void GetVideo(float percent = 0)
    {
        // if got video loaded
        if (mainVideoPLayer.url != null)
        {
                string url = mainVideoPLayer.url;
                secondaryVideoPlayer.source = VideoSource.Url;
                secondaryVideoPlayer.url = url;
                Texture vidTex = secondaryVideoPlayer.texture;

                float ratio = 1;
            if (!secondaryVideoPlayer.isPrepared)
            {
                secondaryVideoPlayer.prepareCompleted += (VideoPlayer source) =>
                {
                    videoWith = source.texture.width;
                    videoHeight = source.texture.height;

                    ratio = ((float)videoWith / (float)videoHeight);
                    img.GetComponent<RectTransform>().sizeDelta = new Vector2(scale, scale / ratio);
                    length = secondaryVideoPlayer.length;
                    print(length);
                };
            }
                
            
            double previewTime = length * percent;
            secondaryVideoPlayer.time = previewTime;
            //print(previewTime);
            //secondaryVideoPlayer.Play();
            SkiptPercent(percent);
            //img.texture = secondaryVideoPlayer.targetTexture;
            //secondaryVideoPlayer.Pause();

        }
    }

    public void SkiptPercent(float pct)
    {
        secondaryVideoPlayer.Play();
        var frame = secondaryVideoPlayer.frameCount * pct;
        secondaryVideoPlayer.frame = (long)frame;
        img.texture = secondaryVideoPlayer.targetTexture;
        secondaryVideoPlayer.Pause();
    }

    public void GetImageofFrame(float percent)
    {

    }
}
