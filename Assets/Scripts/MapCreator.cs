﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
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

    public Dictionary<int, KeyValuePair<int, GameObject>> boxes = new Dictionary<int, KeyValuePair<int, GameObject>>();
    public HashSet<int> walls = new HashSet<int>();
    public HashSet<int> stones = new HashSet<int>();
    public HashSet<int> ices = new HashSet<int>();
    public HashSet<int> muds = new HashSet<int>();
    public Dictionary<int, int> pits = new Dictionary<int, int>();

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
        pit.SetActive(false);
        pit_brown.SetActive(false);
        pit_red.SetActive(false);
        pit_blue.SetActive(false);
        pit_green.SetActive(false);
        pit_gray.SetActive(false);


        GameObject newPlayer = Instantiate(player, new Vector3(player_position[0], player_position[1], 0), Quaternion.identity);
        newPlayer.SetActive(true);

        for (int i = 0; i < map.Length; i++)
        {
            for(int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] == 'W')
                {
                    GameObject newWall = Instantiate(wall, new Vector3(j, i, 5), Quaternion.identity);
                    newWall.SetActive(true);
                    walls.Add(100 * i + j);
                }
                else if (map[i][j] == 'P')
                {
                    GameObject newPit = Instantiate(pit, new Vector3(j, i, 5), Quaternion.identity);
                    newPit.SetActive(true);
                    pits.Add(100 * i + j,-1);
                }
                else if (map[i][j] == 'S')
                {
                    GameObject newFloor = Instantiate(floor_stone, new Vector3(j, i, 5), Quaternion.identity);
                    stones.Add(100 * i + j);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == 'I')
                {
                    GameObject newFloor = Instantiate(floor_ice, new Vector3(j, i, 5), Quaternion.identity);
                    ices.Add(100 * i + j);
                    newFloor.SetActive(true);
                }
                else if (map[i][j] == 'M')
                {
                    GameObject newFloor = Instantiate(floor_mud, new Vector3(j, i, 5), Quaternion.identity);
                    muds.Add(100 * i + j);
                    newFloor.SetActive(true);
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
                        GameObject newBox = Instantiate(box_brown, new Vector3(x, y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x,new KeyValuePair<int, GameObject>(color,newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 1:
                    {
                        GameObject newBox = Instantiate(box_red, new Vector3(x, y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        GameObject newBox = Instantiate(box_blue, new Vector3(x, y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        GameObject newBox = Instantiate(box_green, new Vector3(x, y, 0), Quaternion.identity);
                        boxes.Add(100 * y + x, new KeyValuePair<int, GameObject>(color, newBox));
                        newBox.SetActive(true);
                        break;
                    }
                case 4:
                    {
                        GameObject newBox = Instantiate(box_gray, new Vector3(x, y, 0), Quaternion.identity);
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
                        GameObject newTarget = Instantiate(target_brown, new Vector3(x, y, 3), Quaternion.identity);
                        targets_brown.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        break;
                    }
                case 1:
                    {
                        GameObject newTarget = Instantiate(target_red, new Vector3(x, y, 0), Quaternion.identity);
                        targets_red.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        GameObject newTarget = Instantiate(target_blue, new Vector3(x, y, 0), Quaternion.identity);
                        targets_blue.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        GameObject newTarget = Instantiate(target_green, new Vector3(x, y, 0), Quaternion.identity);
                        targets_green.Add(100 * y + x, false);
                        newTarget.SetActive(true);
                        break;
                    }
                case 4:
                    {
                        GameObject newTarget = Instantiate(target_gray, new Vector3(x, y, 0), Quaternion.identity);
                        targets_gray.Add(100 * y + x, false);
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
        
    }
}
