using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    private float m_currentPlayerHealth = 0.0f;
    [SerializeField] GameObject m_endMenu;
    [SerializeField] private EndManager m_textMessage;


    void Start()
    {
        m_currentPlayerHealth = 100.0f;
        m_slider.value = m_currentPlayerHealth;
    }

    public override void ReduceHealth(float amount)
    {
        m_currentPlayerHealth -= amount;
        m_slider.value = m_currentPlayerHealth;

        if (m_currentPlayerHealth <= 0.0f)
        {
            m_endMenu.SetActive(true);
            m_textMessage.text("Game Over!");
        }
    }
}
