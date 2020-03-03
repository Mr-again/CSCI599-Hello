using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameController gameController;
    public MapCreator mapCreator;

    public Button back_button;
    public Button replay_button;
    public Button cancel_button;

    public int final_step = 0;
    public int star = 0;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        mapCreator = FindObjectOfType<MapCreator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        back_button.onClick.AddListener(() => { onClickBackButton(); });
        replay_button.onClick.AddListener(() => { onClickReplayButton(); });
        cancel_button.onClick.AddListener(() => { onClickCancelButton(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void onClickReplayButton()
    {
        Debug.Log("Click On Replay");
        SceneManager.LoadScene("GamePlay");
    }

    private void onClickBackButton()
    {
        Debug.Log("Click On Back");
        SceneManager.LoadScene("LevelPage");
    }
    private void onClickCancelButton()
    {
        Debug.Log("Click On Cancel");
        this.gameObject.SetActive(false);
    }

}
