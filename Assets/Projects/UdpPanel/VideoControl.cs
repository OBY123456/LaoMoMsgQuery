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
    public CanvasGroup canvasGroup,TipGroup,VideoGroup;
    public List<string> VideoPath = new List<string>();
    private int Count = 0;
    public bool IsPlay;

    private void Awake()
    {
        mediaPlayer = FindTool.FindChildComponent<MediaPlayer>(transform, "VideoPlayer");
        LeftButton = FindTool.FindChildComponent<Button>(transform, "VideoPlayer/LeftBtn");
        RightButton = FindTool.FindChildComponent<Button>(transform, "VideoPlayer/RightBtn");
        canvasGroup = transform.GetComponent<CanvasGroup>();
        TipGroup = FindTool.FindChildComponent<CanvasGroup>(transform, "tips");
        VideoGroup = FindTool.FindChildComponent<CanvasGroup>(transform, "VideoPlayer");
        //RenderTexture renderTexture = new RenderTexture(400, 300, 24);
    }

    // Start is called before the first frame update
    void Start()
    {

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
            TipGroup.Hide();
            VideoGroup.Open();
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath[Count]);
            IsPlay = true;
        }
        else
        {
            TipGroup.Open();
            VideoGroup.Hide();
            IsPlay = false;
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
        IsPlay = false;
    }
}
