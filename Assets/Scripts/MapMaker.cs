using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapMaker : MonoBehaviour
{
    public Button wall;
    public Button brown_box;


    // Start is called before the first frame update
    void Start()
    {
        wall.onClick.AddListener(() => { OnClickElement(); });
        brown_box.onClick.AddListener(() => { OnClickElement(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnClickElement()
    {
        var selected_element = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log("Click On Element: "+selected_element.name);

    }
}