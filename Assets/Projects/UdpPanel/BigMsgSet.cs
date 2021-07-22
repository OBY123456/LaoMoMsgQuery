using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BigMsgSet : MonoBehaviour
{
    public Text Type, Name, Sex, Msg;
    public VideoControl videoControl;
    public PicControl picControl;
    public string Birthday;

    private void Awake()
    {
        Msg = FindTool.FindChildComponent<Text>(transform, "一级界面/msg");
        Type = FindTool.FindChildComponent<Text>(transform, "一级界面/type"); 
        Sex = FindTool.FindChildComponent<Text>(transform, "一级界面/sex"); 
        Name = FindTool.FindChildComponent<Text>(transform, "一级界面/name");
        videoControl = FindTool.FindChildComponent<VideoControl>(transform, "VideoGroup");
        picControl = FindTool.FindChildComponent<PicControl>(transform, "PicGroup");
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="_Type">荣誉类别</param>
    /// <param name="_Sex">性别</param>
    /// <param name="_Name">姓名</param>
    /// <param name="_Msg">简介</param>
    public void SetText(string _Type, string _Sex, string _Name, string _Msg,string _Birthday)
    {
        Type.text = _Type;
        Sex.text = _Sex;
        Name.text = _Name;
        Msg.text = _Msg;
        Birthday = _Birthday;

        if (ExcelControl.Instance)
        {
            if(ExcelControl.Instance.PicGroup.ContainsKey(_Name))
            {
                picControl.PicGroup = ExcelControl.Instance.PicGroup[_Name];
            }

            if(ExcelControl.Instance.VideoGroup.ContainsKey(_Name))
            {
                videoControl.VideoPath = ExcelControl.Instance.VideoGroup[_Name];
            }
        }
    }
}
