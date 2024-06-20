using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement movement;
    public Transform cam;
    private Rigidbody rb;

    [Header("Variables")]
    public float fullCooldown = 1.5f;
    public float activeCooldown = 0f;
    public float dashForce = 800f;
    public float dashLimit = 80f;
    public float standardLimit = 25f;

    private
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeCooldown > 0)
        {
            activeCooldown -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                DashAbility();
            }
        }
    }

    private void DashAbility()
    {
        movement.moveSpeedLimit = dashLimit;

        Vector3 dashVector = cam.forward * dashForce;
        rb.velocity = Vector3.zero;
        rb.AddForce(dashVector, ForceMode.Impulse);
        activeCooldown = fullCooldown;
        Invoke(nameof(resetLimit), fullCooldown);
    }

    private void resetLimit()
    {
        movement.moveSpeedLimit = standardLimit;
    }
}
