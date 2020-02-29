using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{
    //public static MapCreator instance;
    GameController gameController;
    WinPanelController winPanelController;

    public int level;

    //public string[] map;
    private string[] wall_position
        = { "0 0", "1 0", "2 0", "3 0", "4 0", "5 0", "6 0", "7 0", "8 0", "9 0",
            "10 0", "11 0", "0 1", "11 1", "0 2", "8 2", "9 2", "10 2", "11 2", "0 3",
            "5 3", "6 3", "7 3", "8 3", "9 3", "10 3", "11 3", "0 4", "5 4", "6 4",
            "7 4", "8 4", "9 4", "10 4", "11 4", "0 5", "2 5", "3 5", "4 5", "5 5",
            "6 5", "7 5", "8 5", "9 5", "10 5", "11 5", "0 6", "2 6", "3 6", "4 6",
            "5 6", "6 6", "7 6", "8 6", "9 6", "10 6", "11 6", "0 7", "11 7", "0 8",
            "1 8", "2 8", "3 8", "4 8", "5 8", "6 8", "7 8", "8 8", "9 8", "10 8",
            "11 8" };
    private string[] stone_position
        = { "0 0", "1 0", "2 0", "3 0", "4 0", "5 0", "6 0", "7 0", "8 0", "9 0",
            "10 0", "11 0", "0 1", "1 1", "2 1", "3 1", "4 1", "5 1", "6 1", "7 1",
            "8 1", "9 1", "10 1", "11 1", "0 2", "1 2", "2 2", "3 2", "4 2", "5 2",
            "6 2", "7 2", "8 2", "9 2", "10 2", "11 2", "0 3", "1 3", "5 3", "6 3",
            "7 3", "8 3", "9 3", "10 3", "11 3", "0 4", "1 4", "5 4", "6 4", "7 4",
            "8 4", "9 4", "10 4", "11 4", "0 5", "1 5", "2 5", "3 5", "4 5", "5 5",
            "6 5", "7 5", "8 5", "9 5", "10 5", "11 5", "0 6", "2 6", "3 6", "4 6",
            "5 6", "6 6", "7 6", "8 6", "9 6", "10 6", "11 6", "0 7", "1 7", "9 7",
            "10 7", "11 7", "0 8", "1 8", "2 8", "3 8", "4 8", "5 8", "6 8", "7 8",
            "8 8", "9 8", "10 8", "11 8" };
    private string[] ice_position
        = { "2 3", "3 3", "4 3", "2 4", "1 6", "2 7", "3 7", "4 7", "5 7", "6 7", "7 7", "8 7" };
    private string[] mud_position = { };
    private string[] pit_position = { "3 4", "4 4" };
    private int[] player_position = { 1, 1 };
    private string[] box_position = { "2 3 1", "4 2 0" };
    private string[] target_position = { "2 4 1", "4 1 0" };

    public GameObject win_panel;

    public GameObject player;
    public GameObject wall;
    public GameObject box_brown;
    public GameObject box_red;
    public GameObject box_blue;
    public GameObject box_green;
    public GameObject box_gray;
    public GameObject target_brown;
    public GameObject target_red;
    public GameObject target_blue;
    public GameObject target_green;
    public GameObject target_gray;
    public GameObject floor_stone;
    public GameObject floor_mud;
    public GameObject floor_ice;
    public GameObject pit;
    public GameObject pit_brown;
    public GameObject pit_red;
    public GameObject pit_blue;
    public GameObject pit_green;
    public GameObject pit_gray;

    public Text player_step;
    public Text brown_tar;
    public Text red_tar;
    public Text blue_tar;
    public Text green_tar;
    public Text gray_tar;

    public Image brown_box;
    public Image red_box;
    public Image blue_box;
    public Image green_box;
    public Image gray_box;

    public int brown_num;
    public int red_num;
    public int blue_num;
    public int green_num;
    public int gray_num;

    public int step;
    public int final_step;
    public int star;
    public int win;

    public int map_offset_X = -8;
    public int map_offset_Y = -4;

    public Dictionary<int, KeyValuePair<int, GameObject>> boxes = new Dictionary<int, KeyValuePair<int, GameObject>>();
    //public HashSet<int> walls = new HashSet<int>();
    public Dictionary<int, GameObject> walls = new Dictionary<int, GameObject>();

    //public HashSet<int> stones = new HashSet<int>();
    //public HashSet<int> ices = new HashSet<int>();
    //public HashSet<int> muds = new HashSet<int>();
    public Dictionary<int, GameObject> stones = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> ices = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> muds = new Dictionary<int, GameObject>();

    public Dictionary<int, GameObject> pits = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> covered_pits = new Dictionary<int, GameObject>();

    public Dictionary<int, GameObject> targets_brown = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> targets_red = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> targets_blue = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> targets_green = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> targets_gray = new Dictionary<int, GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        //Debug.Log(gameController.cur_level);

        win = 0;
    }

    void Start()
    {
        step = 0;
        brown_num = 0;
        red_num = 0;
        blue_num = 0;
        green_num = 0;
        gray_num = 0;

        GameObject newPlayer = Instantiate(player, new Vector3(player_position[0]+ map_offset_X, player_position[1]+ map_offset_Y, 0), Quaternion.identity);
        newPlayer.SetActive(true);

        for (int i = 0; i < wall_position.Length; i++)
        {
            string[] str_arr = wall_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newWall = Instantiate(wall, new Vector3(x + map_offset_X, y + map_offset_Y, 0), Quaternion.identity);
            walls.Add(100 * y + x, newWall);
            newWall.SetActive(true);
        }
        for (int i = 0; i < pit_position.Length; i++)
        {
            string[] str_arr = pit_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newPit = Instantiate(pit, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            pits.Add(100 * y + x, newPit);
            newPit.SetActive(true);
        }

        for (int i = 0; i < stone_position.Length; i++)
        {
            string[] str_arr = stone_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newStone = Instantiate(floor_stone, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            stones.Add(100 * y + x, newStone);
            newStone.SetActive(true);
        }

        for (int i = 0; i < ice_position.Length; i++)
        {
            string[] str_arr = ice_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newIce = Instantiate(floor_ice, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            ices.Add(100 * y + x, newIce);
            newIce.SetActive(true);
        }

        for (int i = 0; i < mud_position.Length; i++)
        {
            string[] str_arr = mud_position[i].Split(' ');
            int x = int.Parse(str_arr[0]);
            int y = int.Parse(str_arr[1]);
            GameObject newMud = Instantiate(floor_mud, new Vector3(x + map_offset_X, y + map_offset_Y, 5), Quaternion.identity);
            muds.Add(100 * y + x, newMud);
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
                        GameObject newBox = Instantiate(box_brown, new Vector3(x+ map_offset_X, y+ map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x,new KeyValuePair<int, GameObject>(color,newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 1:
                    {
                        GameObject newBox = Instantiate(box_red, new Vector3(x+ map_offset_X, y+ map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        GameObject newBox = Instantiate(box_blue, new Vector3(x+ map_offset_X, y+ map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        GameObject newBox = Instantiate(box_green, new Vector3(x+ map_offset_X, y+ map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 4:
                    {
                        GameObject newBox = Instantiate(box_gray, new Vector3(x+ map_offset_X, y+ map_offset_Y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
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
                        GameObject newTarget = Instantiate(target_brown, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_brown.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        brown_num++;
                        break;
                    }
                case 1:
                    {
                        GameObject newTarget = Instantiate(target_red, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_red.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        red_num++;
                        break;
                    }
                case 2:
                    {
                        GameObject newTarget = Instantiate(target_blue, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_blue.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        blue_num++;
                        break;
                    }
                case 3:
                    {
                        GameObject newTarget = Instantiate(target_green, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_green.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        green_num++;
                        break;
                    }
                case 4:
                    {
                        GameObject newTarget = Instantiate(target_gray, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_gray.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        gray_num++;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        Vector3[] box_position_in_scene = {new Vector3(-55, 90, 0), new Vector3(40, 90, 0),
            new Vector3(-55, 0, 0), new Vector3(40, 0, 0), new Vector3(-55, -90, 0) };
        int cnt = 0;
        if (brown_num == 0)
        {
            brown_tar.gameObject.SetActive(false);
            brown_box.gameObject.SetActive(false);
        }
        else
        {
            brown_tar.rectTransform.localPosition = new Vector3(box_position_in_scene[cnt].x + 1.5f, box_position_in_scene[cnt].y + 2.0f, 0);
            brown_box.rectTransform.localPosition = box_position_in_scene[cnt];
            cnt++;
        }
        if (red_num == 0)
        {
            red_tar.gameObject.SetActive(false);
            red_box.gameObject.SetActive(false);
        }
        else
        {
            red_tar.rectTransform.localPosition = new Vector3(box_position_in_scene[cnt].x + 1.5f, box_position_in_scene[cnt].y + 2.0f, 0);
            red_box.rectTransform.localPosition = box_position_in_scene[cnt];
            cnt++;
        }
        if (blue_num == 0)
        {
            blue_tar.gameObject.SetActive(false);
            blue_box.gameObject.SetActive(false);
        }
        else
        {
            blue_tar.rectTransform.localPosition = new Vector3(box_position_in_scene[cnt].x + 1.5f, box_position_in_scene[cnt].y + 2.0f, 0);
            blue_box.rectTransform.localPosition = box_position_in_scene[cnt];
            cnt++;
        }
        if (green_num == 0)
        {
            green_tar.gameObject.SetActive(false);
            green_box.gameObject.SetActive(false);
        }
        else
        {
            green_tar.rectTransform.localPosition = new Vector3(box_position_in_scene[cnt].x + 1.5f, box_position_in_scene[cnt].y + 2.0f, 0);
            green_box.rectTransform.localPosition = box_position_in_scene[cnt];
            cnt++;
        }
        if (gray_num == 0)
        {
            gray_tar.gameObject.SetActive(false);
            gray_box.gameObject.SetActive(false);
        }
        else
        {
            gray_tar.rectTransform.localPosition = new Vector3(box_position_in_scene[cnt].x + 1.5f, box_position_in_scene[cnt].y + 2.0f, 0);
            gray_box.rectTransform.localPosition = box_position_in_scene[cnt];
            cnt++;
        }
        UpdateTargetNum();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int UpdateTargetNum()
    {
        brown_tar.text = (100 + brown_num).ToString().Substring(1);
        red_tar.text = (100 + red_num).ToString().Substring(1);
        blue_tar.text = (100 + blue_num).ToString().Substring(1);
        green_tar.text = (100 + green_num).ToString().Substring(1);
        gray_tar.text = (100 + gray_num).ToString().Substring(1);
        if(win == 0 && brown_num == 0 && red_num == 0 && blue_num == 0 && green_num == 0 && gray_num == 0)
        {
            win = 1;
            win_panel.SetActive(true);
            winPanelController = FindObjectOfType<WinPanelController>();
            final_step = step;
            //todo star
            winPanelController.final_step = final_step;
            winPanelController.star = star;
            return 1;
        }
        return 0;
    }

    public void UpdateStepNum()
    {
        player_step.text = (1000+step).ToString().Substring(1);
    }

    public void UpdateBoxNum()
    {
        player_step.text = (100 + step).ToString().Substring(1);
    }
}
