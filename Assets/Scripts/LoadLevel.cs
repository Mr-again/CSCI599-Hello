﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public Text te;
    void Start()
    {
        //本地路径
        var fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels/level_1.txt");
        FileInfo fInfo0 = new FileInfo(fileAddress);
        if (fInfo0.Exists)
        {
            StreamReader r = new StreamReader(fileAddress);
            //StreamReader默认的是UTF8的不需要转格式了，因为有些中文字符的需要有些是要转的，下面是转成String代码
            //byte[] data = new byte[1024];
            // data = Encoding.UTF8.GetBytes(r.ReadToEnd());
            // s = Encoding.UTF8.GetString(data, 0, data.Length);
            string s = r.ReadToEnd();
            te.text = s;
            Debug.Log(s);
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}