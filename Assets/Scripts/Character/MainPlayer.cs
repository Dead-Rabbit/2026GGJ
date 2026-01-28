using System;
using UnityEngine;

public class MainPlayer : CharacterBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        MoveDireciton = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveDireciton.y = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveDireciton.y = -1;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveDireciton.x = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveDireciton.x = 1;
        }
        
        base.Update();
    }
}