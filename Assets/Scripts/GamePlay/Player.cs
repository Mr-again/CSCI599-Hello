using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MapCreator mapCreator;
    private Animator animator;

    private int x_dir = 0;
    private int y_dir = 0;

    private float fingerActionSensitivity = Screen.width * 0.05f;

    private float fingerBeginX;
    private float fingerBeginY;
    private float fingerCurrentX;
    private float fingerCurrentY;
    private float fingerSegmentX;
    private float fingerSegmentY;
    
    private int fingerTouchState;
    
    private int FINGER_STATE_NULL = 0;
    private int FINGER_STATE_TOUCH = 1;
    private int FINGER_STATE_ADD = 2;

    //private GameObject[] boxes = new GameObject[5];
    //private Dictionary<int, bool>[] targets = new Dictionary<int, bool>[5];
    public GameObject[] covered_pits = new GameObject[5];

    private void Awake()
    {
        //Debug.Log(SystemInfo.deviceType);

        mapCreator = FindObjectOfType<MapCreator>();
        animator = GetComponent<Animator>();

        covered_pits[0] = mapCreator.pit_brown;
        covered_pits[1] = mapCreator.pit_red;
        covered_pits[2] = mapCreator.pit_blue;
        covered_pits[3] = mapCreator.pit_green;
        covered_pits[4] = mapCreator.pit_gray;
    }
    // Start is called before the first frame update
    void Start()
    {
        fingerActionSensitivity = Screen.width * 0.05f;
        fingerBeginX = 0;
        fingerBeginY = 0;
        fingerCurrentX = 0;
        fingerCurrentY = 0;
        fingerSegmentX = 0;
        fingerSegmentY = 0;

        fingerTouchState = FINGER_STATE_NULL;
    }

    // Update is called once per frame
    void Update()
    {
        int dx = 0;
        int dy = 0;
        if (SystemInfo.deviceType==DeviceType.Desktop)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                dx--;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                dx++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                dy++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                dy--;
            }
        }else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (fingerTouchState == FINGER_STATE_NULL)
                {
                    fingerTouchState = FINGER_STATE_TOUCH;
                    fingerBeginX = Input.mousePosition.x;
                    fingerBeginY = Input.mousePosition.y;
                }

            }

            if (fingerTouchState == FINGER_STATE_TOUCH)
            {
                fingerCurrentX = Input.mousePosition.x;
                fingerCurrentY = Input.mousePosition.y;
                fingerSegmentX = fingerCurrentX - fingerBeginX;
                fingerSegmentY = fingerCurrentY - fingerBeginY;

            }


            if (fingerTouchState == FINGER_STATE_TOUCH)
            {
                float fingerDistance = fingerSegmentX * fingerSegmentX + fingerSegmentY * fingerSegmentY;

                if (fingerDistance > (fingerActionSensitivity * fingerActionSensitivity))
                {
                    toAddFingerAction(ref dx,ref dy);
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                fingerTouchState = FINGER_STATE_NULL;
            }

        }
        else
        {
            Console.Error.WriteLine("Wrong Device Type");
        }
        //else if(Input.GetMouseButton(0))
        //{
        //    Debug.Log(Input.mousePosition);
        //}

        if (dx == 0 && dy == 0)
        {
            animator.SetBool("isMoving", false);
            return;
        }
        x_dir = dx;
        y_dir = dy;
        animator.SetBool("isMoving", true);
        animator.SetFloat("deltaX", dx);
        animator.SetFloat("deltaY", dy);

        int x0 = (int)transform.position.x - mapCreator.map_offset_X;
        int y0 = (int)transform.position.y - mapCreator.map_offset_Y;

        int x1 = x0 + dx;
        int y1 = y0 + dy;

        int x2 = x1 + dx;
        int y2 = y1 + dy;

        if (isWall(x1, y1))
        {
            return;
        }
        else if(isPit(x1, y1))
        {
            return;
        }
        else if(isBox(x1, y1))
        {
            if(isMud(x1, y1))
            {
                return;
            }
            if(isWall(x2, y2))
            {
                string box_color = getBox(x1, y1).name.Split('_')[1].Split('(')[0];
                //Debug.Log(box_color);
                if (box_color.Equals("Red"))
                {
                    int[] tmp_dx = { 1, 0, -1 };
                    int[] tmp_dy = { 1, 0, -1 };
                    for(int i = 0; i < 3; i++)
                    {
                        for(int j = 0; j < 3; j++)
                        {
                            if (isWall(x1 - tmp_dx[i], y1 - tmp_dy[j]))
                            {
                                makeWallExpode(getWall(x1 - tmp_dx[i], y1 - tmp_dy[j]));
                                mapCreator.walls.Remove(100 * (y1 - tmp_dy[j]) + x1 - tmp_dx[i]);
                            }
                                
                        }
                    }
                    makeRedBoxExpode(getBox(x1, y1));
                    if (mapCreator.targets_red.ContainsKey(y1 * 100 + x1))
                    {
                        mapCreator.red_num++;
                        mapCreator.UpdateTargetNum();
                    }
                    mapCreator.boxes.Remove(100 * y1 + x1);


                }
                return;
            }
            else if(isBox(x2, y2))
            {
                return;
            }
            else if(isPit(x2, y2))
            {
                GameObject inHandBox = getBox(x1, y1);
                inHandBox.SetActive(false);
                int box_color = mapCreator.boxes[y1 * 100 + x1].Key;
                mapCreator.boxes.Remove(y1 * 100 + x1);
                GameObject inHandPit = getPit(x2, y2);
                inHandPit.SetActive(false);
                mapCreator.pits.Remove(y2 * 100 + x2);
                GameObject newPit = Instantiate(covered_pits[box_color],
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 3), Quaternion.identity);
                newPit.SetActive(true);
                mapCreator.covered_pits.Add(100 * y2 + x2, newPit);

                mapCreator.step++;
                mapCreator.UpdateStepNum();
                transform.position = new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y);
                //determine if here box is already on the target, then we need add target_num
                switch (box_color)
                {
                    case 0:
                        {
                            if(mapCreator.targets_brown.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.brown_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (mapCreator.targets_red.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.red_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (mapCreator.targets_blue.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.blue_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            if (isStone(x1, y1))
                            {
                                GameObject x1_y1_stone = getStone(x1, y1);
                                Destroy(x1_y1_stone);
                                mapCreator.stones.Remove(100 * y1 + x1);
                                GameObject new_x1_y1_ice = Instantiate(mapCreator.floor_ice,
                                    new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y, 5),
                                    Quaternion.identity);
                                new_x1_y1_ice.SetActive(true);
                                mapCreator.ices.Add(100 * y1 + x1, new_x1_y1_ice);
                            }
                            else if (isMud(x1, y1))
                            {
                                GameObject x1_y1_mud = getMud(x1, y1);
                                Destroy(x1_y1_mud);
                                mapCreator.muds.Remove(100 * y1 + x1);
                                GameObject new_x1_y1_ice = Instantiate(mapCreator.floor_ice,
                                    new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y, 5),
                                    Quaternion.identity);
                                new_x1_y1_ice.SetActive(true);
                                mapCreator.ices.Add(100 * y1 + x1, new_x1_y1_ice);
                            }
                            else if (isIce(x1, y1))
                            {
                                // here is ice
                            }
                            else
                            {
                                Debug.Log("Here should only be stone or mud or ice!");
                            }
                            break;
                        }
                    case 3:
                        {
                            if (mapCreator.targets_green.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.green_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 4:
                        {
                            if (mapCreator.targets_gray.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.gray_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                GameObject inHandBox = getBox(x1, y1);
                inHandBox.transform.position = new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y);
                int box_color = mapCreator.boxes[y1 * 100 + x1].Key;
                mapCreator.boxes.Remove(y1 * 100 + x1);
                mapCreator.boxes.Add(y2 * 100 + x2, new KeyValuePair<int, GameObject>(box_color, inHandBox));

                mapCreator.step++;
                mapCreator.UpdateStepNum();
                transform.position = new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y);
                //determine if here box is on the target already
                switch (box_color)
                {
                    case 0:
                        {
                            if (mapCreator.targets_brown.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.brown_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (mapCreator.targets_red.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.red_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (mapCreator.targets_blue.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.blue_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            if (isStone(x1, y1))
                            {
                                GameObject x1_y1_stone = getStone(x1, y1);
                                Destroy(x1_y1_stone);
                                mapCreator.stones.Remove(100 * y1 + x1);
                                GameObject new_x1_y1_ice = Instantiate(mapCreator.floor_ice,
                                    new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y, 5),
                                    Quaternion.identity);
                                new_x1_y1_ice.SetActive(true);
                                mapCreator.ices.Add(100 * y1 + x1, new_x1_y1_ice);
                            }
                            else if (isMud(x1, y1))
                            {
                                GameObject x1_y1_mud = getMud(x1, y1);
                                Destroy(x1_y1_mud);
                                mapCreator.muds.Remove(100 * y1 + x1);
                                GameObject new_x1_y1_ice = Instantiate(mapCreator.floor_ice,
                                    new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y, 5),
                                    Quaternion.identity);
                                new_x1_y1_ice.SetActive(true);
                                mapCreator.ices.Add(100 * y1 + x1, new_x1_y1_ice);
                            }
                            else if (isIce(x1, y1))
                            {
                                // here is ice
                            }
                            else
                            {
                                Debug.Log("Here should only be stone or mud or ice!");
                            }
                            break;
                        }
                    case 3:
                        {
                            if (mapCreator.targets_green.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.green_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 4:
                        {
                            if (mapCreator.targets_gray.ContainsKey(y1 * 100 + x1))
                            {
                                mapCreator.gray_num++;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                // determine if box is on the corresponding target
                switch (box_color)
                {
                    case 0:
                        {
                            if(mapCreator.targets_brown.ContainsKey(y2 * 100 + x2))
                            {
                                mapCreator.brown_num--;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 1:
                        {
                            if (mapCreator.targets_red.ContainsKey(y2 * 100 + x2))
                            {
                                mapCreator.red_num--;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (mapCreator.targets_blue.ContainsKey(y2 * 100 + x2))
                            {
                                mapCreator.blue_num--;
                                mapCreator.UpdateTargetNum();
                            }
                            // blue box make (x1, y1) to ice
                            if (isStone(x1, y1))
                            {
                                GameObject x1_y1_stone = getStone(x1, y1);
                                Destroy(x1_y1_stone);
                                mapCreator.stones.Remove(100 * y1 + x1);
                                GameObject new_x1_y1_ice = Instantiate(mapCreator.floor_ice,
                                    new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y, 5),
                                    Quaternion.identity);
                                new_x1_y1_ice.SetActive(true);
                                mapCreator.ices.Add(100 * y1 + x1, new_x1_y1_ice);
                            }
                            else if (isMud(x1, y1))
                            {
                                GameObject x1_y1_mud = getMud(x1, y1);
                                Destroy(x1_y1_mud);
                                mapCreator.muds.Remove(100 * y1 + x1);
                                GameObject new_x1_y1_ice = Instantiate(mapCreator.floor_ice,
                                    new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y, 5),
                                    Quaternion.identity);
                                new_x1_y1_ice.SetActive(true);
                                mapCreator.ices.Add(100 * y1 + x1, new_x1_y1_ice);
                            }
                            else if(isIce(x1, y1))
                            {
                                // here is ice
                            }
                            else
                            {
                                Debug.Log("Here should only be stone or mud or ice!");
                            }
                            break;
                        }
                    case 3:
                        {
                            if (mapCreator.targets_green.ContainsKey(y2 * 100 + x2))
                            {
                                mapCreator.green_num--;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    case 4:
                        {
                            if (mapCreator.targets_gray.ContainsKey(y2 * 100 + x2))
                            {
                                mapCreator.gray_num--;
                                mapCreator.UpdateTargetNum();
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        else if (isIce(x1, y1))
        {
            if (!isIce(x0, y0))
            {
                mapCreator.step++;
                mapCreator.UpdateStepNum();
                transform.position = new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y);
                return;
            }
            int ice_len = 0;
            while (isIce(x1 + ice_len * dx, y1 + ice_len * dy) && !isBox(x1 + ice_len * dx, y1 + ice_len * dy))
            {
                ice_len++;
            }
            if(isBox(x1 + ice_len * dx, y1 + ice_len * dy)
                || isWall(x1 + ice_len * dx, y1 + ice_len * dy)
                || isPit(x1 + ice_len * dx, y1 + ice_len * dy))
            {
                ice_len--;
            }
            
            mapCreator.step++;
            mapCreator.UpdateStepNum();
            // TODO: Need to add the process of walking on ice
            transform.position = new Vector3(x1 + ice_len * dx + mapCreator.map_offset_X, y1 + ice_len * dy + mapCreator.map_offset_Y);
            return;
        }
        else
        {
            mapCreator.step++;
            mapCreator.UpdateStepNum();
            transform.position = new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y);
        }
    }

    private void toAddFingerAction(ref int dx,ref int dy)
    {

        fingerTouchState = FINGER_STATE_ADD;

        if (Mathf.Abs(fingerSegmentX) > Mathf.Abs(fingerSegmentY))
        {
            fingerSegmentY = 0;
        }
        else
        {
            fingerSegmentX = 0;
        }

        if (fingerSegmentX == 0)
        {
            if (fingerSegmentY > 0)
            {
                //Debug.Log("up");
                dy++;
            }
            else
            {
                //Debug.Log("down");
                dy--;
            }
        }
        else if (fingerSegmentY == 0)
        {
            if (fingerSegmentX > 0)
            {
                //Debug.Log("right");
                dx++;
            }
            else
            {
                //Debug.Log("left");
                dx--;
            }
        }

    }


    bool isWall(int x, int y)
    {
        return mapCreator.walls.ContainsKey(y * 100 + x);
    }

    bool isBox(int x, int y)
    {
        return mapCreator.boxes.ContainsKey(y * 100 + x);
    }
    
    bool isPit(int x, int y)
    {
        return mapCreator.pits.ContainsKey(y * 100 + x);
    }

    bool isPitCovered(int x, int y)
    {
        return mapCreator.covered_pits.ContainsKey(y * 100 + x);
    }

    bool isStone(int x, int y)
    {
        return mapCreator.stones.ContainsKey(y * 100 + x);
    }

    bool isIce(int x, int y)
    {
        return mapCreator.ices.ContainsKey(y * 100 + x);
    }

    bool isMud(int x, int y)
    {
        return mapCreator.muds.ContainsKey(y * 100 + x);
    }

    GameObject getBox(int x, int y)
    {
        return mapCreator.boxes[y * 100 + x].Value;
    }
    
    GameObject getPit(int x, int y)
    {
        return mapCreator.pits[y * 100 + x];
    }

    GameObject getWall(int x, int y)
    {
        return mapCreator.walls[y * 100 + x];
    }

    GameObject getStone(int x, int y)
    {
        return mapCreator.stones[y * 100 + x];
    }

    GameObject getIce(int x, int y)
    {
        return mapCreator.ices[y * 100 + x];
    }

    GameObject getMud(int x, int y)
    {
        return mapCreator.muds[y * 100 + x];
    }

    void makeWallExpode(GameObject wall)
    {

        wall.GetComponent<Wall>().Explode();
    }

    void makeRedBoxExpode(GameObject red_box)
    {
        red_box.GetComponent<RedBox>().Explode();
    }

}
