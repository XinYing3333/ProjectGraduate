using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject myPlayer; 
    public CombatSystem combatSystem; 

    private void Start()
    {
        combatSystem = myPlayer.GetComponent<CombatSystem>();
        if (combatSystem != null)
        {
            combatSystem.OnPlayerAttack -= HandlePlayerAttack; // 確保無重複訂閱
            combatSystem.OnPlayerAttack += HandlePlayerAttack;
        }
    }

    private void OnDisable()
    {
        if (combatSystem != null)
        {
            combatSystem.OnPlayerAttack -= HandlePlayerAttack;
        }
    }

    private void HandlePlayerAttack()
    {
        Debug.Log("Beat " + combatSystem.timing + ", Enemy HP -" + combatSystem.currentDamage);
    }

}
