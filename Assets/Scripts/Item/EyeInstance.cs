using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeInstance : MonoBehaviour
{
    public float EnterMoveMinTime = 3;
    public float EnterMoveMaxTime = 8;

    private float _remainTime = 0;

    private Animator _animator;

    public void Start()
    {
        _remainTime = Random.Range(EnterMoveMinTime, EnterMoveMaxTime);
        _animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        _remainTime -= Time.fixedDeltaTime;
        if (_remainTime <= 0)
        {
            _remainTime = Random.Range(EnterMoveMinTime, EnterMoveMaxTime);
            _animator.Play("move");
        }
    }
}