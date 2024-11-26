using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject myPlayer; 
    private CombatSystem _combatSystem;

    private bool _attackEnemy = false; // 避免重複攻擊同一敵人
    private CapsuleCollider _triggerCollider;

    private void Start()
    {
        _triggerCollider = GetComponent<CapsuleCollider>();

        _combatSystem = myPlayer.GetComponent<CombatSystem>();
        if (_combatSystem != null)
        {
            _combatSystem.OnPlayerAttack -= HandlePlayerAttack; // 確保無重複訂閱
            _combatSystem.OnPlayerAttack += HandlePlayerAttack;
        }
    }

    private void OnDisable()
    {
        if (_combatSystem != null)
        {
            _combatSystem.OnPlayerAttack -= HandlePlayerAttack;
        }
    }

    private void HandlePlayerAttack()
    {
        _attackEnemy = true; // 當玩家攻擊時允許劍進行碰撞檢測
    }

    public void EnableTriggerCollider()
    {
        _triggerCollider.enabled = true;
    }
    
    public void DisableTriggerCollider()
    {
        _triggerCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_attackEnemy && other.CompareTag("Enemy"))
        {
            _attackEnemy = false; // 重置避免重複攻擊同一敵人
            EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
            if (enemy != null)
            {
                enemy.TakeDamage(_combatSystem.currentDamage);
                Debug.Log("Beat " + _combatSystem.timing + ", Enemy HP -" + _combatSystem.currentDamage);
            }
        }
    }
}