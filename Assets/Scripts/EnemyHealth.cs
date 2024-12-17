using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    private float m_currentEnemyHealth = 0.0f;
    [SerializeField] GameObject m_endMenu;
    [SerializeField] private EndManager m_textMessage;

    void Start()
    {
        m_currentEnemyHealth = 100.0f;
        m_slider.value = m_currentEnemyHealth;
    }
    
    public override void ReduceHealth(float amount)
    {
        m_currentEnemyHealth -= amount;
        m_slider.value = m_currentEnemyHealth;

        if (m_currentEnemyHealth <= 0.0f) { 
            m_endMenu.SetActive(true);
            m_textMessage.text("You Win!");
        }
    }
}
