using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class WinPanelController : MonoBehaviour
{
    public GameController gameController;
    public MapCreator mapCreator;

    public Button next_button;
    public Button replay_button;
    public Button return_button;

    public GameObject star_1;
    public GameObject star_2;
    public GameObject star_3;

    public int final_step = 0;
    public int star = 0;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        mapCreator= FindObjectOfType<MapCreator>();

        Analytics.CustomEvent("level_finish", new Dictionary<string, object>
        {
            {"level_index",gameController.cur_level },
            {"session_id" ,AnalyticsSessionInfo.sessionId },
            {"user_id" ,AnalyticsSessionInfo.userId  },
            {"steps", final_step },
            {"time_elapsed", Time.realtimeSinceStartup - AnalyticsHelper.time_startPlayingLevel },
            {"tries", AnalyticsHelper.GetTries(gameController.cur_level) }
        });
        AnalyticsHelper.ResetTries(gameController.cur_level);
    }
    // Start is called before the first frame update
    void Start()
    {
        next_button.onClick.AddListener(() => { onClickNextButton(); });
        replay_button.onClick.AddListener(() => { onClickReplayButton(); });
        return_button.onClick.AddListener(() => { onClickReturnButton(); });
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void onClickNextButton()
    {
        Debug.Log("Click On Next");
        Debug.Log(final_step);

        int star = GetStar();

        gameController.levelPass(star);
        if (gameController.top_level == gameController.cur_level)
        {
            SceneManager.LoadScene("LevelPage");
            return;
        }
        gameController.cur_level++;
        gameController.gameplay_enetrance = 0;
        SceneManager.LoadScene("GamePlay");
    }
    void onClickReplayButton()
    {
        Debug.Log("Click On Replay");
        int star = GetStar();
        gameController.levelPass(star);
        gameController.gameplay_enetrance = 0;
        SceneManager.LoadScene("GamePlay");
    }
    void onClickReturnButton()
    {
        Debug.Log("Click On Return");
        int star = GetStar();
        gameController.levelPass(star);
        SceneManager.LoadScene("LevelPage");
    }
    public int GetStar()
    {
        int[] threshold = gameController.thresholds[gameController.cur_level];
        if (final_step < threshold[0])
        {
            return 3;
        }
        else if (final_step < threshold[1])
        {
            return 2;
        }
        else if (final_step < threshold[2])
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public void ShowStar()
    {
        Debug.Log(star);
        if (star == 0)
            return;
        if (star == 1)
        {
            star_1.SetActive(true);
            star_2.SetActive(false);
            star_3.SetActive(false);
            return;
        }
        if (star == 2)
        {
            star_1.SetActive(true);
            star_2.SetActive(true);
            star_3.SetActive(false);
            return;
        }
        if (star == 3)
        {
            star_1.SetActive(true);
            star_2.SetActive(true);
            star_3.SetActive(true);
            return;
        }
    }
}
