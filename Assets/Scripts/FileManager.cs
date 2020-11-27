using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SFB;

public class FileManager : MonoBehaviour
{
    [ShowOnly] public string path;
    [ShowOnly] public string saveUrl;
    [ShowOnly] public string saveExten;
    string[] formats = { "Video files", "mp4,avi,wmv,mkv", "All files", "*" };

    public string patientName;

    
    // C:/Users/Gaston/Desktop/BioIng/FABIAN KIN.avi

    [ShowOnly] public UnityEngine.Video.VideoPlayer player;

    
    private void Start()
    {
        player = GetComponent<UnityEngine.Video.VideoPlayer>();
        if (player.url != null)
        {
            path = player.url;
        }
        

    }

    public void SetPatientName(string st)
    {
        patientName = st;
    }

    public void OpenExplorer()
    {
        var extensions = new[] {
        new ExtensionFilter("Video Files", "avi", "mp4", "wmv", "mkv"),
        new ExtensionFilter("All Files", "*" ),
        };

        StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] _path) => { GetVideo(_path); });
    }

    public void SaveFile()
    {
        string fileName = "";
        if (patientName != "")
        {
            fileName = patientName + " (" + System.DateTime.Today.Date.Day + System.DateTime.Today.Date.Month + System.DateTime.Today.Date.Year + "-" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ")";
        }
        else
        {
            fileName = "HAPET (" + System.DateTime.Today.Date.Day + System.DateTime.Today.Date.Month + System.DateTime.Today.Date.Year + "-" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ")";
        }
        /*
        var extensionList = new[] {
        new ExtensionFilter("Pdf", "pdf"),
        new ExtensionFilter("Csv", "csv"),
        new ExtensionFilter("Text", "txt"),};

        var savePath = StandaloneFileBrowser.SaveFilePanel("Save File", "", fileName, extensionList);
        */
        var extensions = new[] {
        new ExtensionFilter("Pdf", "pdf"),
        //new ExtensionFilter("Csv", "csv"),
        //new ExtensionFilter("Text", "txt"),
        };
        var savePath = StandaloneFileBrowser.SaveFilePanel("Save File", "", fileName, extensions);

        if (savePath != "")
        {
            saveUrl = savePath;
            saveExten = savePath.Substring(savePath.Length - 3);
        }
    }


    void GetVideo(string[] name)
    {
        if (name.Length == 1)
        {
            path = name[0];
        } else
        {
            print("diferent than 1 path");
        }

        if (path != "")
        {
            player.url = path;
            print(path);
            //player.Play();  
        }
    }

    public void StepBack()
    {
        if (path != null)
        {
            player.frame--;
            if (player.isPlaying)
            {
                player.Pause();
            }
        }
    }
}
