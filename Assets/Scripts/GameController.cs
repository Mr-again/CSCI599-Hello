using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    //public DeviceType deviceType = SystemInfo.deviceType;

    public int top_level = 0;
    public int cur_level = 0;
    public int total_star = 0;
    public int[] level_scores;

    private int[][] thresholds;
    private int[][] unlock_requires;

    // Start is called before the first frame update
    private void Awake()
    {
        level_scores = new int[24];
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        DealWithLevels();

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

    private void DealWithLevels()
    {
        var fileAddress = Path.Combine(Application.streamingAssetsPath, "Levels/thresholds.txt");
        FileInfo fInfo0 = new FileInfo(fileAddress);
        if (fInfo0.Exists)
        {
            StreamReader r = new StreamReader(fileAddress);
    //StreamReader默认的是UTF8的不需要转格式了，因为有些中文字符的需要有些是要转的，下面是转成String代码
    //byte[] data = new byte[1024];
    // data = Encoding.UTF8.GetBytes(r.ReadToEnd());
    // s = Encoding.UTF8.GetString(data, 0, data.Length);
            string s = r.ReadToEnd();
            string[] lines = s.Split('\n');
            this.thresholds = new int[lines.Length][];
            this.unlock_requires = new int[lines.Length][];
            for(int i= 0;i < lines.Length;i++)
            {
                string[] lineArr = lines[i].Split(' ');
                int[] threshold = new int[3];
                int[] unlock_require = new int[2];
                unlock_require[0] = int.Parse(lineArr[0]);
                unlock_require[1] = int.Parse(lineArr[1]);
                threshold[0] = int.Parse(lineArr[2]);
                threshold[1] = int.Parse(lineArr[3]);
                threshold[2] = int.Parse(lineArr[4]);
                thresholds[i] = threshold;
                unlock_requires[i] = unlock_require;
            }
        }
    }
}
