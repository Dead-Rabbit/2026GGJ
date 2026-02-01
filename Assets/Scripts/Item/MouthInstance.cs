using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class MouthInstance : MonoBehaviour
{
    [Tooltip("吃饭音效")]
    [SerializeField] private AudioClip eatSound;

    private Animator _animator;

    [LabelText("吃饭时长")] public float EatingTime = 1;
    private float remainEatingTime = 0;
    private bool isEating = false;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isEating)
        {
            remainEatingTime -= Time.deltaTime;
            if (remainEatingTime <= 0)
            {
                isEating = false;
                _animator?.Play("idle");
            }
        }
        
        var hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Food"));
        if (hit != null)
        {
            var food = hit.GetComponentInParent<FoodInstance>();
            if (food)
            {
                Destroy(food.gameObject);
                
                GlobalEvent.Instance.OnEatFood?.Invoke();
                
                // 获得食物奖励
                GamePlay.Instance.CurrentDoubtValue -= food.RewardScore;
                
                // 播放音效
                if (eatSound)
                {
                    AudioSource.PlayClipAtPoint(eatSound, transform.position);
                }

                isEating = true;
                remainEatingTime = EatingTime;
                
                // 播放动画
                _animator?.Play("eat");
            }
        }
    }
}