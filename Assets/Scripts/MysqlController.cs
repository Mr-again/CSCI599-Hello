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
        my.Execute("insert into t_schools(Name,Address) values(@Name,@Address)", new { Name = "西南大学", Address = "重庆市北碚区天生路2号" });
        //批量插入数据
        List<School> schools = new List<School>()
        {
            new School() {Address="China·BeiJing",Title="清华大学" },
            new School() {Address="杭州",Title="浙江大学" },
            new School() {Address="不知道，US?",Title="哈弗大学" }
        };
        //在执行参数化的SQL时，SQL中的参数（如@title可以和数据表中的字段不一致，但要和实体类型的属性Title相对应）
        my.Execute("insert into t_schools(Address,Name) values(@address,@title);", schools);
        //通过匿名类型批量插入数据
        my.Execute("insert into t_schools(Address,Name) values(@address,@name)", new[]
        {
            new {Address="杨浦区四平路1239号",Name="同济大学"},
            new {Address="英国",Name="剑桥"},
            new {Address="美国·硅谷",Name="斯坦福大学"}
        });
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

class School
{
    /*
      若属性名和数据库字段不一致（不区分大小写）则查询不出数据，如果使用EF则可以通过Column特性
      建立属性和数据表字段之间的映射关系，Dapper则不行
    */
    //[Column("Name")]
    public string Title { set; get; }
    public string Address { set; get; }
}

class Student
{
    public string Name { set; get; }
    public string Number { set; get; }
    public int SchoolId { set; get; }
}