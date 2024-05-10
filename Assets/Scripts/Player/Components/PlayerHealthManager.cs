using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : HealthManager
{
    protected override void OnLethalDamageTaken()
    {
        Debug.Log("End game");
        Destroy(gameObject);
    }
}
