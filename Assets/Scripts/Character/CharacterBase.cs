using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public Vector2 Velocity;

    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        _animator?.SetFloat(Speed, Velocity.magnitude);
    }
}