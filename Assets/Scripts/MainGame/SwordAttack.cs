using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject myPlayer; 
    private CombatSystem combatSystem;

    private bool _attackEnemy = false; // 避免重複攻擊同一敵人

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
        _attackEnemy = true; // 當玩家攻擊時允許劍進行碰撞檢測
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_attackEnemy && other.CompareTag("Enemy"))
        {
            _attackEnemy = false; // 重置避免重複攻擊同一敵人
            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
            if (enemy != null)
            {
                enemy.TakeDamage(combatSystem.currentDamage);
                Debug.Log("Beat " + combatSystem.timing + ", Enemy HP -" + combatSystem.currentDamage);
            }
        }
    }
}