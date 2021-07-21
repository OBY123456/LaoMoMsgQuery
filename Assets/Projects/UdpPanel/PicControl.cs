using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PicControl : MonoBehaviour
{
    public CanvasGroup canvasGroup, TipText;
    public Button LeftButton, RightButton;
    public RawImage rawImage;
    public List<Texture2D> PicGroup = new List<Texture2D>();
    private int Count = 0;
    RectTransform rect;
    float WidthMax = 400;
    float HeightMax = 300;

    private void Awake()
    {
        rawImage = FindTool.FindChildComponent<RawImage>(transform, "Image");
        LeftButton = FindTool.FindChildComponent<Button>(transform, "Image/LeftBtn");
        RightButton = FindTool.FindChildComponent<Button>(transform, "Image/RightBtn");
        canvasGroup = transform.GetComponent<CanvasGroup>();
        rect = FindTool.FindChildComponent<RectTransform>(transform, "Image");
        TipText = FindTool.FindChildComponent<CanvasGroup>(transform, "Text");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ExcelControl.Instance)
        {
            PicGroup = ExcelControl.Instance.PicGroup;
        }

        if(Config.Instance)
        {
            WidthMax = Config.Instance.configData.图片宽;
            HeightMax = Config.Instance.configData.图片高;
        }

        LeftButton.onClick.AddListener(() => {
            if (PicGroup.Count > 0)
            {
                Count--;
                if (Count < 0)
                {
                    Count = PicGroup.Count - 1;
                }
                SetTexture(PicGroup[Count]);
            }
        });

        RightButton.onClick.AddListener(() => {
            if (PicGroup.Count > 0)
            {
                Count++;
                if (Count > PicGroup.Count - 1)
                {
                    Count = 0;
                }
                SetTexture(PicGroup[Count]);

            }
        });
    }

    public void Open()
    {
        Count = 0;
        if (PicGroup.Count > 0)
        {
            TipText.Hide();
            SetTexture(PicGroup[Count]);
        }
        else
        {
            TipText.Open();
        }
        canvasGroup.Open();
    }

    public void Hide()
    {
        Count = 0;
        if (PicGroup.Count > 0)
        {
            rawImage.texture = null;
        }
        canvasGroup.Hide();
    }

    float width;
    float Height;
    private void SetTexture(Texture2D texture2D)
    {
        rawImage.texture = texture2D;
        width = texture2D.width;
        Height = texture2D.height;

        if (width == Height)
        {
            rect.sizeDelta = new Vector2(WidthMax, WidthMax);
        }
        else if (width > Height)
        {
            if (width > WidthMax)
            {
                rect.sizeDelta = new Vector2(WidthMax, WidthMax / width * Height);
            }
            else
            {
                rect.sizeDelta = new Vector2(width, Height);
            }
        }
        else
        {
            if (Height > HeightMax)
            {
                rect.sizeDelta = new Vector2(HeightMax / Height * width, HeightMax);
            }
            else
            {
                rect.sizeDelta = new Vector2(width, Height);
            }
        }
    }
}
