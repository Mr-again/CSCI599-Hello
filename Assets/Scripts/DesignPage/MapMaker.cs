using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Json;


public class MapMaker : MonoBehaviour
{
    GameController gameController;

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
    public Button try_button;
    public Button save_button;
    public Button back_button;

    public Text alert_text;

    public InputField one_star;
    public InputField two_star;
    public InputField three_star;

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

    //TODO: here need to return serveral arrays of map data
    //public string[] map = new string[] {
    //    "############",
    //    "############",
    //    "############",
    //    "############",
    //    "############",
    //    "############",
    //    "############",
    //    "############",
    //    "############",};
    public int[] player_position = { -100, -100 };
    //public string[] box_position;
    //public string[] target_position;
    
    //public Dictionary<int, GameObject> pits = new Dictionary<int, GameObject>();
    //public Dictionary<int, GameObject> covered_pits = new Dictionary<int, GameObject>();

    //public Dictionary<int, bool> targets_brown = new Dictionary<int, bool>();
    //public Dictionary<int, bool> targets_red = new Dictionary<int, bool>();
    //public Dictionary<int, bool> targets_blue = new Dictionary<int, bool>();
    //public Dictionary<int, bool> targets_green = new Dictionary<int, bool>();
    //public Dictionary<int, bool> targets_gray = new Dictionary<int, bool>();

    
    // the walls
    public Dictionary<int, GameObject> walls = new Dictionary<int, GameObject>();

    // the pits
    public Dictionary<int, GameObject> pits = new Dictionary<int, GameObject>();

    // the layer0 includes stones, ices, muds
    public Dictionary<int, GameObject> layer0 = new Dictionary<int, GameObject>();
    
    // the layer1 includes boxes, targets
    public Dictionary<int, GameObject> boxes = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> targets = new Dictionary<int, GameObject>();

    private GameObject selected_element;
    private bool is_player_exist = false;
    private GameObject existed_player_obj = null;


    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
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
        try_button.onClick.AddListener(() => { OnClickTry(); });
        save_button.onClick.AddListener(() => { OnClickSave(); });
        back_button.onClick.AddListener(() => { OnClickBack(); });


