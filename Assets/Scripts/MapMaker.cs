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
    public Button player;

    public GameObject player_obj;
    public GameObject wall_obj;
    public GameObject box_brown_obj;
    public GameObject box_red_obj;
    public GameObject box_blue_obj;
    public GameObject box_green_obj;
    public GameObject box_gray_obj;
    public GameObject target_brown_obj;
    public GameObject target_red_obj;
    public GameObject target_blue_obj;
    public GameObject target_green_obj;
    public GameObject target_gray_obj;
    public GameObject floor_stone_obj;
    public GameObject floor_mud_obj;
    public GameObject floor_ice_obj;
    public GameObject pit_obj;

    int mouse_x;
    int mouse_y;

    int offset_x = 8;
    int offset_y = 4;

    public string select_elem_name;

    public string[] map = new string[] {
        "############",
        "############",
        "############",
        "############",
        "############",
        "############",
        "############",
        "############",
        "############",};
    public int[] player_position = new int[2];
    public string[] box_position;
    public string[] target_position;
    
    public Dictionary<int, KeyValuePair<int, GameObject>> boxes = new Dictionary<int, KeyValuePair<int, GameObject>>();
    public HashSet<int> walls = new HashSet<int>();
    public HashSet<int> stones = new HashSet<int>();
    public HashSet<int> ices = new HashSet<int>();
    public HashSet<int> muds = new HashSet<int>();
    public Dictionary<int, GameObject> pits = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> covered_pits = new Dictionary<int, GameObject>();

    public Dictionary<int, bool> targets_brown = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_red = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_blue = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_green = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_gray = new Dictionary<int, bool>();

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
        player.onClick.AddListener(() => { OnClickElement(); });

        select_elem_name = "Wall";
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
                Debug.Log("mouse position: " + mouse_x + " " + mouse_y);
                if (mouse_x >= -8 && mouse_x <= 3 && mouse_y >= -4 && mouse_y <= 4)
                {
                    GenerateAndStoreMapElement(select_elem_name, mouse_x, mouse_y);
                }
            }
            
        }
    }
    void OnClickElement()
    {
        var selected_element = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log("Click On Element: "+selected_element.name);
        select_elem_name = selected_element.name;
    }


    public void GenerateAndStoreMapElement(string elem_name, int pos_x,int pos_y)
    {
        int store_x = pos_x + offset_x;
        int store_y = pos_y + offset_y;

        switch (elem_name)
        {
            case "Player":
                {
                    GameObject newPlayer = Instantiate(player_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    newPlayer.SetActive(true);
                    player_position[0] = store_x;
                    player_position[1] = store_y;
                    break;
                }
            case "Wall":
                {
                    GameObject newWall = Instantiate(wall_obj, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    newWall.SetActive(true);
                    walls.Add(100 * store_y + store_x);
                    break;
                }
            case "Brown_Box":
                {
                    GameObject newBox = Instantiate(box_brown_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    boxes.Add(100 * store_y + store_x, new KeyValuePair<int, GameObject>(0, newBox));
                    newBox.SetActive(true);
                    break;
                }
            case "Red_Box":
                {
                    GameObject newBox = Instantiate(box_red_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    boxes.Add(100 * store_y + store_x, new KeyValuePair<int, GameObject>(1, newBox));
                    newBox.SetActive(true);
                    break;
                }
            case "Blue_Box":
                {
                    GameObject newBox = Instantiate(box_blue_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    boxes.Add(100 * store_y + store_x, new KeyValuePair<int, GameObject>(2, newBox));
                    newBox.SetActive(true);
                    break;
                }
            case "Green_Box":
                {
                    GameObject newBox = Instantiate(box_green_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    boxes.Add(100 * store_y + store_x, new KeyValuePair<int, GameObject>(3, newBox));
                    newBox.SetActive(true);
                    break;
                }
            case "Gray_Box":
                {
                    GameObject newBox = Instantiate(box_gray_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    boxes.Add(100 * store_y + store_x, new KeyValuePair<int, GameObject>(4, newBox));
                    newBox.SetActive(true);
                    break;
                }
            case "Brown_Tar":
                {
                    GameObject newTarget = Instantiate(target_brown_obj, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    targets_brown.Add(100 * store_y + store_x, false);
                    newTarget.SetActive(true);
                    break;
                }
            case "Red_Tar":
                {
                    GameObject newTarget = Instantiate(target_red_obj, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    targets_red.Add(100 * store_y + store_x, false);
                    newTarget.SetActive(true);
                    break;
                }
            case "Blue_Tar":
                {
                    GameObject newTarget = Instantiate(target_blue_obj, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    targets_blue.Add(100 * store_y + store_x, false);
                    newTarget.SetActive(true);
                    break;
                }
            case "Green_Tar":
                {
                    GameObject newTarget = Instantiate(target_green_obj, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    targets_green.Add(100 * store_y + store_x, false);
                    newTarget.SetActive(true);
                    break;
                }
            case "Gray_Tar":
                {
                    GameObject newTarget = Instantiate(target_gray_obj, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    targets_gray.Add(100 * store_y + store_x, false);
                    newTarget.SetActive(true);
                    break;
                }
            case "Stone_Floor":
                {
                    GameObject newFloor = Instantiate(floor_stone_obj, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    stones.Add(100 * store_y + store_x);
                    newFloor.SetActive(true);
                    break;
                }
            case "Mud_Floor":
                {
                    GameObject newFloor = Instantiate(floor_mud_obj, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    muds.Add(100 * store_y + store_x);
                    newFloor.SetActive(true);
                    break;
                }
            case "Ice_Floor":
                {
                    GameObject newFloor = Instantiate(floor_ice_obj, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    ices.Add(100 * store_y + store_x);
                    newFloor.SetActive(true);
                    break;
                }
            case "Pit":
                {
                    GameObject newPit = Instantiate(pit_obj, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    newPit.SetActive(true);
                    pits.Add(100 * store_y + store_x, newPit);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}