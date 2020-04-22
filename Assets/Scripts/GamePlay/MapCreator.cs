using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Analytics;
using Json;

public class MapCreator : MonoBehaviour
{
    //public static MapCreator instance;
    GameController gameController;
    WinPanelController winPanelController;

    //public int level;

    //public string[] map;
    private string[] wall_position;
    private string[] stone_position;
    private string[] ice_position;
    private string[] mud_position;
    private string[] pit_position;
    private int[] player_position;
    private string[] box_position;
    private string[] target_position;

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
        Debug.Log("Current Level: " + Convert.ToString(gameController.cur_level + 1));
        win = 0;
        if (gameController.gameplay_enetrance == 0)
        {
            getMapDataFromLocalFile(gameController.cur_level + 1);
        }
        else
        {
            getMapDataFromLocalJson();
        }
        

        AnalyticsHelper.time_startPlayingLevel = Time.realtimeSinceStartup;
        AnalyticsHelper.AddTry(gameController.cur_level);
        Analytics.CustomEvent("level_start", new Dictionary<string, object>
        {
            {"level_index", gameController.cur_level },
            {"session_id", AnalyticsSessionInfo.sessionId },
            {"user_id", AnalyticsSessionInfo.userId}
        });
    }

    private void getMapDataFromLocalJson()
    {
        JsonObject jsonObject = JsonObject.CreateFromJSON(gameController.target_map_json);
        wall_position = jsonObject.wall_position;
        stone_position = jsonObject.stone_position;
        ice_position = jsonObject.ice_position;
        mud_position = jsonObject.mud_position;
        pit_position = jsonObject.pit_position;
        box_position = jsonObject.box_position;
        target_position = jsonObject.target_position;
        player_position = jsonObject.player_position;
    }

    private void getMapDataFromLocalFile(int level)
    {
        var fileAddress = Path.Combine(Application.streamingAssetsPath, "Levels/level_" + Convert.ToString(level) + ".txt");
        FileInfo fInfo0 = new FileInfo(fileAddress);
        if (fInfo0.Exists)
        {
            StreamReader r = new StreamReader(fileAddress);
            string s = r.ReadToEnd();
            string[] lines = s.Split('\n');
            int type = -1;
            string[] keywords;
            if (SystemInfo.operatingSystem.ToString()[0] == 'W')
            {
                keywords = new string[]{ "WALL\r", "STONE\r", "ICE\r", "MUD\r", "PIT\r", "BOX\r", "TARGET\r", "PLAYER\r" };
            }
            else
            {
                keywords = new string[]{ "WALL", "STONE", "ICE", "MUD", "PIT", "BOX", "TARGET", "PLAYER" };

            }
            List<string> walls_list = new List<string>();
            List<string> stones_list = new List<string>();
            List<string> ices_list = new List<string>();
            List<string> muds_list = new List<string>();
            List<string> pits_list = new List<string>();
            List<string> boxes_list = new List<string>();
            List<string> targets_list = new List<string>();
            List<int> player_list = new List<int>();
            foreach (string line in lines)
            {
                int index = Array.IndexOf(keywords, line);
                if(index != -1)
                {
                    type = index;
                }
                else
                {
                    
                    switch (type)
                    {
                        case 0:
                            {
                                walls_list.Add(line);
                                break;
                            }
                        case 1:
                            {
                                stones_list.Add(line);
                                break;
                            }
                        case 2:
                            {
                                ices_list.Add(line);
                                break;
                            }
                        case 3:
                            {
                                muds_list.Add(line);
                                break;
                            }
                        case 4:
                            {
                                pits_list.Add(line);
                                break;
                            }
                        case 5:
                            {
                                boxes_list.Add(line);
                                break;
                            }
                        case 6:
                            {
                                targets_list.Add(line);
                                break;
                            }
                        case 7:
                            {
                                player_list.Add(int.Parse(line.Split(' ')[0]));
                                player_list.Add(int.Parse(line.Split(' ')[1]));
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                }
                
            }
            wall_position = walls_list.ToArray();
            stone_position = stones_list.ToArray();
            ice_position = ices_list.ToArray();
            mud_position = muds_list.ToArray();
            pit_position = pits_list.ToArray();
            box_position = boxes_list.ToArray();
            target_position = targets_list.ToArray();
            player_position = player_list.ToArray();
            JsonObject jsonObject = new JsonObject(wall_position, stone_position, ice_position, mud_position, pit_position, player_position, box_position, target_position);
            Debug.Log(jsonObject.SaveToString());
        }
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
                        if (boxes.ContainsKey(100 * y + x) && boxes[100 * y + x].Key == 0)
                        {
                            brown_num--;
                        }
                        break;
                    }
                case 1:
                    {
                        GameObject newTarget = Instantiate(target_red, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_red.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        red_num++;
                        if (boxes.ContainsKey(100 * y + x) && boxes[100 * y + x].Key == 1)
                        {
                            red_num--;
                        }
                        break;
                    }
                case 2:
                    {
                        GameObject newTarget = Instantiate(target_blue, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_blue.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        blue_num++;
                        if (boxes.ContainsKey(100 * y + x) && boxes[100 * y + x].Key == 2)
                        {
                            blue_num--;
                        }
                        break;
                    }
                case 3:
                    {
                        GameObject newTarget = Instantiate(target_green, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_green.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        green_num++;
                        if (boxes.ContainsKey(100 * y + x) && boxes[100 * y + x].Key == 3)
                        {
                            green_num--;
                        }
                        break;
                    }
                case 4:
                    {
                        GameObject newTarget = Instantiate(target_gray, new Vector3(x+ map_offset_X, y+ map_offset_Y, 3), Quaternion.identity);
                        targets_gray.Add(100 * y + x, newTarget);
                        newTarget.SetActive(true);
                        gray_num++;
                        if (boxes.ContainsKey(100 * y + x) && boxes[100 * y + x].Key == 4)
                        {
                            gray_num--;
                        }
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
        if(gameController.gameplay_enetrance != 2 && win == 0 && brown_num == 0 && red_num == 0 && blue_num == 0 && green_num == 0 && gray_num == 0)
        {
            win = 1;
            win_panel.SetActive(true);
            winPanelController = FindObjectOfType<WinPanelController>();
            final_step = step;
            //todo star
            
            winPanelController.final_step = final_step;
            int tmp= winPanelController.GetStar();
            winPanelController.star = tmp;
            if (gameController.gameplay_enetrance == 1)
            {
                Debug.Log("handle currency");
                gameController.currency.PassLevel(tmp);
            }
            winPanelController.GetComponentInChildren<Text>().text= "You Win!";
            
            winPanelController.ShowStar();
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
