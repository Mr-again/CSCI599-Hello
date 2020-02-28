using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    //public DeviceType deviceType = SystemInfo.deviceType;

    public int top_level = 0;
    public int cur_level = 0;
    public int total_star = 0;
    public int[] level_scores = new int[24];

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        level_scores[0] = 0;

        for(int i = 1; i < 24; i++)
        {
            level_scores[i] = -1;
        }

        if (PlayerPrefs.HasKey("total_star"))
        {
            total_star = PlayerPrefs.GetInt("total_star");
        }

        if (PlayerPrefs.HasKey("top_level"))
        {
            top_level = PlayerPrefs.GetInt("top_level");
        }
        else
        {
            top_level = 0;
            PlayerPrefs.SetInt("top_level", 0);
            PlayerPrefs.SetInt("level_score_0", 0);
        }
        for(int i = 0; i <= top_level; i++)
        {
            string str = "level_score_" + i;
            if (PlayerPrefs.HasKey(str))
            {
                level_scores[i] = PlayerPrefs.GetInt(str);
            }
            else
            {
                Debug.Log("pass but fail to load level score, level = "+i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void levelPass(int star)
    {
        if (top_level == cur_level)
        {
            levelUnlock();
        }
        int history_star = level_scores[cur_level];
        if (star > history_star)
        {
            level_scores[cur_level] = star;
            PlayerPrefs.SetInt("level_score_" + cur_level, star);
        }
    }
    private void levelUnlock()
    {
        //todo
    }
}
