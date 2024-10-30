using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    public RhythmCheck rhythmCheck; 
    public Text attackTimingText;
    public GameObject attackTimingUI;
    public PlayerCtrl playerCtrl;

    [Header("Player Stats")]
    public float defaultDamage = 3;
    public float currentDamage;

    [HideInInspector]
    public string timing;

    public delegate void TriggerEnterEvent();
    public event TriggerEnterEvent OnPlayerAttack; 

    private bool _attackEnemy;

    private void Start()
    {
        _attackEnemy = false;
        attackTimingUI.SetActive(false); 
    }

    public void PerformAttack()
    {
        timing = RhythmCheck.Instance.CheckAttackTiming();

        switch (timing)
        {
            case "Perfect":
                currentDamage = defaultDamage * 2f;
                break;
            case "Good":
                currentDamage = defaultDamage * 1.5f;
                break;
            case "Normal":
                currentDamage = defaultDamage;
                break;
        }

        // 顯示攻擊結果文本
        attackTimingText.text = "Attack Timing: " + timing;
        StartCoroutine(ShowAttackTimingText());
    }

    // 顯示 Text 並在短暫時間後隱藏
    private IEnumerator ShowAttackTimingText()
    {
        attackTimingUI.SetActive(true); 
        yield return new WaitForSeconds(0.6f); 
        attackTimingUI.SetActive(false); 
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.5f);
        _attackEnemy = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            if (!_attackEnemy && playerCtrl.isAttacking)
            {
                _attackEnemy = true;
                OnPlayerAttack?.Invoke();
                StartCoroutine(ResetAttackState());
                EnemyCtrl enemy = other.GetComponent<EnemyCtrl>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentDamage);
                }
            }
        }
    }
}
