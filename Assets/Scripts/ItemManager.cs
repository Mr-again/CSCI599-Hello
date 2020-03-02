using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    GameController gameController;
    public int starNum;
    
    public Button home;
    public Button item1;
    public Button item2;
    public Button item3;

    public string scores;

    public int score = 0;

    public Text diamonds;

    private int[][] thresholds;
    private int[][] unlock_requires;

    private string tempItem;
    private void Awake()
<<<<<<< Updated upstream
    {
        gameController = FindObjectOfType<GameController>();
        //home = GetComponent<Button>();
        //diamonds = GetComponent<Text>();
=======
    {
        //home = GetComponent<Button>();
        //diamonds = GetComponent<Text>();
        //本地路径
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
            Debug.Log(s);
        }
>>>>>>> Stashed changes
    }


    void CreateData()
    {
        for (int i = 1; i <= 24; i++)
        {
            tempItem = "Grid/Item" + " (" + i.ToString() + ")";

            starNum = 1;
            score += starNum;
            ShowStar(starNum, tempItem);
        }
        ShowScores(score);
    }

    // 
    void ShowStar(int num, string level)
    {
        if (num == 0)
        {
            transform.Find(level + "/stars/star1").gameObject.SetActive(false);
            transform.Find(level + "/stars/star2").gameObject.SetActive(false);
            transform.Find(level + "/stars/star3").gameObject.SetActive(false);
        }
        if (num == 1)
        {
            transform.Find(level + "/stars/star1").gameObject.SetActive(true);
            transform.Find(level + "/stars/star2").gameObject.SetActive(false);
            transform.Find(level + "/stars/star3").gameObject.SetActive(false);
        }
        if (num == 2)
        {
            transform.Find(level + "/stars/star1").gameObject.SetActive(true);
            transform.Find(level + "/stars/star2").gameObject.SetActive(true);
            transform.Find(level + "/stars/star3").gameObject.SetActive(false);
        }
        if (num == 3)
        {
            transform.Find(level + "/stars/star1").gameObject.SetActive(true);
            transform.Find(level + "/stars/star2").gameObject.SetActive(true);
            transform.Find(level + "/stars/star3").gameObject.SetActive(true);
        }
    }

    void ShowScores(int number)
    {
        scores = score.ToString();
        diamonds.text = scores;
    }

    // Start is called before the first frame update
    void Start()
    {
        home.onClick.AddListener(() => { ClickHome(); });
        item1.onClick.AddListener(() => { ClickItem1(); });
        item2.onClick.AddListener(() => { ClickItem2(); });
        item3.onClick.AddListener(() => { ClickItem3(); });
        CreateData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ClickHome()
    {
        Debug.Log("Click on Home");
        SceneManager.LoadScene("Enterance");
    }
    void ClickItem1()
    {
        Debug.Log("Click on Level 1");
        gameController.cur_level = 0;
        SceneManager.LoadScene("GamePlay");
    }
    void ClickItem2()
    {
        Debug.Log("Click on Level 2");
        gameController.cur_level = 1;
        SceneManager.LoadScene("GamePlay");
    }
    void ClickItem3()
    {
        Debug.Log("Click on Level 3");
        gameController.cur_level = 2;
        SceneManager.LoadScene("GamePlay");
    }
}