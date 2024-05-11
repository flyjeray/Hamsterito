using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossLogAttackTwo : BossAttack
{
    public override IEnumerator Action(int phase)
    {
        yield return new WaitForSeconds(0);
        Debug.Log("Attack2");
    }
}
