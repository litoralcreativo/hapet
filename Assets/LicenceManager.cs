using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

public class LicenceManager : MonoBehaviour
{

    public InputField keyField;

    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    public static string GetMacAddress()
    {
        var macAdress = "";
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        var i = 0;
        foreach (NetworkInterface adapter in nics)
        {
            PhysicalAddress address = adapter.GetPhysicalAddress();
            if (address.ToString() != "")
            {
                macAdress = address.ToString();
                return macAdress;
            }
        }
        return "error lectura mac address";
    }

    public static string GetHash(string input)
    {
        return string.Join("", (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input))).Select(x => x.ToString("X2")).ToArray());
    }

    private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

    public static string Encrypt(string encryptString)
    {
        string EncryptionKey = "HAPET2020";
        byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, _salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                encryptString = Convert.ToBase64String(ms.ToArray());
            }
        }
        return encryptString;
    }

    public static string Decrypt(string cipherText)
    {
        try
        {
            string EncryptionKey = "HAPET2020";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        catch
        {
            return null;
        }
        
    }

    public void UpdateLicence()
    {
        if (Decrypt(keyField.text) != null)
        {
            print("SameAddress? " + CheckKeyAddress(Decrypt(keyField.text).Split('|')[1]));
            print("Licence? " + CheckLicence(Decrypt(keyField.text).Split('|')[0]));

            if (CheckKeyAddress(Decrypt(keyField.text).Split('|')[1]))
                AddLicence(CheckLicence(Decrypt(keyField.text).Split('|')[0]));

        }
        else
            print("Invalid key");
    }

    private void AddLicence(string lic) {
        Licence licence = 0;
        if (lic == "Study")
            licence = Licence.Study;
        else if (lic == "Teaching")
            licence = Licence.Teaching;
        else if (lic == "Comercial")
            licence = Licence.Comercial;
        else licence = 0;

        FindObjectOfType<User>().currentLicence = licence;
        FindObjectOfType<User>().SaveData();
    }

    public bool CheckKeyAddress(string mac)
    {
        return mac == GetMacAddress();
    }

    public string CheckLicence(string licType)
    {
        
        return licType;
    }

    public enum Licence
    {
        trial,
        Study,
        Teaching,
        Comercial,
    }
}
