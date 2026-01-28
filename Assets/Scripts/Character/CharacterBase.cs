using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public Vector2 Velocity;

    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");

    /// <summary>
    /// 测试
    /// </summary>
    private PateInstance _testPateInstance;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (GlobalConfig.Instance.TestPate)
        {
            _testPateInstance = PateManager.Instance.CreateForTarget(GlobalConfig.Instance.TestPate, transform, Vector2.zero);
        }
    }

    protected virtual void Update()
    {
        _animator?.SetFloat(Speed, Velocity.magnitude);
    }

    protected void OnDestroy()
    {
        if (_testPateInstance)
        {
            PateManager.Instance.Remove(_testPateInstance);
        }
    }
}