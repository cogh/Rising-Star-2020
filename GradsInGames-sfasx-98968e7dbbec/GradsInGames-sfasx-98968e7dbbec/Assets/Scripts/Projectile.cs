using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Bullet collides with enemy");
        collider.gameObject.GetComponent<Enemy>().dead = true;
        Destroy(gameObject);
    }
}
