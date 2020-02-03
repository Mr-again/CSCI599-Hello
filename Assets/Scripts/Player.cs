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
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            dy++;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            dy--;
        }

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

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        int nx = dx + x;
        int ny = dy + y;

        if (isWall(nx, ny))
        {
            return;
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
            inHandBox.transform.position = new Vector3(nnx, nny);
            mapCreator.boxes.Remove(nx * 100 + ny);
            mapCreator.boxes.Add(nnx * 100 + nny,inHandBox);
        }

        transform.position = new Vector3(x + dx, y + dy);
    }

    bool isWall(int x,int y)
    {
        return mapCreator.walls.Contains(x * 100 + y);
    }

    bool isBox(int x,int y)
    {
        return mapCreator.boxes.ContainsKey(x * 100 + y);
    }

    GameObject getBox(int x,int y)
    {
        return mapCreator.boxes[x * 100 + y];
    }
}
