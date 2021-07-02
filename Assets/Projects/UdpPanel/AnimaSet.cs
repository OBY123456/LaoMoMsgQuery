using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaSet : MonoBehaviour
{
    public Animation anima;
    private string AnimaName = "CirleAnimation";

    private void Awake()
    {
        if(anima == null)
        anima = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        anima[AnimaName].normalizedTime = UnityEngine.Random.Range(0.0f, 1.0f);
        anima.Play();
    }

    private void OnDisable()
    {
        anima.Stop();
    }

    private void OnEnable()
    {
        anima[AnimaName].normalizedTime = UnityEngine.Random.Range(0.0f, 1.0f);
        anima.Play();
    }
}
