using UnityEngine;
using System.Collections;

public class LimbHealth : MonoBehaviour
{
    public int health = 3;
    public float normalSpring = 50f;  // Can durumu normalken yay sabiti
    public float damagedSpring = 0f;  // Can 0 olduðunda yay sabiti
    public float respawnTime = 3f;    // Canýn sýfýrlandýktan sonra yeniden dolma süresi

    private ConfigurableJoint joint;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        UpdateJointSpring();
    }

    void UpdateJointSpring()
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = health > 0 ? normalSpring : damagedSpring,
            positionDamper = joint.angularXDrive.positionDamper,
            maximumForce = joint.angularXDrive.maximumForce
        };

        joint.angularXDrive = drive;
        joint.angularYZDrive = drive;
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            UpdateJointSpring();
            if (health <= 0)
            {
                StartCoroutine(RespawnHealth());
            }
        }
    }

    IEnumerator RespawnHealth()
    {
        yield return new WaitForSeconds(respawnTime);
        health = 3;
        UpdateJointSpring();
    }
}
