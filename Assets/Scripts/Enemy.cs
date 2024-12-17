using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth m_enemyHealth;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private Transform m_muzzle;
    [SerializeField] private float m_shootInterval = 6.0f;

    [SerializeField] GameObject m_endMenu;

    private float m_currentTimer = 0.0f;

    void Start()
    {
        m_currentTimer = m_shootInterval;
    }

    private void Update()
    {
        if (m_endMenu.activeInHierarchy)
            return;

        m_currentTimer -= Time.deltaTime;
        if( m_currentTimer <= 0.0f )
        {
            Shoot();
            m_currentTimer = m_shootInterval;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(m_bullet);
        bullet.transform.position = m_muzzle.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Spear"))
        {
            m_enemyHealth.ReduceHealth(obj.GetComponent<Weapons>().damageAttack);
        }
    }


}
