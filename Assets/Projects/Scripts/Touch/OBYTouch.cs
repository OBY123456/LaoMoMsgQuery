using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class OBYTouch : MonoBehaviour
{
    public int finger1, finger2;

    public bool IsTouch;

    private float ScaleMax = 2.5f;
    private float ScaleMin = 1.0f;
    Vector3 offsetPos = Vector3.zero;

    private void Awake()
    {
        finger1 = finger2 = -1;
        if(!transform.GetComponent<EventTrigger>())
        transform.gameObject.AddComponent<EventTrigger>();
    }

    Touch touch, touch1, touch2;
    Vector2 oldTouch1;
    Vector2 oldTouch2;
    Vector2 newTouch1;
    Vector2 newTouch2;
    // Update is called once per frame
    void Update()
    {
        if(IsTouch)
        {
            //缩放
            if(finger1 != -1 && finger2 != -1)
            {
                try
                {
                    for (int i = 0; i < Input.touches.Length; i++)
                    {
                        if (Input.touches[i].fingerId == finger1)
                        {
                            touch1 = Input.touches[i];
                        }
                    }

                    for (int i = 0; i < Input.touches.Length; i++)
                    {
                        if (Input.touches[i].fingerId == finger2)
                        {
                            touch2 = Input.touches[i];
                        }
                    }
                }
                catch
                {
                    finger1 = -1;
                    finger2 = -1;
                    oldTouch1 = oldTouch2 = newTouch1 = newTouch2 = Vector2.zero;
                    return;
                }

                newTouch1 = touch1.position;
                newTouch2 = touch2.position;

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型

                float oldDistance = Vector2.Distance(oldTouch1, oldTouch2);

                float newDistance = Vector2.Distance(newTouch1, newTouch2);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势

                float offset = newDistance - oldDistance;

                float scaleFactor = offset / 200f;

                Vector3 localScale = transform.localScale;

                Vector3 scale = new Vector3(localScale.x + scaleFactor,

                localScale.y + scaleFactor,

                localScale.z + scaleFactor);

                //在什么情况下进行缩放

                float _x = Mathf.Clamp(scale.x, ScaleMin, ScaleMax);
                float _y = Mathf.Clamp(scale.y, ScaleMin, ScaleMax);
                scale = new Vector3(_x, _y, scale.z);

                //if(oldTouch1 != Vector2.zero)
                transform.localScale = scale;

                oldTouch1 = newTouch1;

                oldTouch2 = newTouch2;
            }
            else//移动 拖曳
            {
                try
                {
                    if (finger1 != -1)
                    {
                        for (int i = 0; i < Input.touches.Length; i++)
                        {
                            if (Input.touches[i].fingerId == finger1)
                            {
                                touch = Input.touches[i];
                            }
                        }
                    }

                    if (finger2 != -1)
                    {
                        for (int i = 0; i < Input.touches.Length; i++)
                        {
                            if (Input.touches[i].fingerId == finger2)
                            {
                                touch = Input.touches[i];
                            }
                        }
                    }
                }
                catch
                {
                    finger1 = -1;
                    finger2 = -1;
                    return;
                }

                transform.position = touch.position;
            }
        }
    }

    public void PointUp()
    {
        Touch[] touches = Input.touches;

        for (int i = 0; i < touches.Length; i++)
        {
            if (touches[i].phase == TouchPhase.Ended)
            {
                if (finger1 != -1)
                {
                    if(finger1 == touches[i].fingerId)
                    {
                        finger1 = -1;
                        oldTouch1 = newTouch1 = Vector2.zero;
                        return;
                    }
                }

                if (finger2 != -1)
                {
                    if (finger2 == touches[i].fingerId)
                    {
                        finger2 = -1;
                        oldTouch2 = newTouch2 = Vector2.zero;
                    }
                }
            }
        }

        if(finger1 == -1 && finger2 == -1)
        {
            IsTouch = false;
            oldTouch1 = oldTouch2 = newTouch1 = newTouch2 =Vector2.zero;
        }
    }

    public void PointDown()
    {
        if (finger1 != -1 && finger2 != -1)
            return;

        Touch[] touches = Input.touches;

        for (int i = 0; i < touches.Length; i++)
        {
            if (touches[i].phase == TouchPhase.Began)
            {
                if(finger1 == -1)
                {
                    finger1 = touches[i].fingerId;
                    return;
                }
                    
                if (finger2 == -1)
                {
                    finger2 = touches[i].fingerId;
                }
                    
            }
        }

       IsTouch = true;
    }

    public void OnMouseDrag()
    {
        IsTouch = true;
    }
}
