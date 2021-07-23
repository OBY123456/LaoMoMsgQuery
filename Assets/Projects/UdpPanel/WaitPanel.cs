using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using System;
using Newtonsoft.Json;

public class WaitPanel : BasePanel
{
    public static WaitPanel Instance;

    public Transform Cirles;

    bool isStartShow = false;

    List<Vector3> initPos = new List<Vector3>();

    bool isstart = true;

    //行
    int row = 10;
    //列
    int column = 7;

    float XOffset = 5;

    float YOffset = 5;

    // [Header("起始位置")]
    Vector3 startPos = new Vector3(1800, 130, 0);

    Vector3 originalPos = Vector3.zero;

    float createTimer = 0;

    float timer = 0;

    [HideInInspector]
    public List<HeadData> CurrentListName;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        CurrentListName = new List<HeadData>();
    }

    protected override void Start()
    {
        base.Start();
        if(ExcelControl.Instance)
        {
            createTimer = (XOffset + 320) / (2f * (1 / Time.fixedDeltaTime));
            StartCoroutine(LoadImage());
        }
    }

    public override void InitFind()
    {
        base.InitFind();
        Cirles = FindTool.FindChildNode(transform, "Cirles");
    }

    public static bool IsInCircle(Vector2 CirclePoint, float r, Vector2 point)
    {
        return Mathf.Sqrt((point.x - CirclePoint.x) * (point.x - CirclePoint.x) + (point.y - CirclePoint.y) * (point.y - CirclePoint.y)) < r;
    }

    private void FixedUpdate()
    {
        if (isStartShow)
        {
            timer += Time.deltaTime;
            if (timer >= createTimer)
            {
                timer = 0;
                for (int i = 0; i < initPos.Count; i++)
                {
                    GameObject newImage = PoolManager.Instance.GetPool(MTFrame.MTPool.PoolType.CirleImage);
                    newImage.SetActive(true);
                    newImage.transform.SetParent(Cirles);
                    newImage.transform.localScale = Vector3.one;
                    newImage.GetComponent<RectTransform>().sizeDelta = Vector2.one * 200;
                    newImage.GetComponent<MyCircle_ImageItem>().currentPos = initPos[i];
                    newImage.GetComponent<MyCircle_ImageItem>().OnEnter();
                    GetMsg(newImage.GetComponent<MsgSet>());
                }
            }
        }


    }

    //加载图片
    IEnumerator LoadImage()
    {
        for (int i = 0; i < row; i++) //行
        {
            Vector3 resetPos = Vector3.zero;
            for (int j = 0; j < column; j++) //列
            {
                //i*200f展示错开效果  320 180 移动图片的宽、高
                Vector3 newPos = new Vector3(j * (320f + XOffset) - Screen.width / 2 + startPos.x + i * 200f, i * (180f + YOffset) - Screen.height / 2 + startPos.y, 0);
                GameObject newImage = PoolManager.Instance.GetPool(MTFrame.MTPool.PoolType.CirleImage) /*MyCirle.MessageEvent.instance.GetNewMoveImage()*/;
                newImage.transform.SetParent(Cirles);
                newImage.transform.localScale = Vector3.one;
                newImage.GetComponent<RectTransform>().sizeDelta = Vector2.one * 200;
                resetPos = new Vector3((column - 1) * (320f + XOffset) - Screen.width / 2 + startPos.x + i * 200f, i * (180f + YOffset) - Screen.height / 2 + startPos.y, 0);
                newImage.GetComponent<MyCircle_ImageItem>().currentPos = newPos;
                newImage.transform.localPosition = newPos;
                newImage.GetComponent<MyCircle_ImageItem>().OnEnter();
                GetMsg(newImage.GetComponent<MsgSet>());
            }
            initPos.Add(resetPos);
        }
        yield return new WaitForSeconds(0.2f);
        isStartShow = true;
    }

    private void GetMsg(MsgSet msgSet)
    {
        PersonData personData = new PersonData();
        personData = ExcelControl.Instance.GetPersonMsg();
        if(personData != null)
        {
            msgSet.SetText(personData.Type, personData.Sex, personData.Name, personData.Msg,personData.Birthday);
            HeadData headData = new HeadData();
            headData.Name = personData.Name;
            headData.Birthday = personData.Birthday;
        }
    }
}
