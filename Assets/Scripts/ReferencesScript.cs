using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ReferencesScript : MonoBehaviour
{
    public List<GameObject> gridList;
    public VideoPlayer videoPlayer;

    public void showOnlyRefQuad()
    {
        foreach (GameObject go in gridList)
        {
            go.SetActive(false);
        }
        gridList[0].SetActive(true);
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
    }

    public void ShowGrid()
    {
        foreach(GameObject go in gridList)
        {
            go.SetActive(!go.activeSelf);
        }
        gridList[0].SetActive(gridList[1].activeSelf);
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
    }
}
