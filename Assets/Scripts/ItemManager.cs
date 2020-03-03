using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    GameController gameController;
    public int starNum;

    public Button home;
    private Button[] items = new Button[24];
    private bool[] items_index = new bool[24];
    private Text[] levels = new Text[24];
    private Image[] locks = new Image[24];

    public string scores;

    public int score = 0;

    public Text diamonds;

    private string tempItem;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        //home = GetComponent<Button>();
        //diamonds = GetComponent<Text>();
        Debug.Log("top_level=" + gameController.top_level);
        Debug.Log("total_star=" + gameController.total_star);
        for (int i = 0; i < 24; i++)
        {
            items_index[i] = true;
            items[i] = GameObject.Find("Item (" + (i + 1).ToString() + ")").GetComponent<Button>();
            levels[i] = GameObject.Find("level" + (i + 1).ToString()).GetComponent<Text>();
            locks[i] = GameObject.Find("Lock" + (i + 1).ToString()).GetComponent<Image>();
        }
        for(int i = 0; i < 24; i++)
        {
            if (gameController.total_star >= gameController.unlock_requires[i])
            {
                levels[i].gameObject.SetActive(true);
                locks[i].gameObject.SetActive(false);
                items_index[i] = true;
            }
            else
            {
                levels[i].gameObject.SetActive(false);
                locks[i].gameObject.SetActive(true);
                items_index[i] = false;
            }
        }
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
        //item1.onClick.AddListener(() => { ClickItem1(); });
        //item2.onClick.AddListener(() => { ClickItem2(); });
        //item3.onClick.AddListener(() => { ClickItem3(); });
        for (int i = 0; i < 24; i++)
        {
            int index = i;
            items[i].onClick.AddListener(delegate () { this.ClickItem(index, items_index[index]); });
        }
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

    void ClickItem(int i, bool can_play)
    {
        if (can_play)
        {
            Debug.Log(i);
            Debug.Log("Click on Level " + (i + 1).ToString());
            gameController.cur_level = i;
            SceneManager.LoadScene("GamePlay");
        }
        else
        {
            Debug.Log("Level " + (i + 1).ToString() + " is unlock~");
        }
        
    }
}