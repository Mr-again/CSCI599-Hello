using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public int starNum;
    
    public Button home;
    public Button item1;

    public string scores;

    public int score = 0;

    public Text diamonds;



    private string tempItem;
    private void Awake()
    {
        //home = GetComponent<Button>();
        //diamonds = GetComponent<Text>();
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
        SceneManager.LoadScene("GamePlay");
    }
}