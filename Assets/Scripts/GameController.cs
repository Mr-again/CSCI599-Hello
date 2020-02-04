using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public int top_level=0;
    public int cur_level = 0;
    public List<int> level_scores = new List<int>();

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
        if (PlayerPrefs.HasKey("top_level"))
        {
            top_level = PlayerPrefs.GetInt("top_level");
        }
        for(int i = 0; i < top_level; i++)
        {
            string str = "level_score_" + i;
            if (PlayerPrefs.HasKey("str"))
            {
                level_scores.Add(PlayerPrefs.GetInt("str"));
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
}
