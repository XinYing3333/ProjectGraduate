using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    public EnemyData enemyData;
    public Text nameText;
    public Slider healthBar;
    
    public Animator anim;

    private bool _enemyHurt;
    private PlayerStats _playerStats;
    
    void Start()
    {
        nameText.text = enemyData.myData.enemyName;
        healthBar.maxValue = enemyData.myData.health;
        healthBar.value = healthBar.maxValue;
    }
    
    public void TakeDamage(float playerDamage)
    {
        if(_enemyHurt)return;
        anim.SetTrigger("isHurt");
        healthBar.value -= playerDamage;
    }

    public void OutputDamage()
    {
        _playerStats.TakeDamage(enemyData.myData.damage);
    }
    
    private IEnumerator ResetHurtState()
    {
        _enemyHurt = true;
        yield return new WaitForSeconds(0.5f);
        _enemyHurt = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if(!_enemyHurt)return;
            StartCoroutine(ResetHurtState());
        }
    }
}
