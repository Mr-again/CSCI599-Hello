using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    //public DeviceType deviceType = SystemInfo.deviceType;

    public int top_level = 0;
    public int cur_level = 0;
    public int total_star = 0;
    public int[] level_scores;

    public int cur_community = 1;
    public int cur_slot_index = 0;
    public int cur_one_star_step = -1;
    public int cur_two_star_step = -1;
    public int cur_three_star_step = -1;
    public int[] cur_threshhold = { 0, 0, 0 };
    public string cur_level_name = "";
    public string cur_map_maker_id = "Default";
    public string cur_maker_id = "Default";
    public int cur_community_maps_page = 1;

    public int gameplay_enetrance;
    public string target_map_json;

    public string target_map_id;

    public int[][] thresholds;

    public int[] unlock_requires;

    public Currency currency;

    public PlayHistory playHistory;

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

        if(SystemInfo.deviceType== DeviceType.Desktop)
        {
            //PlayerPrefs.DeleteAll();
        }
        cur_maker_id = SystemInfo.deviceUniqueIdentifier;
        if (!PlayerPrefs.HasKey("Hint1"))
        {
            PlayerPrefs.SetInt("Hint1", 1);
        }

        if (!PlayerPrefs.HasKey("Hint2"))
        {
            PlayerPrefs.SetInt("Hint2", 1);
        }

        if (!PlayerPrefs.HasKey("Hint3"))
        {
            PlayerPrefs.SetInt("Hint3", 1);
        }
    }

    void Start()
    {
        //deal with currency
        this.currency = new Currency();

        //deal with play history
        this.playHistory = new PlayHistory();
        


        level_scores[0] = 0;

        for(int i = 1; i < 24; i++)
        {
            level_scores[i] = -1;//未解锁
        }

        if (PlayerPrefs.HasKey("total_star"))
        {
            total_star = PlayerPrefs.GetInt("total_star");
        }
        else
        {
            PlayerPrefs.SetInt("total_star", 0);
        }

        if (PlayerPrefs.HasKey("top_level"))
        {
            top_level = PlayerPrefs.GetInt("top_level");
        }
        else
        {
            top_level = 0;
            PlayerPrefs.SetInt("top_level", 0);
            PlayerPrefs.SetInt("level_score_0", 0);//解锁
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
        levelUnlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void levelPass(int star)
    {
        int history_star = level_scores[cur_level];
        if (star > history_star)
        {
            total_star = total_star + star - history_star;
            PlayerPrefs.SetInt("total_star", total_star);
            level_scores[cur_level] = star;
            PlayerPrefs.SetInt("level_score_" + cur_level, star);
        }
        if (top_level == cur_level)
        {
            levelUnlock();
        }
    }
    private void levelUnlock()
    {
        for(int i = 0; i < level_scores.Length; i++)
        {
            if (level_scores[i] == -1)
            {
                if (unlock_requires[i] <= total_star)
                {
                    level_scores[i] = 0;
                    PlayerPrefs.SetInt("level_score_"+i, 0);
                    top_level = i;
                    PlayerPrefs.SetInt("top_level", i);
                }
            }
        }
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
            this.unlock_requires = new int[lines.Length];
            for(int i= 0;i < lines.Length;i++)
            {
                string[] lineArr = lines[i].Split(' ');
                int[] threshold = new int[3];
                int unlock_require = int.Parse(lineArr[0]);
                threshold[0] = int.Parse(lineArr[1]);
                threshold[1] = int.Parse(lineArr[2]);
                threshold[2] = int.Parse(lineArr[3]);
                thresholds[i] = threshold;
                unlock_requires[i] = unlock_require;
            }
        }
    }
}

public class Currency
{
    private int money;

    public Currency()
    {
        if (PlayerPrefs.HasKey("currency"))
        {
            this.money = PlayerPrefs.GetInt("currency");
        }
        else
        {
            this.money = 100;
            PlayerPrefs.SetInt("currency", this.money);
        }
        Debug.Log("Current money: " + this.money);
    }

    public bool ReleaseMap()
    {
        if (this.money >= 100)
        {
            this.money -= 100;
            PlayerPrefs.SetInt("currency", this.money);
            Analytics.CustomEvent("spend_money", new Dictionary<string, object>{
                {"amount", 100 },
                {"source", "release_map" },
                {"session_id", AnalyticsSessionInfo.sessionId },
                {"user_id", AnalyticsSessionInfo.userId  }
            });
            return true;
        }
        return false;
    }

    public int GetMoney()
    {
        return this.money;
    }

    public void PassLevel(int star)
    {
        Debug.Log("star "+star);
        if (star <= 0) return;
        this.money += 10 * star;
        Debug.Log("money " + this.money);
        PlayerPrefs.SetInt("currency", this.money);
        Analytics.CustomEvent("earn_money", new Dictionary<string, object>{
            {"amount", 10 * star },
            {"source", "pass_level" },
            {"session_id", AnalyticsSessionInfo.sessionId },
            {"user_id", AnalyticsSessionInfo.userId  }
        });
    }
}

public class PlayHistory
{
    int history_len;
    Dictionary<string, int[]> dict;//key:id  value:{index of local storage, star}

    public PlayHistory()
    {
        history_len = 0;
        if (PlayerPrefs.HasKey("history_len"))
        {
            history_len = PlayerPrefs.GetInt("history_len");
        }
        dict = new Dictionary<string, int[]>();
        for (int i = 0; i < history_len; i++)
        {
            string[] history_map = PlayerPrefs.GetString("history_map_" + i).Split(';');//key:idx   value:"id;star"
            int star = int.Parse(history_map[1]);
            int[] tmp = new int[2];
            tmp[0] = i;
            tmp[1] = star;
            dict.Add(history_map[0], tmp);
        }
    }

    public void AddHistory(string id, int star)
    {
        int[] before = GetHistory(id);
        if (before[0] == -1)
        {
            dict.Add(id, new int[] { this.history_len, star });
            PlayerPrefs.SetString("history_map_" + this.history_len, id + ";" + star);
            this.history_len++;
            PlayerPrefs.SetInt("history_len", this.history_len);
        }
        else if (before[1] < star)
        {
            dict[id][1] = star;
            PlayerPrefs.SetString("history_map_" + dict[id][0], id + ";" + star);
        }
        
    }
    //return star
    public int[] GetHistory(string id)
    {
        if (!dict.ContainsKey(id))
        {
            return new int[] { -1, 0 };
        }
        return dict[id];
    }
}
