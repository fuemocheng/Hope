using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TestRegex : MonoBehaviour
{
    
    void Start()
    {
        TestFun();
    }

    
    void Update()
    {
        
    }

    void TestFun()
    {
        string msg = "你aaa好aa哈哈a";
        msg = Regex.Replace(msg, @"a+", "A");
        Debug.LogError(msg);


        string msg2 = "Hello 'welcome' to 'china'";

        msg2 = Regex.Replace(msg2, "'(.+?)'", "[$1]");
        Debug.LogError(msg2);


        string msg3 = "文天祥12345678911";
        msg3 = Regex.Replace(msg3, "([0-9]{4})[0-9]{4}([0-9]{3})", "$1****$2");
        Debug.LogError(msg3);


        string msg4 = "123456789@qq.com";
        msg4 = Regex.Replace(msg4, "(.+?)@", "*****");
        Debug.LogError(msg4);

        string msg5 = "对话开头#对话男|对话女#,对话结尾";
        Match msg6 = Regex.Match(msg5, $"(#.+?#)");
        string[] strArr = msg6.ToString().Substring(1, msg6.Length - 2).Split('|');

        int sex = 0;
        if (sex == 0)
        {
            string a = Regex.Replace(msg5, $"(#.+?#)", strArr[0]);
            Debug.LogError(a);
        }
        else
        {
            string b = Regex.Replace(msg5, $"(#.+?#)", strArr[1]);
            Debug.LogError(b);
        }
        //Debug.LogError(msg6.ToString().Substring(1, msg6.Length - 2));

    }
}
