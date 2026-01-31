using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCenterPosition : MonoBehaviour
{
    private List<Transform> _children = new();

    private Vector3 _defaultOffset = Vector3.zero;

    public void Awake()
    {
        if (transform.childCount <= 0)
        {
            return;
        }
        
        Vector3 defaultCenter = Vector3.zero;
        for (int i = 0; i < transform.childCount; i++)
        {
            var childTransform = transform.GetChild(i);
            _children.Add(childTransform);

            defaultCenter += childTransform.position;
        }
        defaultCenter /= transform.childCount;

        var currentPosition = transform.position;
        _defaultOffset = defaultCenter - currentPosition;
    }

    public void Update()
    {
        if (_children.Count == 0)
            return;
        
        Vector3 position = Vector3.zero;
        foreach (Transform option in _children)
        {
            position += option.transform.position;
        }

        transform.position = position / _children.Count - _defaultOffset;
    }
}
