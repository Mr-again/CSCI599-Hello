using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanelController : MonoBehaviour
{
    public GameController gameController;
    public MapCreator mapCreator;

    public Button next_button;
    public Button replay_button;
    public Button return_button;

    public int final_step = 0;
    public int star = 0;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        mapCreator= FindObjectOfType<MapCreator>();
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
        gameController.levelPass(star);
        //todo
        SceneManager.LoadScene("GamePlay");
    }
    void onClickReplayButton()
    {
        Debug.Log("Click On Replay");
        gameController.levelPass(star);
        //todo
        SceneManager.LoadScene("GamePlay");
    }
    void onClickReturnButton()
    {
        Debug.Log("Click On Return");
        gameController.levelPass(star);
        //todo
        SceneManager.LoadScene("LevelPage");
    }
}
