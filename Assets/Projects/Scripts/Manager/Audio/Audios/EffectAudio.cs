﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MTFrame.MTAudio
{

    /// <summary>
    /// 效果音效
    /// </summary>
    public class EffectAudio : BaseAudio
    {

        /// <summary>
        /// 默认路径
        /// </summary>
        protected override string defaultPath
        {
            get
            {
                return AudioPath.effectAudioPath;
            }
            set { }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="parent">父体</param>
        /// <param name="audioParamete">音频参数</param>
        /// <returns></returns>
        public override AudioObject Play(string path, Transform parent, AudioParamete audioParamete)
        {
            return base.Play(path, parent, audioParamete);
        }

        /// <summary>
        /// 生成音频对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="trans"></param>
        /// <param name="audioParamete"></param>
        /// <returns></returns>
        protected override AudioObject AddAudioObject(string path, Transform trans, AudioParamete audioParamete)
        {
            AudioObject audioObject = base.AddAudioObject(path, trans, audioParamete);
            if (trans == null)
            {
                if (defaultParent)
                    audioObject.transform.parent = defaultParent;
                else
                {
                    defaultParent = new GameObject("EffsetAudioManage").transform;
                    GameObject.DontDestroyOnLoad(defaultParent);
                    audioObject.transform.parent = defaultParent;
                }
            }
            return audioObject;
        }
    }
}