using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class EntranceController : MonoBehaviour
{
    public Button stage_mode;
    public Button community;
    public Button leader_board;

    // Start is called before the first frame update
    void Start()
    {
        stage_mode.onClick.AddListener(() => { OnClickStageMode(); });
        community.onClick.AddListener(() => { OnClickCommunity(); });
        leader_board.onClick.AddListener(() => { OnClickLeaderBoard(); });
    }

    // Update is called once per frame
    void Update()
    {

    }       
    

    
    void OnClickStageMode()
    {

       
        Debug.Log("Click On Stage Mode");
        SceneManager.LoadScene("LevelPage");
        /*Analytics.CustomEvent("StageModeTime", new Dictionary<string, object> {
                    {"user_id" ,AnalyticsSessionInfo.userId},
                    {"times", AnalyticsSessionInfo.sessionCount}
            });
        Debug.Log(Analytics.enabled);
        Debug.Log(Analytics.IsCustomEventEnabled("StageModeTime"));*/



    }

    void OnClickCommunity()
    {
      

        Debug.Log("Click On Community");
        SceneManager.LoadScene("Community");
        /*Analytics.CustomEvent("CommunityTime", new Dictionary<string, object> {
                {"user_id" ,AnalyticsSessionInfo.userId},
                {"times", AnalyticsSessionInfo.sessionCount}
            });
        Debug.Log(Analytics.enabled);
        Debug.Log(Analytics.IsCustomEventEnabled("CommunityTime"));*/


    }
    void OnClickLeaderBoard()
    {
     
        Debug.Log("Click On Leader Board");
        //SceneManager.LoadScene("LeaderBoard");
        /*Analytics.CustomEvent("LeaderBoardTime", new Dictionary<string, object> {
                    {"user_id" ,AnalyticsSessionInfo.userId},
                    {"times", AnalyticsSessionInfo.sessionCount}
            });
        Debug.Log(Analytics.enabled);
        Debug.Log(Analytics.IsCustomEventEnabled("LeaderBoardTime"));*/


    }
}
