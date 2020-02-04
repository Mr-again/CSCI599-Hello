using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public Button stage_mode;

    // Start is called before the first frame update
    void Start()
    {
        stage_mode.onClick.AddListener(() => { OnClickStageMode(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickStageMode()
    {
        Debug.Log(111);
        SceneManager.LoadScene("GamePlay");
    }
}
