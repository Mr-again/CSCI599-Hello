using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanelController : MonoBehaviour
{
    public Button next_button;
    public Button replay_button;
    public Button return_button;

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
        //todo
        SceneManager.LoadScene("GamePlay");
    }
    void onClickReplayButton()
    {
        Debug.Log("Click On Replay");
        //todo
        SceneManager.LoadScene("GamePlay");
    }
    void onClickReturnButton()
    {
        Debug.Log("Click On Return");
        //todo
    }
}
