using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int>
{

    [System.Serializable]
    public class EnemyStats
    {
        public int health = 100;
    }

    public EnemyStats enemyStats = new EnemyStats();

    public void Damage (int damage)
    {
        enemyStats.health -= damage;
        if (enemyStats.health <= 0)
        {
            Kill();
        }
    }

    public void Kill ()
    {
        Destroy(gameObject);
    }

}
