using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class Health : MonoBehaviour
{

    protected Slider m_slider;
    
    private void Awake()
    {
        m_slider = GetComponent<Slider>();
    }

    public abstract void ReduceHealth(float amount);
}
