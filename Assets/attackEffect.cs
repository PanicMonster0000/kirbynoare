using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackEffect : MonoBehaviour {


    // Update is called once per frame
    void Update () {
        

        transform.Translate(transform.forward*-1);
        if (transform.position.z > 75 || transform.position.x > 75 || transform.position.z < -5 || transform.position.x < -5)
            Destroy(gameObject);
	}
}
