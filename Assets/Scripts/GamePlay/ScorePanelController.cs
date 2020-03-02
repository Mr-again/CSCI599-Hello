using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScorePanelController : MonoBehaviour
{
    public GameObject menu_panel;
    public Button menu_button;
    // Start is called before the first frame update
    void Start()
    {
        menu_button.onClick.AddListener(() => { onClickMenuButton(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onClickMenuButton()
    {
        menu_panel.SetActive(true);
    }
}
