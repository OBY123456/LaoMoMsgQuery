using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCircle_ImageItem : MyUIBase {

	public bool isInCircle=false;

    public Vector3 inCirclePos;

    public List<BigImage> bigImageList = new List<BigImage>();

    public Vector3 currentPos;
    public Vector3 targetPos;

    Transform bigShowParent;

    public bool isEnable=false;

    Vector3 offsetPos;

    Vector3 targetScale = Vector2.one;

    float CircleR = 700;

    Button selfButton;

    RawImage rawImage;

    public override void OnEnter()
    {
        isEnable = true;
        transform.localPosition= currentPos;
        targetPos = currentPos;
        bigImageList.Clear();
        transform.localScale=Vector3.one;
        isInCircle = false;
    }


    private void Start()
    {
        rawImage = this.GetComponent<RawImage>();
        selfButton = transform.GetComponent<Button>();

        if(Config.Instance)
        {
            CircleR = Config.Instance.configData.r;
        }

        bigShowParent = GameObject.Find("BigImagePanel").transform;
        selfButton.onClick.AddListener(() =>
        {
            GameObject newBigImage = PoolManager.Instance.GetPool(MTFrame.MTPool.PoolType.BigImage);
            newBigImage.transform.SetParent(bigShowParent);
            newBigImage.transform.localScale = Vector3.one;
            newBigImage.transform.localPosition = this.transform.localPosition;

            MsgSet msg = transform.GetComponent<MsgSet>();
            newBigImage.GetComponent<BigMsgSet>().SetText(msg.Type.text, msg.Sex.text, msg.Name.text, msg.Msg);
        });
    }

   
    bool isInLine(Vector3 selfPos,Vector3 CircclePos) {

        Vector2 v2 = (selfPos - CircclePos).normalized;
        float angle2 = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        return (angle2 == 0 || angle2 == 180);
    }
    
    private void FixedUpdate()
    {
        if (isEnable)
        {

            currentPos += Vector3.left * 2f;
            targetPos += Vector3.left * 2f;

            if (currentPos.x < -1800)
            {
                OnExit();
            }

            if (isInCircle)
            {

                Vector3 nor = (currentPos - bigImageList[0].currentPos).normalized;
                currentPos = bigImageList[0].currentPos + nor * CircleR;
                if (bigImageList.Count > 0)
                {
                    //剔除空物体
                    for (int i = 0; i < bigImageList.Count; i++)
                    {
                        if (bigImageList[i]==null)
                        {
                            bigImageList.RemoveAt(i);
                        }
                    }

                    //float scale = 0;
                    //scale = 1 - bigImageList.Count * 0.5f;
                    //scale = scale >= 0f ? scale : 0f;
                    //targetScale = Vector2.one *scale;
                }
                if (isInLine(currentPos, bigImageList[bigImageList.Count - 1].currentPos))
                {
                    //当圆与图片处于同一直线上时将图片移动目标位置向上提升50,防止卡死在一个位置上
                    offsetPos = new Vector3(0, 50f, 0);
                }
            }
            else
            {
                offsetPos = Vector3.zero;
                //targetScale = Vector3.one;
            }
            currentPos = Vector3.Lerp(currentPos, targetPos + offsetPos, 0.1f);
            //transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.1f);
            transform.localPosition = currentPos;
        }
    }

    public override void OnExit()
    {
        PoolManager.Instance.AddPool(MTFrame.MTPool.PoolType.CirleImage, gameObject);
    }

    public void Hide()
    {
        isEnable = false;
    }

    public void Open()
    {
        isEnable = true;
    }
}
