using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConfigManager : MonoBehaviour
{
    private User user;

    public Toggle showTooltip;
    //public TMP_InputField userName;
    public InputField productID;
    public InputField userName;
    public TextMeshProUGUI licenceName;

    private List<SimpleTooltip> tooltips = new List<SimpleTooltip>();

    public GameObject quitPanel;

    public Toggle monitorLedsToogle;
    public ShowingPanelScript monitorLeds;

    private void Start()
    {
        if (user == null)
            user = FindObjectOfType<User>();

        if (user != null)
        {

            showTooltip.isOn = user.showTooltips;
            monitorLedsToogle.isOn = user.showMonitorLed;
            userName.text = user.userName;
            productID.text = user.productID;
        }
        UpdateTooltipState();
        UpdateLicenceTextOnCOnfig();
        UpdateMonitorLedsState();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            quitPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        print("Quit");
        Application.Quit();

    }

    public void UpdateMonitorLedsState()
    {
        monitorLeds.showedLock = monitorLedsToogle.isOn;
        user.showMonitorLed = monitorLedsToogle.isOn;
    }

    public void UpdateTooltipState()
    {
        tooltips = FindObjectsOfTypeAllInScene<SimpleTooltip>();
        foreach(SimpleTooltip st in tooltips)
        {
            st.enabled = showTooltip.isOn;
        }

        user.showTooltips = showTooltip.isOn;
    }

    public void UpdateUserName()
    {
        user.userName = userName.text;
    }

    public void UpdateLicenceTextOnCOnfig()
    {
        if (user.currentLicence == LicenceManager.Licence.trial)
            licenceName.text = "Version de prueba";
        if (user.currentLicence == LicenceManager.Licence.Study)
            licenceName.text = "Version de estudio";
        if (user.currentLicence == LicenceManager.Licence.Teaching)
            licenceName.text = "Version para docentes";
        if (user.currentLicence == LicenceManager.Licence.Comercial)
            licenceName.text = "Version profesional";
    }

    public static List<T> FindObjectsOfTypeAllInScene<T>()
    {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }

    public bool dontQuit()
    {
        return true;
    }

    private void OnApplicationQuit()
    {
        quitPanel.SetActive(true);
        
        Application.wantsToQuit += dontQuit;

    }
}
