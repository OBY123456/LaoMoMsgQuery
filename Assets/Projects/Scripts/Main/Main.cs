using UnityEngine;
/// <summary>
/// 入口类
/// </summary>
public class Main : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 屏幕分辨率
    /// </summary>
    [Header("屏幕分辨率")]
    public Vector2Int resolution = new Vector2Int(1920, 1080);
    /// <summary>
    /// 是否全屏
    /// </summary>
    [Header("是否全屏")]
    public bool fullScreen = true;

#endif


    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);

#if UNITY_STANDALONE_WIN
        Screen.SetResolution(resolution.x, resolution.y, fullScreen);
#endif
       
        //AudioManager.Init();//音效初始化

    }
    private void Start()
    {
        Init();
    }
    /// <summary>
    ///初始化
    /// </summary>
    public void Init()
    {

#if UNITY_STANDALONE_WIN
        if(Config.Instance)
        Cursor.visible = Config.Instance.configData.isCursor;
#endif
        //在这里更改场景入口
        StateManager.ChangeState(new UIState());

    }
}
