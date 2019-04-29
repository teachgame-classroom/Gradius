using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas").GetComponentInChildren<UIManager>();
            }
            return _instance;
        }
    }
    private static UIManager _instance;

    private string[] weaponNames = new string[] { "Speedup", "Missile", "Double", "Laser", "Option", "Barrier" };

    private Image[] speedupImages;
    private Image[] missileImages;
    private Image[] doubleImages;
    private Image[] laserImages;
    private Image[] optionImages;
    private Image[] barrierImages;

    private Text lifeText;
    private Text nameText;
    private Text scoreText;
    private Text hiscoreText;

    private int[] weaponStates = new int[6];
    private KeyCode[] testKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };

    private int currentPowerupPanelIdx = -1;
    private int testPowerup = 0;

    // Awake 是比 Start 更早执行的初始化方法
    void Awake()
    {
        speedupImages = GetWeaponImages(weaponNames[0]);
        missileImages = GetWeaponImages(weaponNames[1]);
        doubleImages = GetWeaponImages(weaponNames[2]);
        laserImages = GetWeaponImages(weaponNames[3]);
        optionImages = GetWeaponImages(weaponNames[4]);
        barrierImages = GetWeaponImages(weaponNames[5]);

        // 查找子对象的第一种写法
        lifeText = transform.Find("StatusPanel").Find("Life").GetComponent<Text>();

        // 查找子对象的第二种写法
        nameText = transform.Find("StatusPanel/PlayerName").GetComponent<Text>();

        scoreText = transform.Find("StatusPanel/Score").GetComponent<Text>();
        hiscoreText = transform.Find("StatusPanel/HiScore").GetComponent<Text>();

        OnLifeChanged(99);
        OnPlayerNameChanged("CGWANG");
        OnScoreChanged(12345);
        OnHiScoreChanged(98765);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < testKeys.Length; i++)
        {
            if(Input.GetKeyDown(testKeys[i]))
            {
                Debug.Log("按下了" + testKeys[i]);
                weaponStates[i]++;
                if(weaponStates[i] > 2)
                {
                    weaponStates[i] = 0;
                }

                ChangeWeaponPanelState(i, weaponStates[i]);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            testPowerup++;
            if(testPowerup > 6)
            {
                testPowerup = 0;
            }
            OnPowerupChanged(testPowerup);
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            OnPowerup();
        }
    }

    public void OnPowerup()
    {
        ChangeWeaponPanelState(currentPowerupPanelIdx, 2);
    }

    Image[] GetWeaponImages(string weaponName)
    {
        Image[] images = new Image[2];

        Transform weaponTrans = transform.Find("WeaponPanel/" + weaponName);

        images[0] = weaponTrans.GetChild(0).GetComponent<Image>();
        images[1] = weaponTrans.GetChild(1).GetComponent<Image>();

        return images;
    }

    public void ChangeWeaponPanelState(int weaponId, int stateId)
    {
        Debug.Log("武器" + weaponId + "的状态变更为：" + stateId);

        weaponId = Mathf.Clamp(weaponId, 0, 5);
        weaponStates[weaponId] = stateId;

        switch(weaponId)
        {
            case 0:
                ToggleWeaponImage(speedupImages, stateId);
                break;
            case 1:
                ToggleWeaponImage(missileImages, stateId);
                break;
            case 2:
                ToggleWeaponImage(doubleImages, stateId);
                break;
            case 3:
                ToggleWeaponImage(laserImages, stateId);
                break;
            case 4:
                ToggleWeaponImage(optionImages, stateId);
                break;
            case 5:
                ToggleWeaponImage(barrierImages, stateId);
                break;
            default:
                break;
        }
    }

    void ToggleWeaponImage(Image[] images, int stateId)
    {
        switch(stateId)
        {
            case 0: // 显示普通图标，关闭高亮和空白图标
                images[0].enabled = false;
                images[1].enabled = false;
                Debug.Log("0:" + images[0].enabled + "," + images[1].enabled);
                break;
            case 1: // 显示高亮图标，关闭空白图标
                images[0].enabled = true;
                images[1].enabled = false;
                Debug.Log("1:" + images[0].enabled + "," + images[1].enabled);
                break;
            case 2: // 显示空白图标，关闭高亮图标
                images[0].enabled = false;
                images[1].enabled = true;
                Debug.Log("2:" + images[0].enabled + "," + images[1].enabled);
                break;
            default: // 状态编号如果不是0，1，2，不做任何操作
                break;
        }
    }

    // 玩家对象通过这个方法通知UI“强化数量发生了变化”
    public void OnPowerupChanged(int powerupLevel)
    {
        // 强化数量的有效范围是0 - 6
        powerupLevel = Mathf.Clamp(powerupLevel, 0, 6);
        int newPowerupPanelIdx = powerupLevel - 1;

        // 如果当前已有强化被激活，复位当前强化显示
        if (currentPowerupPanelIdx > -1)
        {
            if(weaponStates[currentPowerupPanelIdx] == 1)
            {
                ChangeWeaponPanelState(currentPowerupPanelIdx, 0);
            }
        }

        // 如果新的强化等级大于0
        if (powerupLevel > 0)
        {
            // 并且该强化未被装备，就激活新的强化显示
            if (weaponStates[newPowerupPanelIdx] == 0)
            {
                ChangeWeaponPanelState(newPowerupPanelIdx, 1);
            }
        }

        currentPowerupPanelIdx = powerupLevel - 1;
    }

    public void OnLifeChanged(int life)
    {
        lifeText.text = life.ToString();
    }

    public void OnPlayerNameChanged(string playerName)
    {
        nameText.text = playerName;
    }

    public void OnScoreChanged(int score)
    {
        scoreText.text = score.ToString("D7");
    }

    public void OnHiScoreChanged(int hiscore)
    {
        hiscoreText.text = hiscore.ToString("D7");
    }

    public void OnGameOver()
    {
        Debug.Log("Game Over");
    }
}
