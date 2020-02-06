using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{
    //public static MapCreator instance;
    GameController gameController;

    public int level;

    public string[] map;
    public int[] player_position;
    public string[] box_position;
    public string[] target_position;

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

    public int map_offset_X = -8;
    public int map_offset_Y = -4;

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

    private void Awake()
    {
        //gameController = FindObjectOfType<GameController>();
        //Debug.Log(gameController.cur_level);
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

        for (int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] == 'W')
                {
                    GameObject newWall = Instantiate(wall, new Vector3(j+ map_offset_X, i+ map_offset_Y, 5), Quaternion.identity);
                    newWall.SetActive(true);
                    walls.Add(100 * i + j);
                }
                else if (map[i][j] == 'P')
                {
                    GameObject newPit = Instantiate(pit, new Vector3(j+ map_offset_X, i+ map_offset_Y, 5), Quaternion.identity);
                    newPit.SetActive(true);
                    pits.Add(100 * i + j, newPit);
                }
                else if (map[i][j] == 'S')
                {
                    GameObject newFloor = Instantiate(floor_stone, new Vector3(j+ map_offset_X, i+ map_offset_Y, 5), Quaternion.identity);
                    stones.Add(100 * i + j);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == 'I')
                {
                    GameObject newFloor = Instantiate(floor_ice, new Vector3(j+ map_offset_X, i+ map_offset_Y, 5), Quaternion.identity);
                    ices.Add(100 * i + j);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == 'M')
                {
                    GameObject newFloor = Instantiate(floor_mud, new Vector3(j+ map_offset_X, i+ map_offset_Y, 5), Quaternion.identity);
                    muds.Add(100 * i + j);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == '#')                
                {
                    continue;
                }
                else
                {
                    Debug.Log("Item letter is not defined " + map[i][j]);
                }
            }
        }

        for(int i = 0; i < box_position.Length; i++)
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
                        targets_brown.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        brown_num++;
                        break;
                    }
                case 1:
                    {
                        GameObject newTarget = Instantiate(target_red, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_red.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        red_num++;
                        break;
                    }
                case 2:
                    {
                        GameObject newTarget = Instantiate(target_blue, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_blue.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        blue_num++;
                        break;
                    }
                case 3:
                    {
                        GameObject newTarget = Instantiate(target_green, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_green.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        green_num++;
                        break;
                    }
                case 4:
                    {
                        GameObject newTarget = Instantiate(target_gray, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_gray.Add(100 * y + x, false);
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

    public void UpdateTargetNum()
    {
        brown_tar.text = (100+brown_num).ToString().Substring(1);
        red_tar.text = (100 + red_num).ToString().Substring(1);
        blue_tar.text = (100 + blue_num).ToString().Substring(1);
        green_tar.text = (100 + green_num).ToString().Substring(1);
        gray_tar.text = (100 + gray_num).ToString().Substring(1);
        if(brown_num == 0 && red_num == 0 && blue_num == 0 && green_num == 0 && gray_num == 0)
        {
            Debug.Log("Win!");
            //TODO: Add a dialog here to show all targets are reached.
        }
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
