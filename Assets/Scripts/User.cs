using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class User : MonoBehaviour
{
    // user settings
    public string userName = "Usuario";
    [ShowOnly] public bool premiumUser = false;

    [ShowOnly] public LicenceManager.Licence currentLicence;

    // config panel data
    [ShowOnly] public bool showTooltips = false;
    [ShowOnly] public bool showMonitorLed = false;
    [ShowOnly] public string userMacAddress;
    [ShowOnly] public string productID;

    public static bool created = false;
    public void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (userMacAddress == "" || userMacAddress == null)
        {
            userMacAddress = LicenceManager.GetMacAddress();
            //Debug.Log("MAC Adress: " + LicenceManager.GetMacAddress());
        }

        if (productID == "" || productID == null)
        {
            productID = LicenceManager.Encrypt(userMacAddress);
            //Debug.Log("Product ID: " + productID);
        }

        //SaveData();

        LoadData();
    }

    public void Start()
    {
        /*string enc = LicenceManager.Encrypt(productID);
        print(LicenceManager.Encrypt("Gaston"));
        print(LicenceManager.Decrypt(enc));*/
    }

    public void SaveData()
    {
        SaveSystem.SaveUserData(this);
        print("Save Data");
    }

    public void LoadData()
    {
        UserData data = SaveSystem.LoadUserData();

        userName = data.userName;
        premiumUser = data.premiumState;
        showTooltips = data.showTooltips;
        showMonitorLed = data.showMonitorLed;
        productID = data.productID;
        currentLicence = (LicenceManager.Licence)data.licenceIndex;
    }

    public void ClearData()
    {
        userName = "Usuario";
        showTooltips = true;
        showMonitorLed = true;
        Debug.Log("Data Cleared");
        SaveData();
    }

}
