using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterBase : MonoBehaviour
{
    #region Movement

    [LabelText("移动速度")] public float MaxMoveSpeed;

    #endregion

    #region Pate

    [LabelText("Pate头顶偏移")] public float PateOffset = 2.0f;

    #endregion
    
    [HideInInspector] public Vector2 MoveDireciton;

    private Animator _animator;

    private static readonly int SpeedX = Animator.StringToHash("SpeedX");
    private static readonly int SpeedY = Animator.StringToHash("SpeedY");

    /// <summary>
    /// 测试
    /// </summary>
    private PateInstance _testPateInstance;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        if (GlobalConfig.Instance.TestPate)
        {
            _testPateInstance = PateManager.Instance.CreateForTarget(GlobalConfig.Instance.TestPate, transform,
                new Vector3(0, PateOffset, 0), Vector2.zero);
        }
    }

    protected virtual void Update()
    {
        var dt = Time.deltaTime;

        var currentPos = transform.position;

        var velocity = Vector3.zero;
        MoveDireciton.Normalize();
        velocity = MoveDireciton * MaxMoveSpeed;
        
        transform.position = currentPos + velocity * dt;
        
        // 同步动画状态
        _animator?.SetFloat(SpeedX, velocity.x);
        _animator?.SetFloat(SpeedY, velocity.y);

    }

    protected void OnDestroy()
    {
        if (_testPateInstance)
        {
            PateManager.Instance.Remove(_testPateInstance);
        }
    }
}