using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MTFrame.MTPool;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class Seach : MonoBehaviour
{
    public Button SeachBtn, PicBtn, VideoBtn;
    public CanvasGroup 一级页面,二级页面;
    public InputField inputField;
    public ScrollRect scrollRect;
    public BigImage bigImage;
    public BigMsgSet msgSet;
    public VideoControl videoControl;
    public PicControl picControl;

    private List<PersonData> ResultList = new List<PersonData>();
    private List<GameObject> ResultObj = new List<GameObject>();
    private List<GameObject> bigImagePool = new List<GameObject>();

    public Dictionary<HeadData, PersonData> ShengJiMsg = new Dictionary<HeadData, PersonData>();
    public Dictionary<HeadData, PersonData> ShiJiMsg = new Dictionary<HeadData, PersonData>();
    public Dictionary<HeadData, PersonData> QuanGuoMsg = new Dictionary<HeadData, PersonData>();

    //百度手写输入法信息
    private string Path = @"C:\Program Files (x86)\Baidu\BaiduPinyin\Plugin\HandInput\1.0.0.138\HandInput.exe";
    private float width,height;
    private float PosX, PosY;

    private void Awake()
    {
        SeachBtn = FindTool.FindChildComponent<Button>(transform, "二级按钮/SeachBtn");
        PicBtn = FindTool.FindChildComponent<Button>(transform, "二级按钮/PicBtn");
        VideoBtn = FindTool.FindChildComponent<Button>(transform, "二级按钮/VideoBtn");
        inputField = FindTool.FindChildComponent<InputField>(transform, "二级界面/InputField");
        scrollRect = FindTool.FindChildComponent<ScrollRect>(transform, "二级界面/Scroll View");

        一级页面 = FindTool.FindChildComponent<CanvasGroup>(transform, "一级界面");
        二级页面 = FindTool.FindChildComponent<CanvasGroup>(transform, "二级界面");
        picControl = FindTool.FindChildComponent<PicControl>(transform, "PicGroup");
        videoControl = FindTool.FindChildComponent<VideoControl>(transform, "VideoGroup");
        bigImage = transform.GetComponent<BigImage>();
        msgSet = transform.GetComponent<BigMsgSet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SeachBtn.onClick.AddListener(() => {
            二级页面.Open();
            videoControl.Hide();
            picControl.Hide();
            一级页面.Hide();
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            OnPointClick();
        });

        PicBtn.onClick.AddListener(() => {
            videoControl.Hide();
            picControl.Open();
            一级页面.Hide();
            二级页面.Hide();
            ClearList();
            UDPSend.Instance.Send("close");
        });

        VideoBtn.onClick.AddListener(() => {
            videoControl.Open();
            picControl.Hide();
            一级页面.Hide();
            二级页面.Hide();
            ClearList();
            UDPSend.Instance.Send("close");
        });

        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        if(ExcelControl.Instance)
        {
            ShengJiMsg = ExcelControl.Instance.ShengJiMsg;
            ShiJiMsg = ExcelControl.Instance.ShiJiMsg;
            QuanGuoMsg = ExcelControl.Instance.QuanGuoMsg;
        }

        if(PoolManager.Instance)
        bigImagePool = PoolManager.BigimagePool.UsePool;

        if(Config.Instance)
        {
            width = Config.Instance.configData.百度手写框宽;
            height = Config.Instance.configData.百度手写框高;
        }
    }

    private void ValueChangeCheck()
    {
        if (ResultObj.Count > 0)
        {
            foreach (var item in ResultObj)
            {
                PoolManager.Instance.AddPool(PoolType.ResultText, item);
            }

            ResultObj.Clear();
            ResultList.Clear();

            ResultObj = new List<GameObject>();
            ResultList = new List<PersonData>();
        }

        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }

        foreach (HeadData item in ShengJiMsg.Keys)
        {
            if(item.Name.Contains(inputField.text))
            {
                ResultList.Add(ShengJiMsg[item]);
            }
        }

        foreach (HeadData item in ShiJiMsg.Keys)
        {
            if (item.Name.Contains(inputField.text))
            {
                ResultList.Add(ShiJiMsg[item]);
            }
        }

        foreach (HeadData item in QuanGuoMsg.Keys)
        {
            if (item.Name.Contains(inputField.text))
            {
                ResultList.Add(QuanGuoMsg[item]);
            }
        }

        if(ResultList.Count > 0)
        {
            for (int i = 0; i < ResultList.Count; i++)
            {
                GameObject obj = PoolManager.Instance.GetPool(PoolType.ResultText);
                obj.transform.SetParent(scrollRect.content);
                obj.GetComponent<Text>().text = ResultList[i].Name;
                InitBtn(obj.GetComponent<Button>(), ResultList[i]);
                ResultObj.Add(obj);
            }
        }
    }

    private void InitBtn(Button button,PersonData personData)
    {
        if(button.onClick != null)
        {
            button.onClick.RemoveAllListeners();
        }
        button.onClick.AddListener(() => {
            //首先判断是否是自己
            if(personData.Name == msgSet.Name.text && personData.Birthday == msgSet.Birthday)
            {
                Open();
                return;
            }
            //在判断是否在现有的大圆中,除去本身
            if (bigImagePool.Count > 1)
            {
                for (int i = 0; i < bigImagePool.Count; i++)
                {
                    if(bigImagePool[i].GetComponent<BigMsgSet>().Name.text == personData.Name && bigImagePool[i].GetComponent<BigMsgSet>().Birthday == personData.Birthday)
                    {
                        bigImagePool[i].transform.localPosition = transform.localPosition;
                        bigImage.CloseCircle();
                        bigImagePool[i].transform.GetComponent<Seach>().Open();
                        UDPSend.Instance.Send("close");
                        return;
                    }
                }
            }

            //啥都没有，那就只能用搜索到的信息覆盖掉当前信息了
            msgSet.SetText(personData.Type, personData.Sex, personData.Name, personData.Msg, personData.Birthday);
            Open();
            UDPSend.Instance.Send("close");
        });
    }

    private void OnEnable()
    {
        Open();
    }

    private void OnDisable()
    {
        ClearList();
    }

    string st;
    public void OnPointClick()
    {
        Vector3 vector = Vector3.zero;
#if UNITY_EDITOR
        string st = PosX.ToString("0") + "," + PosY.ToString("0") + "," + width + "," + height + "," + Path;
        UDPSend.Instance.Send(st);
        return;
#endif
        vector = Camera.main.WorldToScreenPoint(transform.localPosition);
        if (vector != Vector3.zero)
        {
            PosX = vector.x;
            PosY = Screen.height - vector.y;
        }
        else
        {
            return;
        }
        st = PosX.ToString("0") + "," + PosY.ToString("0") + "," + width + "," + height + "," + Path;
        UDPSend.Instance.Send(st);
    }

    private void ClearList()
    {
        if (ResultObj.Count > 0)
        {
            foreach (var item in ResultObj)
            {
                PoolManager.Instance.AddPool(PoolType.ResultText, item);
            }
        }
        ResultObj.Clear();
        ResultList.Clear();

        ResultObj = new List<GameObject>();
        ResultList = new List<PersonData>();

        inputField.text = "";
    }

    public void Open()
    {
        一级页面.Open();
        二级页面.Hide();
        videoControl.Hide();
        picControl.Hide();
        ClearList();
    }
}
