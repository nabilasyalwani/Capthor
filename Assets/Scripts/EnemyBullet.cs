using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private AudioSource m_audioSource;
    private float m_speed = 10.0f;
    private Rigidbody2D m_rigidBody;
    public float damageAttack = 20.0f;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.time = 0.08f;
        m_audioSource.Play();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.useFullKinematicContacts = true;
        m_rigidBody.velocity = transform.right * m_speed;
    }



}
