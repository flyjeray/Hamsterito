using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossLogAttack : BossAttack
{
    public override IEnumerator Action()
    {
        yield return new WaitForSeconds(0);
        Debug.Log("Attack1");
    }
}
