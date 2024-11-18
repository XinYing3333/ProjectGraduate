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
    [HideInInspector] public bool IsAttacking { get; private set; }
    public float attackResetTime = 0.65f;

    public delegate void TriggerEnterEvent();
    public event TriggerEnterEvent OnPlayerAttack; // 攻擊事件

    private void Start()
    {
        attackTimingUI.SetActive(false); 
    }

    public void PerformAttack()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;
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

            attackTimingText.text = timing;
            StartCoroutine(ShowAttackTimingText());
            StartCoroutine(ResetAttackState());

            // 觸發攻擊事件，通知劍擊進行碰撞檢測
            OnPlayerAttack?.Invoke();
        }
    }

    private IEnumerator ShowAttackTimingText()
    {
        attackTimingUI.SetActive(true); 
        yield return new WaitForSeconds(0.55f); 
        attackTimingUI.SetActive(false); 
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackResetTime);
        IsAttacking = false;
    }
}