using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : BaseManager<SaveManager>
{
    private const string SAVE_KEY = "SAVE";
    private SaveData saveData;

    public void Save()
    {
        var saveString = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, saveString);
    }

    public void LoadSave()
    {
        var saveString = PlayerPrefs.GetString(SAVE_KEY);
        if (string.IsNullOrWhiteSpace(saveString))
        {
            saveData = new SaveData();
        }
        else
        {
            saveData = JsonUtility.FromJson<SaveData>(saveString);
        }
    }
}
