using UnityEngine;
using Framework;

public class RattleInstance : PickableItem
{
    [Header("摇铃触发")]
    [Tooltip("横向晃动速度超过此值（单位/秒）时触发")]
    [SerializeField] private float speedThreshold = 5f;
    [Tooltip("触发后冷却时间（秒），避免一次晃动连续触发")]
    [SerializeField] private float cooldown = 0.2f;

    [Header("声音")]
    [Tooltip("摇铃音效")]
    [SerializeField] private AudioClip rattleSound;
    [Tooltip("播放用的 AudioSource，空则使用 AudioSource.PlayClipAtPoint")]
    [SerializeField] private AudioSource audioSource;

    private Vector3 _lastPosition;
    private float _lastTriggerTime = -999f;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (!IsHolding)
        {
            _lastPosition = transform.position;
            return;
        }

        float dt = Time.deltaTime;
        if (dt <= 0f)
        {
            _lastPosition = transform.position;
            return;
        }

        Vector3 delta = transform.position - _lastPosition;
        float speedX = Mathf.Abs(delta.x) / dt;

        if (speedX >= speedThreshold && Time.time - _lastTriggerTime >= cooldown)
        {
            _lastTriggerTime = Time.time;
            PlayRattleSound();
            GlobalEvent.Instance.OnRattleWorlk?.Invoke();
        }

        _lastPosition = transform.position;
    }

    private void PlayRattleSound()
    {
        if (rattleSound == null) return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(rattleSound);
        }
        else
        {
            AudioSource.PlayClipAtPoint(rattleSound, transform.position);
        }
    }
}
