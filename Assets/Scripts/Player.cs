using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MapCreator mapCreator;
    private Animator animator;
    private int x_dir = 0;
    private int y_dir = 0;

    private void Awake()
    {
        mapCreator = FindObjectOfType<MapCreator>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int dx = 0;
        int dy = 0;
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
            if(isWall(x2, y2))
            {
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
                GameObject newPit;
                switch (box_color)
                {
                    case 0:
                        {
                            newPit = Instantiate(mapCreator.pit_brown,
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 5), Quaternion.identity);
                            break;
                        }
                    case 1:
                        {
                            newPit = Instantiate(mapCreator.pit_red,
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 5), Quaternion.identity);
                            break;
                        }
                    case 2:
                        {
                            newPit = Instantiate(mapCreator.pit_blue,
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 5), Quaternion.identity);
                            break;
                        }
                    case 3:
                        {
                            newPit = Instantiate(mapCreator.pit_green,
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 5), Quaternion.identity);
                            break;
                        }
                    case 4:
                        {
                            newPit = Instantiate(mapCreator.pit_gray,
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 5), Quaternion.identity);
                            break;
                        }
                    default:
                        {
                            newPit = Instantiate(mapCreator.pit,
                                new Vector3(x2 + mapCreator.map_offset_X, y2 + mapCreator.map_offset_Y, 5), Quaternion.identity);
                            break;
                        }


                }
                newPit.SetActive(true);
                mapCreator.covered_pits.Add(100 * y2 + x2, newPit);

                mapCreator.step++;
                mapCreator.UpdateStepNum();
                transform.position = new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y);
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
            }
        }
        else
        {
            mapCreator.step++;
            mapCreator.UpdateStepNum();
            transform.position = new Vector3(x1 + mapCreator.map_offset_X, y1 + mapCreator.map_offset_Y);
        }
    }

    bool isWall(int x, int y)
    {
        return mapCreator.walls.Contains(y * 100 + x);
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
    GameObject getBox(int x, int y)
    {
        return mapCreator.boxes[y * 100 + x].Value;
    }
    
    GameObject getPit(int x, int y)
    {
        return mapCreator.pits[y * 100 + x];
    }

    
}
