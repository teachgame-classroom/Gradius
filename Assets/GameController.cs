using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GradiusSave
{
    public int hiscore;
}

public class GameController : MonoBehaviour
{
    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameController").GetComponentInChildren<GameController>();
            }
            return _instance;
        }
    }
    private static GameController _instance;


    public const int MAX_SCORE = 9999999;
    public int score { get; private set; }
    public int hiscore { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        InitScore();
        UIManager.instance.OnScoreChanged(score);
        UIManager.instance.OnHiScoreChanged(hiscore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitScore()
    {
        score = 0;
        LoadHiScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        score = Mathf.Min(score, MAX_SCORE);
        UIManager.instance.OnScoreChanged(score);

        if (score > hiscore)
        {
            hiscore = score;
            SaveHiScore();
            LoadHiScore();
            //UIManager.instance.OnHiScoreChanged(hiscore);
        }
    }

    void SaveHiScore()
    {
        GradiusSave save = new GradiusSave();
        save.hiscore = hiscore;

        FileStream fs = File.Create(Application.persistentDataPath + "/" + "gradius.sav");

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, save);

        fs.Close();
    }

    void LoadHiScore()
    {
        if(File.Exists(Application.persistentDataPath + "/" + "gradius.sav"))
        {
            FileStream fs = File.Open(Application.persistentDataPath + "/" + "gradius.sav", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            GradiusSave sav = (GradiusSave)bf.Deserialize(fs);
            hiscore = sav.hiscore;
            UIManager.instance.OnHiScoreChanged(hiscore);
        }
    }
}
