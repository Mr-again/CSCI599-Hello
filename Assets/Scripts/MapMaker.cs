using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    public GameObject pit_brown_obj;
    public GameObject pit_red_obj;
    public GameObject pit_blue_obj;
    public GameObject pit_green_obj;
    public GameObject pit_gray_obj;

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
    public int[] player_position = { -100, -100 };
    public string[] box_position;
    public string[] target_position;
    
    //public Dictionary<int, KeyValuePair<int, GameObject>> boxes = new Dictionary<int, KeyValuePair<int, GameObject>>();
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

    
    // the holder includes walls, pits
    public Dictionary<int, GameObject> holder = new Dictionary<int, GameObject>();
    
    // the layer0 includes stones, ices, muds
    public Dictionary<int, GameObject> layer0 = new Dictionary<int, GameObject>();
    
    // the layer1 includes boxes, targets
    public Dictionary<int, GameObject> boxes = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> targets = new Dictionary<int, GameObject>();

    private GameObject selected_element;
    private bool is_player_exist = false;
    private GameObject existed_player_obj = null;
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
        selected_element = wall_obj;


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
                //Debug.Log("mouse position: " + mouse_x + " " + mouse_y);
                if (mouse_x >= -8 && mouse_x <= 3 && mouse_y >= -4 && mouse_y <= 4)
                {
                    
                    GenerateAndStoreMapElement(select_elem_name, selected_element, mouse_x, mouse_y);
                }
            }
            
        }
    }
    void OnClickElement()
    {
        var selected_button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log("Click On Element: "+selected_button.name);
        select_elem_name = selected_button.name;
        switch (select_elem_name)
        {
            case "Player":
                {
                    selected_element = player_obj;
                    break;
                }
            case "Wall":
                {
                    selected_element = wall_obj;
                    break;
                }
            case "Brown_Box":
                {
                    selected_element = box_brown_obj;
                    break;
                }
            case "Red_Box":
                {
                    selected_element = box_red_obj;
                    break;
                }
            case "Blue_Box":
                {
                    selected_element = box_blue_obj;
                    break;
                }
            case "Green_Box":
                {
                    selected_element = box_green_obj;
                    break;
                }
            case "Gray_Box":
                {
                    selected_element = box_gray_obj;
                    break;
                }
            case "Brown_Tar":
                {
                    selected_element = target_brown_obj;
                    break;
                }
            case "Red_Tar":
                {
                    selected_element = target_red_obj;
                    break;
                }
            case "Blue_Tar":
                {
                    selected_element = target_blue_obj;
                    break;
                }
            case "Green_Tar":
                {
                    selected_element = target_green_obj;
                    break;
                }
            case "Gray_Tar":
                {
                    selected_element = target_gray_obj;
                    break;
                }
            case "Stone_Floor":
                {
                    selected_element = floor_stone_obj;
                    break;
                }
            case "Mud_Floor":
                {
                    selected_element = floor_mud_obj;
                    break;
                }
            case "Ice_Floor":
                {
                    selected_element = floor_ice_obj;
                    break;
                }
            case "Pit":
                {
                    selected_element = pit_obj;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    public bool isHolder(string elem_name)
    {
        return (elem_name == "Wall" || elem_name == "Pit");
    }

    public bool isHolder(int x, int y)
    {
        return holder.ContainsKey(100 * y + x);
    }

    public bool isLayer0(string elem_name)
    {
            return (elem_name == "Stone_Floor" || elem_name == "Ice_Floor" || elem_name == "Mud_Floor");
    }

    public bool isLayer0(int x, int y)
    {
        return layer0.ContainsKey(100 * y + x);
    }

    public bool isBox(string elem_name)
    {
        return (elem_name == "Brown_Box" || elem_name == "Red_Box" || elem_name == "Blue_Box"
            || elem_name == "Green_Box" || elem_name == "Gray_Box");
    }

    public bool isBox(int x, int y)
    {
        return boxes.ContainsKey(100 * y + x);
    }

    public bool isTarget(string elem_name)
    {
        return (elem_name == "Brown_Tar" || elem_name == "Red_Tar" || elem_name == "Blue_Tar"
            || elem_name == "Green_Tar" || elem_name == "Gray_Tar");
    }

    public bool isTarget(int x, int y)
    {
        return targets.ContainsKey(100 * y + x);
    }

    public bool isPlayer(int x, int y)
    {
        return player_position[0] == x && player_position[1] == y;
    }

    public void GenerateAndStoreMapElement(string elem_name, GameObject element, int pos_x, int pos_y)
    {
        int store_x = pos_x + offset_x;
        int store_y = pos_y + offset_y;


        
        bool new_obj_is_holder = isHolder(elem_name);
        bool new_obj_is_layer0 = isLayer0(elem_name);
        bool new_obj_is_box = isBox(elem_name);
        bool new_obj_is_target = isTarget(elem_name);

        bool old_obj_is_holder = isHolder(store_x, store_y);
        bool old_obj_is_layer0 = isLayer0(store_x, store_y);
        bool old_obj_is_box = isBox(store_x, store_y);
        bool old_obj_is_target = isTarget(store_x, store_y);
        bool old_obj_is_player = isPlayer(store_x, store_y);

        if (elem_name == "Player")
        {
            if (old_obj_is_holder)
            {
                return;
            }
            if(old_obj_is_box)
            {
                return;
            }
            if (!old_obj_is_layer0)
            {
                Debug.Log("There must be a floor before putting " + elem_name);
                return;
            }
            
            if (is_player_exist)
            {
                Debug.Log("Some player exist");
                Destroy(existed_player_obj);
            }
            GameObject newPlayer = Instantiate(player_obj, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
            newPlayer.SetActive(true);
            player_position[0] = store_x;
            player_position[1] = store_y;
            is_player_exist = true;
            existed_player_obj = newPlayer;
            return;
        }
        else if (new_obj_is_holder)
        {
            if (old_obj_is_player)
            {
                Debug.Log("Cannot put " + elem_name + " on player!");
                return;
            }
        }




        int kind = Convert.ToInt32(new_obj_is_holder) * 128
            + Convert.ToInt32(new_obj_is_layer0) * 64
            + Convert.ToInt32(new_obj_is_target) * 32
            + Convert.ToInt32(new_obj_is_box) * 16
            + Convert.ToInt32(old_obj_is_holder) * 8
            + Convert.ToInt32(old_obj_is_layer0) * 4
            + Convert.ToInt32(old_obj_is_target) * 2
            + Convert.ToInt32(old_obj_is_box) * 1;

        switch (kind)
        {
            case 16:
                {
                    Debug.Log("There must be a floor before putting " + elem_name);
                    break;
                }
            case 20:
                {
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 21:
                {
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 22:
                {
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 23:
                {
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 24:
                {
                    Debug.Log(elem_name + " cannot be put on walls or pits.");
                    break;
                }
            case 32:
                {
                    Debug.Log("There must be a floor before putting " + elem_name);
                    break;
                }
            case 36:
                {
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 37:
                {
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 38:
                {
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 39:
                {
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 40:
                {
                    Debug.Log(elem_name + " cannot be put on walls or pits.");
                    break;
                }
            case 64:
                {
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 68:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 69:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 70:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 71:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 72:
                {
                    GameObject old_holder = holder[store_y * 100 + store_x];
                    Destroy(old_holder);
                    holder.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 128:
                {
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    holder.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 132:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    holder.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 133:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    holder.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 134:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    holder.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 135:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    holder.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 136:
                {
                    GameObject old_holder = holder[store_y * 100 + store_x];
                    Destroy(old_holder);
                    holder.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    holder.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            default:
                {
                    Debug.Log("Someting Went Wrong --- " + elem_name + " " + kind.ToString());
                    return;
                }
        }
    }
}