using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemydata")]
public class EnemyData : ScriptableObject
{
    public AudioClip hurtClip;
    public AudioClip deathClip;

    public int score = 10;
    public float health = 100f;
    public float damage = 20f;
    public float speed = 3f;
}
