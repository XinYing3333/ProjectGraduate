using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public string enemyName;
    public float health;
    public float damage;

}

[CreateAssetMenu(fileName = "NewEnemy",menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public Data myData;
}
