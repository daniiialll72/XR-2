using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysics : MonoBehaviour
{
    private Collider[] colliders;

    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    public void EnableCollidersAfterSeconds(float delay)
    {
        Invoke("EnableColliders", delay);
    }

    public void DisableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void EnableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
