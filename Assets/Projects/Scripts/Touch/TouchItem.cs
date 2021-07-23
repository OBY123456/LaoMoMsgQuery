using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchItem : MonoBehaviour {
    
     public bool isBegin = false;
    Vector3 newpos;

  //  int fingerIndex = -1;

    MyTouch myTouch;

    Vector2 firstMousePos = Vector3.zero;

    //鼠标拖拽的位置

    Vector2 secondMousePos = Vector3.zero;
    Vector3 offsetPos = Vector3.zero;

    [HideInInspector]
    public bool isIntouch = false;
    
    public List<Touch> touchList = new List<Touch>();


    public int count;
    float ScaleMax = 2.5f;
    float ScaleMin = 1.0f;

    private void Awake()
    {
        myTouch = GameObject.FindObjectOfType<MyTouch>();
    }

    private void Start()
    {
        myTouch.myTouchs.Add(this.gameObject);
        if(Config.Instance)
        {
            ScaleMax = Config.Instance.configData.MaxScale;
            ScaleMin = Config.Instance.configData.MinScale;
        }
    }

    Touch oldTouch1;
    Touch oldTouch2;
    Touch newTouch1;
    Touch newTouch2;
    int fingerId1;
    int fingerId2;
    private void Update()
    {
        count = touchList.Count;

       
        if (touchList.Count>1)
        {
            if (!isScale)
            {
                return;
            }
            if (fingerId1==-1||fingerId2==-1)
            {
                return;
            }
            try
            {
                for (int i = 0; i < Input.touches.Length; i++)
                {
                    if (Input.touches[i].fingerId == fingerId1)
                    {
                        newTouch1 = Input.touches[i];
                    }
                }

                for (int i = 0; i < Input.touches.Length; i++)
                {
                    if (Input.touches[i].fingerId == fingerId2)
                    {
                        newTouch2 = Input.touches[i];
                    }
                }
            }
            catch
            {
                isScale = false;
                fingerId1 = -1;
                fingerId2 = -1;
            }
            //newTouch1 = Input.GetTouch(fingerId1);
            //newTouch2 = Input.GetTouch(fingerId2);
            
            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型

            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);

            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

            //两个距离之差，为正表示放大手势， 为负表示缩小手势

            float offset = newDistance - oldDistance;
          //  print(offset);



            //放大因子， 一个像素按 0.01倍来算(100可调整)

            float scaleFactor = offset / 200f;

            Vector3 localScale = transform.localScale;

            Vector3 scale = new Vector3(localScale.x + scaleFactor,

            localScale.y + scaleFactor,

            localScale.z + scaleFactor);

            //在什么情况下进行缩放

            float _x = Mathf.Clamp(scale.x, ScaleMin, ScaleMax);
            float _y = Mathf.Clamp(scale.y, ScaleMin, ScaleMax);
            scale = new Vector3(_x, _y, scale.z);

           // if (scale.x >= 0.5f && scale.y >= 0.5f && scale.z >= 0.5f)
         //   {
                transform.localScale = scale;
          //  }
            
            oldTouch1 = newTouch1;

            oldTouch2 = newTouch2;
        }
    }

    bool isScale = false;
    public void SecondFingerBeganDrag()
    {
        try
        {
            fingerId1 = touchList[0].fingerId;
            fingerId2 = touchList[1].fingerId;
            //   print(fingerId1 + "--" + fingerId2);
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == fingerId1)
                {
                    newTouch1 = Input.touches[i];
                }
            }

            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == fingerId2)
                {
                    newTouch2 = Input.touches[i];
                }
            }
            //newTouch1 = Input.GetTouch(fingerId1);
            //newTouch2 = Input.GetTouch(fingerId2);
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            isScale = true;
        }
        catch
        {
            isScale = false;
            fingerId1 = -1;
            fingerId2 = -1;
        }
      
    }


    void FuncInv() {
        isScale = true;
    }


    public void OnTouchBegin(Touch mytouch)
    {
        try
        {
            isBegin = true;

            newpos = new Vector3(mytouch.position.x, mytouch.position.y, transform.localPosition.z);

            firstMousePos = newpos;
            //transform.SendMessage("GetStatusFromTouchItem", true);
        }
        catch
        {
            isBegin = false;
        }
    }
    
   public void OnTouchDrag(Touch mytouch) {
        if (isBegin)
        {
              try
              {
                newpos = new Vector3(mytouch.position.x, mytouch.position.y, transform.localPosition.z);

                secondMousePos = newpos;
                offsetPos = secondMousePos - firstMousePos;

                float x = transform.localPosition.x;

                float y = transform.localPosition.y;
                x = x + offsetPos.x; y = y + offsetPos.y;

                transform.localPosition = new Vector3(x, y, transform.localPosition.z);
                firstMousePos = secondMousePos;
            }
            catch 
            {
                isBegin = false;
            }
        }
    }

    public void OnTouchEnd(Touch mytouch)
    {
        if (touchList.Count<=0)
        {
            return;
        }

        if (mytouch.fingerId == touchList[0].fingerId)
        {
            isBegin = false;
            isScale = false;
            fingerId1 = -1;
            fingerId2 = -1;
        }
        for (int i = 0; i < touchList.Count; i++)
        {
            if (mytouch.fingerId == touchList[i].fingerId)
            {
                touchList.RemoveAt(i);
            }
        }
    }
}
