using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossDamage : MonoBehaviour
{
    private bool isTossing = false;

    void OnCollisionEnter(Collision col)
    {
        if (isTossing && col.transform.CompareTag("Object"))
        {
            LimbHealth limbHealth = col.transform.GetComponent<LimbHealth>();
            if (limbHealth != null)
            {
                limbHealth.TakeDamage(1);
            }
        }
    }

    public void StartToss()
    {
        isTossing = true;
        StartCoroutine(StopTossAfterAnimation());
    }

    private IEnumerator StopTossAfterAnimation()
    {
        yield return new WaitForSeconds(1f); // Toss animasyon s�resi kadar bekle
        isTossing = false;
    }
}