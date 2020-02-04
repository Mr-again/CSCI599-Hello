using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    }
    void OnClickCommunity()
    {
        Debug.Log("Click On Community");
        SceneManager.LoadScene("Community");
    }
    void OnClickLeaderBoard()
    {
        Debug.Log("Click On Leader Board");
        SceneManager.LoadScene("LeaderBoard");
    }
}
