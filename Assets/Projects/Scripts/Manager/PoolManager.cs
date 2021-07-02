
using MTFrame.MTPool;
using System.Collections.Generic;
using UnityEngine;

/// <remarks>对象池管理类</remarks>
public class PoolManager:MonoBehaviour
{
    public static PoolManager Instance;

    public static BigImagePool BigimagePool;
    [Header("大圆池预设")]
    public GameObject BigimagePrefabs;

    public static CirleImagePool CirleimagePool;
    [Header("小圆池预设")]
    public GameObject CirleImagePrefabs;

    private void Awake()
    {
        Instance = this;
        Init();
        
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        BigimagePool = new BigImagePool();
        BigimagePool.Init();

        CirleimagePool = new CirleImagePool();
        CirleimagePool.Init();
    }

    /// <summary>
    /// 放回对象池
    /// </summary>
    public void AddPool(PoolType poolType,GameObject go)
    {
        switch (poolType)
        {
            case PoolType.BigImage:
                BigimagePool.AddPool(go);
                break;
            case PoolType.CirleImage:
                CirleimagePool.AddPool(go);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 获取对象
    /// </summary>
    public GameObject GetPool(PoolType poolType)
    {
        GameObject t;
        switch (poolType)
        {
            case PoolType.BigImage:
                t= BigimagePool.GetPool();
                if(t == null)
                {
                    t = Instantiate(BigimagePrefabs);
                    BigimagePool.UsePool.Add(t);
                }
                break;
            case PoolType.CirleImage:
                t = CirleimagePool.GetPool();
                if (t == null)
                {
                    t = Instantiate(CirleImagePrefabs);
                    CirleimagePool.UsePool.Add(t);
                }
                break;
            default:
                t = null;
                break;
        }
        return t;
    }

    private void OnDestroy()
    {
        BigimagePool.Clear();
        CirleimagePool.Clear();
    }
}