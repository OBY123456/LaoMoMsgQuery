using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BigMsgSet : MonoBehaviour
{
    public Text Type, Name, Sex, Msg;

    private void Awake()
    {
        Msg = transform.Find("msg").GetComponent<Text>();
        Type = transform.Find("type").GetComponent<Text>();
        Sex = transform.Find("sex").GetComponent<Text>();
        Name = transform.Find("name").GetComponent<Text>();
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="_Type">荣誉类别</param>
    /// <param name="_Sex">性别</param>
    /// <param name="_Name">姓名</param>
    /// <param name="_Msg">简介</param>
    public void SetText(string _Type, string _Sex, string _Name, string _Msg)
    {
        Type.text = _Type;
        Sex.text = _Sex;
        Name.text = _Name;
        Msg.text = _Msg;
    }
}
