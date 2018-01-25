using System.Collections;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Rigidbody floorrb;
    public Vector3 pos;
    private bool fall = false;
    public Renderer rend;
    // Use this for initialization
    void Start()
    {
        floorrb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -120)
        {
            rend.material.SetColor("_Color", Color.white);
            transform.position = pos;
            floorrb.constraints = RigidbodyConstraints.FreezeAll;
            fall = false;
        }
        if(fall)
        {
            transform.Translate(0, -1, 0);
        }
    }
    IEnumerator OnCollisionEnter(Collision wave)
    {

        if (wave.gameObject.name == "attackEffect(Clone)")
        {
            Renderer colRend = wave.gameObject.GetComponent<Renderer>();
            rend.material.SetColor("_Color", colRend.material.GetColor("_Color"));
            yield return new WaitForSecondsRealtime(1f);

            fall = true;
        }
    }
}