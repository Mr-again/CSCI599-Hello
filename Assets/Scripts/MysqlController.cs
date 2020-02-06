using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;

public class MysqlController : MonoBehaviour
{
    public string mysqlconnectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=00000000;";

    // Start is called before the first frame update
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