using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public string[] map;
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

    public Dictionary<int, GameObject> boxes = new Dictionary<int, GameObject>();
    public HashSet<int> walls = new HashSet<int>();
    public Dictionary<int, bool> targets_brown = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_red = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_blue = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_green = new Dictionary<int, bool>();
    public Dictionary<int, bool> targets_gray = new Dictionary<int, bool>();
    // Start is called before the first frame update
    void Start()
    {
        int map_offset_X = 0;
        int map_offset_Y = 0;


        player.SetActive(false);
        wall.SetActive(false);
        box_brown.SetActive(false);
        box_red.SetActive(false);
        box_blue.SetActive(false);
        box_green.SetActive(false);
        box_gray.SetActive(false);
        target_brown.SetActive(false);
        target_red.SetActive(false);
        target_blue.SetActive(false);
        target_green.SetActive(false);
        target_gray.SetActive(false);
        floor_stone.SetActive(false);
        floor_mud.SetActive(false);
        floor_ice.SetActive(false);
        for (int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] == 'W')
                {
                    GameObject newWall = Instantiate(wall, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newWall.SetActive(true);
                    walls.Add(100 * i + j);
                }
                else if(map[i][j] == '0')
                {
                    GameObject newTarget = Instantiate(target_brown, new Vector3(i + map_offset_X, j + map_offset_Y, 1), Quaternion.identity);
                    newTarget.SetActive(true);
                    targets_brown.Add(100 * i + j, false);
                }
                else if (map[i][j] == '1')
                {
                    GameObject newTarget = Instantiate(target_red, new Vector3(i + map_offset_X, j + map_offset_Y, 1), Quaternion.identity);
                    newTarget.SetActive(true);
                    targets_red.Add(100 * i + j, false);
                }
                else if (map[i][j] == '2')
                {
                    GameObject newTarget = Instantiate(target_blue, new Vector3(i + map_offset_X, j + map_offset_Y, 1), Quaternion.identity);
                    newTarget.SetActive(true);
                    targets_blue.Add(100 * i + j, false);
                }
                else if (map[i][j] == '3')
                {
                    GameObject newTarget = Instantiate(target_green, new Vector3(i + map_offset_X, j + map_offset_Y, 1), Quaternion.identity);
                    newTarget.SetActive(true);
                    targets_green.Add(100 * i + j, false);
                }
                else if (map[i][j] == '4')
                {
                    GameObject newTarget = Instantiate(target_gray, new Vector3(i + map_offset_X, j + map_offset_Y, 1), Quaternion.identity);
                    newTarget.SetActive(true);
                    targets_gray.Add(100 * i + j, false);
                }
                else if (map[i][j] == 'P')
                {
                    GameObject newPlayer = Instantiate(player, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newPlayer.SetActive(true);
                }
                else if (map[i][j] == '5')
                {
                    GameObject newBox = Instantiate(box_brown, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newBox.SetActive(true);
                    boxes.Add(100 * i + j, newBox);
                }
                else if (map[i][j] == '6')
                {
                    GameObject newBox = Instantiate(box_red, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newBox.SetActive(true);
                    boxes.Add(100 * i + j, newBox);
                }
                else if (map[i][j] == '7')
                {
                    GameObject newBox = Instantiate(box_blue, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newBox.SetActive(true);
                    boxes.Add(100 * i + j, newBox);
                }
                else if (map[i][j] == '8')
                {
                    GameObject newBox = Instantiate(box_green, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newBox.SetActive(true);
                    boxes.Add(100 * i + j, newBox);
                }
                else if (map[i][j] == '9')
                {
                    GameObject newBox = Instantiate(box_gray, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newBox.SetActive(true);
                    boxes.Add(100 * i + j, newBox);
                }
                //todo pits
                else if (map[i][j] == 'T')
                {
                    GameObject newBox = Instantiate(box_gray, new Vector3(i + map_offset_X, j + map_offset_Y), Quaternion.identity);
                    newBox.SetActive(true);
                    boxes.Add(100 * i + j, newBox);
                }
                else if (map[i][j] == 'S')
                {
                    GameObject newFloor = Instantiate(floor_stone, new Vector3(i + map_offset_X, j + map_offset_Y,2), Quaternion.identity);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == 'I')
                {
                    GameObject newFloor = Instantiate(floor_ice, new Vector3(i + map_offset_X, j + map_offset_Y,2), Quaternion.identity);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == 'M')
                {
                    GameObject newFloor = Instantiate(floor_mud, new Vector3(i + map_offset_X, j + map_offset_Y,2), Quaternion.identity);
                    newFloor.SetActive(true);
                }
                else if(map[i][j]== '#')
                {

                }
                else
                {
                    Debug.Log("Item letter is not defined " + map[i][j]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
