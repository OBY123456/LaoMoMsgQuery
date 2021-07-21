using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VideoControl : MonoBehaviour
{
    public Button LeftButton, RightButton;
    public MediaPlayer mediaPlayer;
    public CanvasGroup canvasGroup,TipText;
    public List<string> VideoPath = new List<string>();
    private int Count = 0;

    private void Awake()
    {
        mediaPlayer = FindTool.FindChildComponent<MediaPlayer>(transform, "VideoPlayer");
        LeftButton = FindTool.FindChildComponent<Button>(transform, "VideoPlayer/LeftBtn");
        RightButton = FindTool.FindChildComponent<Button>(transform, "VideoPlayer/RightBtn");
        canvasGroup = transform.GetComponent<CanvasGroup>();
        TipText = FindTool.FindChildComponent<CanvasGroup>(transform, "Text");
        //RenderTexture renderTexture = new RenderTexture(400, 300, 24);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(ExcelControl.Instance)
        {
            VideoPath = ExcelControl.Instance.VideoPath;
        }

        LeftButton.onClick.AddListener(() => {
            if(VideoPath.Count > 0)
            {
                Count--;
                if(Count < 0)
                {
                    Count = VideoPath.Count - 1;
                }
                mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath[Count]);
            }
        });

        RightButton.onClick.AddListener(() => {
            if (VideoPath.Count > 0)
            {
                Count++;
                if (Count > VideoPath.Count - 1)
                {
                    Count = 0;
                }

                mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath[Count]);
            }
        });
    }

    public void Open()
    {
        Count = 0;
        if (VideoPath.Count > 0)
        {
            TipText.Hide();
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath[Count]);
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
        if (VideoPath.Count > 0)
        {
            mediaPlayer.Stop();
        }
        canvasGroup.Hide();
    }
}
