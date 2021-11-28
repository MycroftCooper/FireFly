using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : Entity
{
    public float force = 500;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Agent")
        {
            Debug.Log("Contact Agent");
            Vector2 forceMotion = ForceUtility.calDirectionString(this.transform.rotation.eulerAngles.z, force);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(forceMotion);
        }
    }
}
