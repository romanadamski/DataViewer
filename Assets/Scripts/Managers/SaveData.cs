using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<uint> Highscore;

    public SaveData()
    {
        Highscore = new List<uint>();
    }
}
