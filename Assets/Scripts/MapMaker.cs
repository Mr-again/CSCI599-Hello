using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapMaker : MonoBehaviour
{
    public Button wall;
    public Button brown_box;
    public Button red_box;
    public Button blue_box;
    public Button green_box;
    public Button gray_box;
    public Button pit;
    public Button brown_tar;
    public Button red_tar;
    public Button blue_tar;
    public Button green_tar;
    public Button gray_tar;
    public Button stone_floor;
    public Button mud_floor;
    public Button ice_floor;

    int mouse_x;
    int mouse_y;

    int offset_x = 8;
    int offset_y = 4;

    public string select_elem_name;


    // Start is called before the first frame update
    void Start()
    {   
        wall.onClick.AddListener(() => { OnClickElement(); });
        brown_box.onClick.AddListener(() => { OnClickElement(); });
        red_box.onClick.AddListener(() => { OnClickElement(); });
        blue_box.onClick.AddListener(() => { OnClickElement(); });
        green_box.onClick.AddListener(() => { OnClickElement(); });
        gray_box.onClick.AddListener(() => { OnClickElement(); });
        pit.onClick.AddListener(() => { OnClickElement(); });
        brown_tar.onClick.AddListener(() => { OnClickElement(); });
        red_tar.onClick.AddListener(() => { OnClickElement(); });
        blue_tar.onClick.AddListener(() => { OnClickElement(); });
        green_tar.onClick.AddListener(() => { OnClickElement(); });
        gray_tar.onClick.AddListener(() => { OnClickElement(); });
        stone_floor.onClick.AddListener(() => { OnClickElement(); });
        mud_floor.onClick.AddListener(() => { OnClickElement(); });
        ice_floor.onClick.AddListener(() => { OnClickElement(); });
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePositionOnScreen = Input.mousePosition;
        mousePositionOnScreen.z = screenPosition.z;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        if (Input.GetMouseButton(0))
        {
            if (mouse_x!= Mathf.RoundToInt(mousePositionInWorld.x) || mouse_y != Mathf.RoundToInt(mousePositionInWorld.y))
            {
                mouse_x = Mathf.RoundToInt(mousePositionInWorld.x);
                mouse_y = Mathf.RoundToInt(mousePositionInWorld.y);
                Debug.Log("stored " + mouse_x + " " + mouse_y);

            }
            
        }
    }
    void OnClickElement()
    {
        var selected_element = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log("Click On Element: "+selected_element.name);
        select_elem_name = selected_element.name;
    }
}