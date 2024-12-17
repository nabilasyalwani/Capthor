using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Weapons : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private bool m_isMoving = false;
    private AudioSource m_audioSource;

    private float m_initialSpeed = 0.0f;
    private float m_initialRotation = 0.0f;
    private string m_weaponTargetTag;
    private Vector2 m_velocity = Vector2.zero;
    public float damageAttack = 0.0f;

    public Action weaponLanded;

    private void Awake() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        if (m_rigidBody == null) {
            m_rigidBody = gameObject.AddComponent<Rigidbody2D>();
        }
        m_rigidBody.isKinematic = true;
        m_rigidBody.useFullKinematicContacts = true;
    }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_isMoving = false;
    }

    public void Throw(float initialSpeed, float directionAngle, string targetTag, float attack)
    {
        damageAttack = attack;
        m_weaponTargetTag = targetTag;
        m_initialSpeed = initialSpeed;
        m_initialRotation = gameObject.transform.localEulerAngles.z - directionAngle;
        
        m_isMoving = true;
        m_velocity = new Vector2(
            Mathf.Cos(directionAngle * Mathf.Deg2Rad), 
            Mathf.Sin(directionAngle * Mathf.Deg2Rad)) * m_initialSpeed;
    }

    private void FixedUpdate()
    {

        if (!m_isMoving) return; 
        
        m_rigidBody.velocity = m_velocity;
        m_velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        Vector3 newRotatin = transform.localEulerAngles;
        newRotatin.z = m_initialRotation + Mathf.Atan2(m_velocity.y, m_velocity.x) * Mathf.Rad2Deg;

        transform.localEulerAngles = newRotatin;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!m_isMoving) return;

        if (collision.gameObject.CompareTag("World") || collision.gameObject.CompareTag(m_weaponTargetTag)) {
            m_isMoving = false;
            m_rigidBody.velocity = Vector3.zero;
            weaponLanded?.Invoke();
        }
    }
}
