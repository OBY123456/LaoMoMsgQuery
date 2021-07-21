using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ConfigData
{
    /// <summary>
    /// 返回待机页时间
    /// </summary>
    public int Backtime;

    public string Ip;

    /// <summary>
    /// UDP端口号
    /// </summary>
    public int Port;

    /// <summary>
    /// 是否显示鼠标
    /// </summary>
    public bool isCursor;

    /// <summary>
    /// 圆半径
    /// </summary>
    public float r;

    /// <summary>
    /// Logo码
    /// </summary>
    public string Logo;

    /// <summary>
    /// 大圆最大缩放
    /// </summary>
    public float MaxScale;

    /// <summary>
    /// 大圆最小缩放
    /// </summary>
    public float MinScale;

    public float 百度手写框宽;

    public float 百度手写框高;

    public float 图片宽;

    public float 图片高;

    public bool 是否开启软件前置;
}


public class Config : MonoBehaviour
{
    public static Config Instance;

    public ConfigData configData  = new ConfigData();

    private string File_name = "config.txt";
    private string Path;

    private void Awake()
    {
        Instance = this;
        configData = new ConfigData();
#if UNITY_STANDALONE_WIN
        Path = Application.streamingAssetsPath + "/" + File_name;
        if (FileHandle.Instance.IsExistFile(Path))
        {
            string st = FileHandle.Instance.FileToString(Path);
            LogMsg.Instance.Log(st);
            configData = JsonConvert.DeserializeObject<ConfigData>(st);
        }
#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        Path = Application.persistentDataPath + "/" + File_name;
        if(FileHandle.Instance.IsExistFile(Path))
        {
            string st = FileHandle.Instance.FileToString(Path);
            configData = JsonConvert.DeserializeObject<ConfigData>(st);
        }
        else
        {
            Path = Application.streamingAssetsPath + "/" + File_name;
            if (FileHandle.Instance.IsExistFile(Path))
            {
                string st = FileHandle.Instance.FileToString(Path);
                configData = JsonConvert.DeserializeObject<ConfigData>(st);
            }
        }
#endif
    }

    private void OnDestroy()
    {
        //SaveData();
    }

    public void SaveData()
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
         Path = Application.persistentDataPath + "/" + File_name;
#endif
        string st = JsonConvert.SerializeObject(configData);
        FileHandle.Instance.SaveFile(st, Path);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
