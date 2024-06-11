using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

public class TossDamage : NetworkBehaviour 
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
        yield return new WaitForSeconds(1f); // Toss animasyon süresi kadar bekle
        isTossing = false;
    }
}
