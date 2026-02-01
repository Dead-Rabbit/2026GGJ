using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntryBackground : MonoBehaviour
{
    private Animator _animator;
    
    public float PlayEyeMinTime = 1.0f;
    public float PlayEyeMaxTime = 5.0f;
    private float remainPlayEyeTime = 0;

    private bool valid = false;

    public void Awake()
    {
        valid = true;
        _animator = GetComponentInChildren<Animator>();
        remainPlayEyeTime = Random.Range(PlayEyeMinTime, PlayEyeMaxTime);

        GlobalEvent.Instance.OnStart += OnStart;
    }
    
    public void Update()
    {
        var dt = Time.deltaTime;

        if (valid && remainPlayEyeTime <= 0)
        {
            remainPlayEyeTime = Random.Range(PlayEyeMinTime, PlayEyeMaxTime);
            _animator.Play("white");
        }
        else
        {
            remainPlayEyeTime -= dt;
        }
    }

    public void OnDestroy()
    {
        GlobalEvent.Instance.OnStart -= OnStart;
    }

    private void OnStart()
    {
        valid = false;
        // 开始眨眼
        _animator.Play("red", 0, 0);
    }
}
