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
