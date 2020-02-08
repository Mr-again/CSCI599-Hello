using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;

public class MysqlController : MonoBehaviour
{
    private string mysqlconnectionString = "Server=127.0.0.1;Database=test;User=root;Password=00000000;charset=utf8;";

    public static MysqlController instance;

    // Start is called before the first emoframe update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MySqlConnection my = OpenConnection();
        Debug.Log(my);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public MySqlConnection OpenConnection()
    {
        MySqlConnection connection = new MySqlConnection(mysqlconnectionString);
        connection.Open();
        return connection;
    }
}