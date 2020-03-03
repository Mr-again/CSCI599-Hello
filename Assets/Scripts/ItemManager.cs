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
    public Button item1;
    public Button item2;
    public Button item3;

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
        //for(int i = 0; i < 24; i++)
        //{

        //}
        int index = 0;
        foreach (Transform i in this.transform)
        {
            if (i.gameObject.name == "Grid")
            {
                GameObject grid = i.gameObject;
                foreach (Transform j in grid.transform)
                {
                    GameObject item = j.gameObject;
                    Debug.Log(gameController.total_star.ToString() + " " + index.ToString() + " "
                        + gameController.unlock_requires[index].ToString());
                    if (gameController.total_star >= gameController.unlock_requires[index])
                    {
                        foreach (Transform k in item.transform)
                        {
                            if(k.gameObject.name[0] == 'l')
                            {
                                k.gameObject.SetActive(true);
                            }
                            else if(k.gameObject.name[0] == 'L')
                            {
                                k.gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        foreach (Transform k in item.transform)
                        {
                            if (k.gameObject.name[0] == 'l')
                            {
                                k.gameObject.SetActive(false);
                            }
                            else if (k.gameObject.name[0] == 'L')
                            {
                                k.gameObject.SetActive(true);
                            }
                        }
                    }
                    index += 1;
                }
                break;
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