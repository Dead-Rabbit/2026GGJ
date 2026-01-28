using UnityEngine;

public class MainPlayer : CharacterBase
{
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Velocity = new Vector2(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Velocity = new Vector2(0, 0);
        }
        
        base.Update();
    }
}