using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField] private GameObject m_handRotationPivot;
    [SerializeField] private GameObject m_hand;
    [SerializeField] private float m_startAngle = 15.0f;
    [SerializeField] private float m_endAngle = 60.0f;
    [SerializeField] private float m_aimingSpeed = 0.3f;
    [SerializeField] private LayerMask m_layer;
    [SerializeField] private PlayerHealth m_playerHealth;

    [Header("Projectile Properties")]
    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private int m_numOfPoints = 10;
    [SerializeField] private float m_initialVelocity = 20.0f;

    [SerializeField] GameObject m_spearWeapon;
    [SerializeField] GameObject m_endMenu;

    private AudioSource m_throwSound;
    private float m_currentTargetAngle = 0.0f;
    private float m_damageAttack = 10.0f;
    private bool m_isAimingUp = true;
    private bool m_isAiming = false;
    private Vector2 m_landPosition = Vector2.zero;

    private void Start() {
        m_throwSound = GetComponent<AudioSource>();

        m_lineRenderer.enabled = false;
        m_lineRenderer.positionCount = m_numOfPoints + 1;

        Color c1 = new Color(0.5686275f, 0.4745098f, 1.0f);
        Color c2 = Color.white;

        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha * 0.1f, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );

        m_lineRenderer.colorGradient = gradient;

        ReloadWeapon();
    }

    private void StartAiming() {
        m_lineRenderer.enabled = true;
        m_currentTargetAngle = m_endAngle;
        m_isAimingUp = true;
        m_isAiming = true;
    }

    void Update() {

        if (m_endMenu.activeInHierarchy)
            return;

        if (!m_isAiming) return;

        float current = m_handRotationPivot.transform.localEulerAngles.z;
        float target = m_isAimingUp ? m_endAngle : m_startAngle;
        float newAngle = Mathf.MoveTowardsAngle(current, target, m_aimingSpeed * Time.deltaTime);

        m_handRotationPivot.transform.localEulerAngles = new Vector3(0.0f, 0.0f, newAngle);

        if( Mathf.Approximately(newAngle, target) ) {
            m_isAimingUp = !m_isAimingUp;
            m_handRotationPivot.transform.localEulerAngles = new Vector3(0.0f, 0.0f, target);
        }

        DrawProjectilePath();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }

    private void Shoot() {
        m_throwSound.Play();

        Debug.Log("HEREEEE");

        m_isAiming = false;
        m_lineRenderer.enabled = false;
        
        GameObject weaponGameObject = m_hand.transform.GetChild(0).gameObject;
        weaponGameObject.transform.parent = null;

        Weapons currentWeapon = weaponGameObject.GetComponent<Weapons>();
        currentWeapon.Throw(m_initialVelocity, m_handRotationPivot.transform.localEulerAngles.z, "Enemy",m_damageAttack);

    }

    private void DrawProjectilePath() {

        float cosTheta = Mathf.Cos(m_handRotationPivot.transform.localEulerAngles.z * Mathf.Deg2Rad);
        float sinTheta = Mathf.Sin(m_handRotationPivot.transform.localEulerAngles.z * Mathf.Deg2Rad);
        Vector2 initialPosition = m_hand.transform.position;
        float gravity = -Physics2D.gravity.y;

        float a = -gravity / 2.0f;
        float b = m_initialVelocity * sinTheta;
        float c = initialPosition.y - (-2.0f);
        float t = (-b - Mathf.Sqrt((b * b) - 4 * a * c)) / (2.0f * a);

        bool targetFound = false;
        Vector2 pos = Vector2.zero;

        for (int i = 0; i <= m_numOfPoints; i++) {
            
            float currentDelta = (i / (float)m_numOfPoints) * t;
            float x = initialPosition.x + m_initialVelocity * currentDelta * cosTheta;
            float y = initialPosition.y + m_initialVelocity * currentDelta * sinTheta - (1.0f / 2.0f) * gravity * currentDelta * currentDelta;
            pos = new Vector2(x, y);

            if (!targetFound) {
                if (CheckForCollision(pos)) {
                    m_landPosition = pos;
                    targetFound = true;
                }
            }
            
            m_lineRenderer.SetPosition(i, new Vector3(x, y, 0.0f));

        }

        if (!targetFound)
            m_landPosition = pos;

    }

    private bool CheckForCollision( Vector2 pos ) {

        RaycastHit2D hit = Physics2D.CircleCast(
            pos, 
            0.1f, 
            Vector2.zero, 
            0.0f,
            m_layer
        );

        return hit;

    }

    private void ReloadWeapon()
    {
        m_handRotationPivot.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, m_endAngle)));

        GameObject spear = Instantiate(m_spearWeapon, m_hand.transform);
        Weapons weaponComponent = spear.GetComponent<Weapons>();
        weaponComponent.weaponLanded += ReloadWeapon;
        StartAiming();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        Debug.Log(obj.name);

         if (obj.CompareTag("Bullet"))
        {
            m_playerHealth.ReduceHealth(obj.GetComponent<EnemyBullet>().damageAttack);
            Destroy(obj);
        }
    }

}
