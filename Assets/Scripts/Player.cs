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

        if (dx != 0 || dy != 0)
        {
            x_dir = dx;
            y_dir = dy;
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("deltaY", y_dir);
        animator.SetFloat("deltaX", x_dir);

        int x = (int)transform.position.x- mapCreator.map_offset_X;
        int y = (int)transform.position.y-mapCreator.map_offset_Y;

        int nx = dx + x;
        int ny = dy + y;

        if (isWall(nx, ny))
        {
            return;
        }

        if (dx != 0 || dy != 0)
        {
            mapCreator.step++;
            mapCreator.UpdateStepNum();
        }

        if (isBox(nx, ny))
        {
            int nnx = nx + dx;
            int nny = ny + dy;
            if (isWall(nnx, nny) || isBox(nnx, nny))
            {
                return;
            }
            GameObject inHandBox = getBox(nx, ny);
            inHandBox.transform.position = new Vector3(nnx+ mapCreator.map_offset_X, nny+mapCreator.map_offset_Y);
            int box_color = mapCreator.boxes[ny * 100 + nx].Key;
            mapCreator.boxes.Remove(ny * 100 + nx);
            mapCreator.boxes.Add(nny * 100 + nnx, new KeyValuePair<int, GameObject>(box_color,inHandBox));
        }

        transform.position = new Vector3(x + dx+ mapCreator.map_offset_X, y + dy+ mapCreator.map_offset_Y);
    }

    bool isWall(int x, int y)
    {
        return mapCreator.walls.Contains(y * 100 + x);
    }

    bool isBox(int x, int y)
    {
        return mapCreator.boxes.ContainsKey(y * 100 + x);
    }

    GameObject getBox(int x, int y)
    {
        return mapCreator.boxes[y * 100 + x].Value;
    }
}
