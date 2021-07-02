using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BigImage : MonoBehaviour
{
    public Button QuitButton;

    public List<GameObject> moveImageList;

    public Vector3 currentPos;

    BigImage bigImage;

    [Header("是否触摸")]
    public bool isTouch = true;
    [Header("未触摸时间")]
    public float touchTimer = 0;

    //判断是否开始检测周边移动图片
    [HideInInspector]
    public bool isCheck = false;

    float returnTimer = 200;
    float r = 700;
    float MaxScale = 2.5f;
    float MinScale = 1.0f;

    private void Awake()
    {
        bigImage = this.GetComponent<BigImage>();
        QuitButton = transform.Find("QuitButton").GetComponent<Button>();
    }

    private void OnEnable()
    {
        moveImageList = PoolManager.CirleimagePool.UsePool;
        isTouch = true;
        isCheck = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        QuitButton.onClick.AddListener(CloseCircle);
        if(Config.Instance)
        {
            returnTimer = Config.Instance.configData.Backtime;
            r = Config.Instance.configData.r;
            MaxScale = Config.Instance.configData.MaxScale;
            MinScale = Config.Instance.configData.MinScale;
        }
    }



    private void FixedUpdate()
    {
        if (isTouch)
        {
            touchTimer += Time.fixedDeltaTime;
            if (touchTimer > returnTimer)
            {
                CloseCircle();
                touchTimer = 0;
                isTouch = false;
                isCheck = false;
            }
        }
        if (!isCheck)
        {
            return;
        }
        for (int i = 0; i < moveImageList.Count; i++)
        {
            if (WaitPanel.IsInCircle(transform.localPosition, r, moveImageList[i].GetComponent<MyCircle_ImageItem>().currentPos))
            {
                if (!moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Contains(bigImage))
                {
                    moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Add(bigImage);
                    moveImageList[i].GetComponent<MyCircle_ImageItem>().isInCircle = true;
                }
            }
            else
            {
                if (moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Contains(bigImage))
                {
                    moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Remove(bigImage);
                    if (moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Count <= 0)
                    {
                        moveImageList[i].GetComponent<MyCircle_ImageItem>().isInCircle = false;
                    }
                }
            }
        }
        
        currentPos = this.transform.localPosition;
    }

    IEnumerator CloseCircleIE()
    {
        isCheck = false;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < moveImageList.Count; i++)
        {
            if (moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Contains(bigImage))
            {
                moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Remove(bigImage);
                if (moveImageList[i].GetComponent<MyCircle_ImageItem>().bigImageList.Count <= 0)
                {
                    moveImageList[i].GetComponent<MyCircle_ImageItem>().isInCircle = false;
                }
            }
        }
        PoolManager.Instance.AddPool(MTFrame.MTPool.PoolType.BigImage, gameObject);
    }

    public void CloseCircle()
    {
        StartCoroutine(CloseCircleIE());
    }

    private void Update()
    {
        if(transform.localScale.x > MaxScale )
        {
            transform.localScale = Vector3.one * MaxScale;
        }

        if(transform.localScale.x < MinScale)
        {
            transform.localScale = Vector3.one * MinScale;
        }
    }
}
