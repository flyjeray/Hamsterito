using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogBehavior : EnemyBehavior
{
    protected override void OnPlayerLost()
    {
        Debug.Log("Player lost");
    }

    protected override void OnPlayerNoticed()
    {
        Debug.Log("Player noticed");
    }
}
