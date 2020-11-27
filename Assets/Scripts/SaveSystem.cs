using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveUserData (User user)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.hapet";
        //Debug.Log(Application.persistentDataPath + "/user.hapet");

        FileStream stream = new FileStream(path, FileMode.Create);

        UserData data = new UserData(user);
        //Debug.Log(data.licenceIndex);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static UserData LoadUserData()
    {
        string path = Application.persistentDataPath + "/user.hapet";
        //Debug.Log(Application.persistentDataPath + "/user.hapet");

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UserData data = formatter.Deserialize(stream) as UserData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
