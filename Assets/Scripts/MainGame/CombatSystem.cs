using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    public Text attackTimingText;
    public GameObject attackTimingUI;

    [Header("Player Stats")]
    public float defaultDamage = 3f; // 基礎傷害
    public float currentDamage;     // 當前傷害
    public float lightAttackResetTime = 0.3f;
    public float heavyAttackResetTime = 0.6f;

    [HideInInspector] 
    public string timing;   // 節奏點精度
    public bool IsAttacking { get; private set; } // 攻擊冷卻狀態
    public float attackResetTime;     // 攻擊冷卻時間

    public delegate void TriggerEnterEvent();
    public event TriggerEnterEvent OnPlayerAttack; // 攻擊事件

    [Header("Combo Stats")]
    public List<PlayerAttackData> combo; // Combo 動畫列表
    public float comboResetTime = 1.0f;  // Combo 超時時間

    private int _comboCounter;      // 當前 Combo 次數

    private PlayerCtrl _playerCtrl;

    private void Start()
    {
        attackTimingUI.SetActive(false);
        _playerCtrl = GetComponent<PlayerCtrl>();
        EndCombo();
    }

    public void PerformAttack(AttackType attackType)
    {
        // 如果正在攻擊或者 Combo 已完成，直接返回
        if (IsAttacking || _comboCounter >= combo.Count)
            return;

        IsAttacking = true; // 標記攻擊狀態
        timing = RhythmCheck.Instance.CheckAttackTiming(); // 檢查節奏點精度

        // 計算基礎傷害
        switch (attackType)
        {
            case AttackType.Light:
                attackResetTime = lightAttackResetTime;
                currentDamage = defaultDamage;
                break;
            case AttackType.Heavy:
                attackResetTime = heavyAttackResetTime;
                currentDamage = defaultDamage * 2f;
                break;
        }

        // 根據節奏點倍率調整傷害
        switch (timing)
        {
            case "Perfect":
                currentDamage *= 2f;
                break;
            case "Good":
                currentDamage *= 1.5f;
                break;
        }

        // 顯示攻擊結果
        attackTimingText.text = $"{timing} - {attackType}";
        StartCoroutine(ShowAttackTimingText());

        // 執行 Combo 邏輯
        ComboCount();
        
        // 觸發攻擊事件
        OnPlayerAttack?.Invoke();
    }

    private void ComboCount()
    {
        // 如果超過 Combo 限制，結束 Combo
        if (_comboCounter >= combo.Count)
        {
            EndCombo();
            return;
        }

        // 播放當前 Combo 的動畫
        _playerCtrl.anim.runtimeAnimatorController = combo[_comboCounter].myData.animatorOV;
        _playerCtrl.anim.SetTrigger("attacking");

        // 判斷是否是最後一個 Combo
        if (_comboCounter == combo.Count - 1) // 最後一個 Combo
        {
            Debug.Log("Finished Last Combo");
            StartCoroutine(AttackCooldown(attackResetTime + 0.5f)); // 冷卻時間增加，例如 +0.5 秒
        }
        else
        {
            StartCoroutine(AttackCooldown(attackResetTime)); // 常規冷卻時間
        }

        // 增加 Combo 計數
        _comboCounter++;

        // 重置 Combo 時限
        StopCoroutine("ComboTimer");
        StartCoroutine("ComboTimer");
    }


    private IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(comboResetTime); // 等待 Combo 時限
        EndCombo(); // 超時後結束 Combo
    }

    private void EndCombo()
    {
        _comboCounter = 0;   // 重置 Combo 計數
        Debug.Log("Combo ended or timeout.");
    }

   
    private IEnumerator ShowAttackTimingText()
    {
        attackTimingUI.SetActive(true);
        yield return new WaitForSeconds(attackResetTime);
        attackTimingUI.SetActive(false);
    }

    private IEnumerator AttackCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        IsAttacking = false; // 重置攻擊狀態
    }
}

public enum AttackType
{
    Light,
    Heavy
}
