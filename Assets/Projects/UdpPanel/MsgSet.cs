using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MsgSet : MonoBehaviour
{
    public Text Type, Name, Sex;
    [HideInInspector]
    public string Msg;
    [HideInInspector]
    public string Birthday;

    private void Awake()
    {
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
    public void SetText(string _Type, string _Sex, string _Name,string _Msg,string _Birthday)
    {
        Type.text = _Type;
        Sex.text = _Sex;
        Name.text = _Name;
        Msg = _Msg;
        Birthday = _Birthday;
    }

    private void OnDisable()
    {
        if(WaitPanel.Instance)
        {
            HeadData headData = new HeadData();
            headData.Name = Name.text;
            headData.Birthday = Birthday;
            WaitPanel.Instance.CurrentListName.Remove(headData);
        }
        
    }
}