        select_elem_name = "Wall";
        selected_element = wall_obj;
        AnalyticsHelper.time_startCreatingLevel = Time.realtimeSinceStartup;
        // TODO: publish event when player finishes creating level
        //Debug.Log(gameController.target_map_json);
        generateMapFromString();
        if(gameController.cur_one_star_step != -1)
            one_star.text = gameController.cur_one_star_step.ToString();
        if(gameController.cur_two_star_step != -1)
            two_star.text = gameController.cur_two_star_step.ToString();
        if(gameController.cur_three_star_step != -1)
            three_star.text = gameController.cur_three_star_step.ToString();
    }

    public int map_offset_X = -8;
    public int map_offset_Y = -4;

    void generateMapFromString()
    {
        if (gameController.target_map_json == "")
            return;

        JsonObject jsonObject = JsonObject.CreateFromJSON(gameController.target_map_json);

        string[] wall_position = jsonObject.wall_position;
        string[] stone_position = jsonObject.stone_position;
        string[] ice_position = jsonObject.ice_position;
        string[] mud_position = jsonObject.mud_position;
        string[] pit_position = jsonObject.pit_position;
        string[] box_position = jsonObject.box_position;
        string[] target_position = jsonObject.target_position;
        int[] player_position_inner = jsonObject.player_position;

        GameObject newPlayer = Instantiate(player_obj, new Vector3(player_position_inner[0] + map_offset_X, player_position_inner[1] + map_offset_Y, 0), Quaternion.identity);
        newPlayer.SetActive(true);
        player_position = player_position_inner;
        is_player_exist = true;
        existed_player_obj = newPlayer;

        for (int i = 0; i < wall_position.Length; i++)
        {
            string[] str_arr = wall_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newWall = Instantiate(wall_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
            walls.Add(100 * y + x, newWall);
            newWall.SetActive(true);
        }
        for (int i = 0; i < pit_position.Length; i++)
        {
            string[] str_arr = pit_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newPit = Instantiate(pit_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            pits.Add(100 * y + x, newPit);
            newPit.SetActive(true);
        }

        for (int i = 0; i < stone_position.Length; i++)
        {
            string[] str_arr = stone_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newStone = Instantiate(floor_stone_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            layer0.Add(100 * y + x, newStone);
            newStone.SetActive(true);
        }

        for (int i = 0; i < ice_position.Length; i++)
        {
            string[] str_arr = ice_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newIce = Instantiate(floor_ice_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            layer0.Add(100 * y + x, newIce);
            newIce.SetActive(true);
        }

        for (int i = 0; i < mud_position.Length; i++)
        {
            string[] str_arr = mud_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newMud = Instantiate(floor_mud_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            layer0.Add(100 * y + x, newMud);
            newMud.SetActive(true);
        }

        for (int i = 0; i < box_position.Length; i++)
        {
            string[] str_arr = box_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            int color = int.Parse(str_arr[2]);
            switch (color)
            {
                case 0:
                    {
                        GameObject newBox = Instantiate(box_brown_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, newBox);
                        newBox.SetActive(true);
                        break;
                    }
                case 1:
                    {
                        GameObject newBox = Instantiate(box_red_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, newBox);
                        newBox.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        GameObject newBox = Instantiate(box_blue_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, newBox);
                        newBox.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        GameObject newBox = Instantiate(box_green_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, newBox);
                        newBox.SetActive(true);
                        break;
                    }
                case 4:
                    {
                        GameObject newBox = Instantiate(box_gray_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, newBox);
                        newBox.SetActive(true);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        for (int i = 0; i < target_position.Length; i++)
        {
            string[] str_arr = target_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            int color = int.Parse(str_arr[2]);
            switch (color)
            {
                case 0:
                    {
                        GameObject newTarget = Instantiate(target_brown_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 3), Quaternion.identity);
                        targets.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        break;
                    }
                case 1:
                    {
                        GameObject newTarget = Instantiate(target_red_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 3), Quaternion.identity);
                        targets.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        GameObject newTarget = Instantiate(target_blue_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 3), Quaternion.identity);
                        targets.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        GameObject newTarget = Instantiate(target_green_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 3), Quaternion.identity);
                        targets.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        break;
                    }
                case 4:
                    {
                        GameObject newTarget = Instantiate(target_gray_obj, new Vector3(x + map_offset_X, y + map_offset_Y, 3), Quaternion.identity);
                        targets.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
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

    void OnClickTry()
    {
        gameController.gameplay_enetrance = 2;
        gameController.target_map_json = generateTestMapData();
        //Debug.Log(gameController.target_map_json);
        SceneManager.LoadScene("GamePlay");

    }

    void OnClickSave()
    {
        if(one_star.text == "" || two_star.text == "" || three_star.text == "")
        {
            Debug.Log("Must have value of star values");
            alert_text.text = "Must have value of star values";
            return;
        }
        int one_star_step = int.Parse(one_star.text);
        int two_star_step = int.Parse(two_star.text);
        int three_star_step = int.Parse(three_star.text);
        gameController.cur_one_star_step = one_star_step;
        gameController.cur_two_star_step = two_star_step;
        gameController.cur_three_star_step = three_star_step;
        string mapData = generateTestMapData();
        LevelData ld = new LevelData(mapData, 1, one_star_step, two_star_step, three_star_step);
        LocalSlot ls = new LocalSlot();
        if(gameController.cur_slot_index == -1)
        {
            ls.AddSlotMap(ld);
        }
        else
        {
            ls.UpdateSlotMap(gameController.cur_slot_index, ld);
        }

        SceneManager.LoadScene("Community");
    }

    void OnClickBack()
    {
        SceneManager.LoadScene("Community");
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

    public bool isWall(string elem_name)
    {
        return (elem_name == "Wall");
    }

    public bool isWall(int x, int y)
    {
        return walls.ContainsKey(100 * y + x);
    }

    public bool isPit(string elem_name)
    {
        return (elem_name == "Pit");
    }

    public bool isPit(int x, int y)
    {
        return pits.ContainsKey(100 * y + x);
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

        bool new_obj_is_wall = isWall(elem_name);
        bool new_obj_is_pit = isPit(elem_name);
        bool new_obj_is_layer0 = isLayer0(elem_name);
        bool new_obj_is_box = isBox(elem_name);
        bool new_obj_is_target = isTarget(elem_name);

        bool old_obj_is_wall = isWall(store_x, store_y);
        bool old_obj_is_pit = isPit(store_x, store_y);
        bool old_obj_is_layer0 = isLayer0(store_x, store_y);
        bool old_obj_is_box = isBox(store_x, store_y);
        bool old_obj_is_target = isTarget(store_x, store_y);
        bool old_obj_is_player = isPlayer(store_x, store_y);

        if (elem_name == "Player")
        {
            if (old_obj_is_pit || old_obj_is_wall || old_obj_is_box)
            {
                return;
            }
            
            if (!old_obj_is_layer0)
            {
                Debug.Log("There must be a floor before putting " + elem_name);
                alert_text.text = "There must be a floor before putting " + elem_name;
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
        else if (new_obj_is_wall || new_obj_is_pit || new_obj_is_box)
        {
            if (old_obj_is_player)
            {
                Debug.Log("Cannot put " + elem_name + " on player!");
                alert_text.text = "Cannot put " + elem_name + " on player!";
                return;
            }
        }




        int kind = Convert.ToInt32(new_obj_is_wall) * 512
            + Convert.ToInt32(new_obj_is_pit) * 256
            + Convert.ToInt32(new_obj_is_layer0) * 128
            + Convert.ToInt32(new_obj_is_target) * 64
            + Convert.ToInt32(new_obj_is_box) * 32
            + Convert.ToInt32(old_obj_is_wall) * 16
            + Convert.ToInt32(old_obj_is_pit) * 8
            + Convert.ToInt32(old_obj_is_layer0) * 4
            + Convert.ToInt32(old_obj_is_target) * 2
            + Convert.ToInt32(old_obj_is_box) * 1;

        // z-value: wall - 0, pit - 5, box - 0, tar - 3, floor - 5
        switch (kind)
        {
            case 32:
                {
                    Debug.Log("There must be a floor before putting " + elem_name);
                    alert_text.text = "There must be a floor before putting " + elem_name;
                    break;
                }
            case 36:
                {
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 37:
                {
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 38:
                {
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 39:
                {
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 40:
                {
                    Debug.Log(elem_name + " cannot be put on pits.");
                    alert_text.text = elem_name + " cannot be put on pits.";
                    break;
                }
            case 52:
                {
                    GameObject old_wall = walls[store_y * 100 + store_x];
                    Destroy(old_wall);
                    walls.Remove(store_y * 100 + store_x);
                    GameObject new_box = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_box.SetActive(true);
                    boxes.Add(store_y * 100 + store_x, new_box);
                    break;
                }
            case 64:
                {
                    Debug.Log("There must be a floor before putting " + elem_name);
                    alert_text.text = "There must be a floor before putting " + elem_name;
                    break;
                }
            case 68:
                {
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 69:
                {
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 70:
                {
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 71:
                {
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 72:
                {
                    Debug.Log(elem_name + " cannot be put on pits.");
                    alert_text.text = elem_name + " cannot be put on pits.";
                    break;
                }
            case 84:
                {
                    GameObject old_wall = walls[store_y * 100 + store_x];
                    Destroy(old_wall);
                    walls.Remove(store_y * 100 + store_x);
                    GameObject new_target = Instantiate(element, new Vector3(pos_x, pos_y, 3), Quaternion.identity);
                    new_target.SetActive(true);
                    targets.Add(store_y * 100 + store_x, new_target);
                    break;
                }
            case 128:
                {
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 132:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
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
                    layer0.Add(store_y * 100 + store_x, new_obj);
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
                    layer0.Add(store_y * 100 + store_x, new_obj);
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
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 136:
                {
                    GameObject old_pit = pits[store_y * 100 + store_x];
                    Destroy(old_pit);
                    pits.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 148:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_wall = walls[store_y * 100 + store_x];
                    Destroy(old_wall);
                    walls.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    layer0.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 256:
                {
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 260:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 261:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 262:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 263:
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
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 264:
                {
                    GameObject old_pit = pits[store_y * 100 + store_x];
                    Destroy(old_pit);
                    pits.Remove(store_y * 100 + store_x);
                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 276:
                {
                    GameObject old_layer0 = layer0[store_y * 100 + store_x];
                    Destroy(old_layer0);
                    layer0.Remove(store_y * 100 + store_x);
                    GameObject old_wall = walls[store_y * 100 + store_x];
                    Destroy(old_wall);
                    walls.Remove(store_y * 100 + store_x);

                    GameObject new_obj = Instantiate(element, new Vector3(pos_x, pos_y, 5), Quaternion.identity);
                    new_obj.SetActive(true);
                    pits.Add(store_y * 100 + store_x, new_obj);
                    break;
                }
            case 512:
                {
                    Debug.Log("There must be a floor before putting " + elem_name);
                    alert_text.text = "There must be a floor before putting " + elem_name;
                    break;
                }
            case 516:
                {
                    GameObject new_wall = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_wall.SetActive(true);
                    walls.Add(store_y * 100 + store_x, new_wall);
                    break;
                }
            case 517:
                {
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    GameObject new_wall = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_wall.SetActive(true);
                    walls.Add(store_y * 100 + store_x, new_wall);
                    break;
                }
            case 518:
                {
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);
                    GameObject new_wall = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_wall.SetActive(true);
                    walls.Add(store_y * 100 + store_x, new_wall);
                    break;
                }
            case 519:
                {
                    GameObject old_box = boxes[store_y * 100 + store_x];
                    Destroy(old_box);
                    boxes.Remove(store_y * 100 + store_x);
                    GameObject old_target = targets[store_y * 100 + store_x];
                    Destroy(old_target);
                    targets.Remove(store_y * 100 + store_x);

                    GameObject new_wall = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_wall.SetActive(true);
                    walls.Add(store_y * 100 + store_x, new_wall);
                    break;
                }
            case 520:
                {
                    Debug.Log(elem_name + " cannot be put on pits.");
                    alert_text.text = elem_name + " cannot be put on pits.";
                    break;
                }
            case 532:
                {
                    GameObject old_wall = walls[store_y * 100 + store_x];
                    Destroy(old_wall);
                    walls.Remove(store_y * 100 + store_x);

                    GameObject new_wall = Instantiate(element, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
                    new_wall.SetActive(true);
                    walls.Add(store_y * 100 + store_x, new_wall);
                    break;
                }
            default:
                {
                    Debug.Log("Someting Went Wrong --- " + elem_name + " " + kind.ToString());
                    return;
                }
        }
    }


    public string generateTestMapData()
    {
        List<string> walls_list = new List<string>();
        List<string> stones_list = new List<string>();
        List<string> ices_list = new List<string>();
        List<string> muds_list = new List<string>();
        List<string> pits_list = new List<string>();
        List<string> boxes_list = new List<string>();
        List<string> targets_list = new List<string>();
        List<int> player_list = new List<int>();

        foreach(int key in walls.Keys)
        {
            walls_list.Add(key % 100 + " " + key / 100);
        }
        foreach (int key in layer0.Keys)
        {
            GameObject value = layer0[key];
            string name = value.name;
            switch(name){
                case "Floor_Mud(Clone)":
                    muds_list.Add(key % 100 + " " + key / 100);
                    break;
                case "Floor_Ice(Clone)":
                    ices_list.Add(key % 100 + " " + key / 100);
                    break;
                case "Floor_Stone(Clone)":
                    stones_list.Add(key % 100 + " " + key / 100);
                    break;
                default:
                    break;
            }
        }
        foreach (int key in pits.Keys)
        {
            pits_list.Add(key % 100 + " " + key / 100);
        }
        foreach (int key in boxes.Keys)
        {
            GameObject value = boxes[key];
            string name = value.name;
            switch (name)
            {
                case "Box_Brown(Clone)":
                    boxes_list.Add(key % 100 + " " + key / 100+" 0");
                    break;
                case "Box_Red(Clone)":
                    boxes_list.Add(key % 100 + " " + key / 100+" 1");
                    break;
                case "Box_Blue(Clone)":
                    boxes_list.Add(key % 100 + " " + key / 100+" 2");
                    break;
                case "Box_Green(Clone)":
                    boxes_list.Add(key % 100 + " " + key / 100+" 3");
                    break;
                case "Box_Gray(Clone)":
                    boxes_list.Add(key % 100 + " " + key / 100+" 4");
                    break;
                default:
                    break;
            }

        }
        foreach (int key in targets.Keys)
        {
            GameObject value = targets[key];
            string name = value.name;
            switch (name)
            {
                case "Target_Brown(Clone)":
                    targets_list.Add(key % 100 + " " + key / 100 + " 0");
                    break;
                case "Target_Red(Clone)":
                    targets_list.Add(key % 100 + " " + key / 100 + " 1");
                    break;
                case "Target_Blue(Clone)":
                    targets_list.Add(key % 100 + " " + key / 100 + " 2");
                    break;
                case "Target_Green(Clone)":
                    targets_list.Add(key % 100 + " " + key / 100 + " 3");
                    break;
                case "Target_Gray(Clone)":
                    targets_list.Add(key % 100 + " " + key / 100 + " 4");
                    break;
                default:
                    break;
            }

        }
        string[] wall_position = walls_list.ToArray();
        string[] stone_position = stones_list.ToArray();
        string[] ice_position = ices_list.ToArray();
        string[] mud_position = muds_list.ToArray();
        string[] pit_position = pits_list.ToArray();
        string[] box_position = boxes_list.ToArray();
        string[] target_position = targets_list.ToArray();

        Debug.Log(player_position[0]);
        JsonObject jsonObject = new JsonObject(wall_position, stone_position, ice_position, mud_position, pit_position, player_position, box_position, target_position);
        
        Debug.Log(jsonObject.SaveToString());
        return jsonObject.SaveToString();
    }
}