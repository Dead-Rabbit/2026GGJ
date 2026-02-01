using System.Collections;
using Framework;
using UnityEngine;
using Object = UnityEngine.Object;

public class WaiterInstance : MonoBehaviour
{
    private Animator _animator;

    public FoodInstance _food;

    public Transform FoodSocket;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Start()
    {
        GamePlay.Instance.WaiterInstance = this;
    }

    public void Enter()
    {
        _animator.Play("Move_In");

        StartCoroutine(StartThrowFood());
    }

    private IEnumerator StartThrowFood()
    {
        if (!_food)
            yield break;

        Instantiate(_food);
        
        yield return new WaitForSeconds(1.0f);

        Leave();
    }

    public void Leave()
    {
        _animator.Play("Move_Out");
    }
}