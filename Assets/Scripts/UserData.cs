using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string userName;
    public bool premiumState;
    public int licenceIndex;
    public bool showTooltips;
    public bool showMonitorLed;
    

    public string productID;

    public UserData (User user)
    {
        userName = user.userName;
        premiumState = user.premiumUser;
        licenceIndex = (int)user.currentLicence;
        //Debug.Log(user.currentLicence);
        //Debug.Log((int)user.currentLicence);
        showTooltips = user.showTooltips;
        showMonitorLed = user.showMonitorLed;
        productID = user.productID;

    }
}
