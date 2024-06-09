using UnityEngine;

public class RagdollPlayerController : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public float jumpForce;

    public Rigidbody hips;
    public bool isGrounded;
    [SerializeField] private LimbHealth[] limbHealth; // LimbHealth bileþeni için referans


    void Start()
    {
        hips = GetComponent<Rigidbody>();
        limbHealth = GetComponentsInChildren<LimbHealth>(); // LimbHealth bileþenini al
    }

    private void FixedUpdate()
    {

        // Karakterin caný 0 deðilse hareket kontrollerini gerçekleþtir
        if (checkLimbs())
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isWalking", true);
                hips.AddForce(hips.transform.forward * speed);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                hips.AddForce(-hips.transform.right * speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("isWalking", true);
                hips.AddForce(-hips.transform.forward * speed);
            }
            else if (!Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isWalking", false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                hips.AddForce(hips.transform.right * speed);
            }

            if (Input.GetAxis("Jump") > 0)
            {
                if (isGrounded)
                {
                    hips.AddForce(new Vector3(0, jumpForce, 0));
                    isGrounded = false;
                }
            }
        }
        else
        {
            // Karakterin caný 0 ise hareket etme
            animator.SetBool("isWalking", false);
        }
    }

    bool checkLimbs()
    {
        foreach (LimbHealth lh in limbHealth)
        {
            if (lh.health <= 0)
                return false;
        }
        return true;
    }
}
