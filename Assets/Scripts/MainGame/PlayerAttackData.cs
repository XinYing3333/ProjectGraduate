using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class AttackData
    {
        public AnimatorOverrideController animatorOV;
        public float damage;
    }

    [CreateAssetMenu(fileName = "NewAttack",menuName = "Attack/Normal Attack")]
    public class PlayerAttackData : ScriptableObject
    {
        public AttackData myData;
    }
