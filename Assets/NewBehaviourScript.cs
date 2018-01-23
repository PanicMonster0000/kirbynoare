using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
	 public Transform target;
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 0, 0.5f);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(-0.5f, 0, 0);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(0, 0, -0.5f);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(0.5f, 0, 0);

         Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
	}
}
