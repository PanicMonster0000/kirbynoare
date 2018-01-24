using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Rigidbody floorrb;
    public Vector3 pos;
    // Use this for initialization
    void Start()
    {
        floorrb = GetComponent<Rigidbody>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -60)
        {
            transform.position = pos;
            floorrb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    void OnCollisionEnter(Collision wave)
    {
        if (wave.gameObject.name == "attackEffect(Clone)")
        {
            floorrb.constraints = RigidbodyConstraints.None;
            floorrb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
}