using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField] private Vector3 velocity;              // 移動方向
    int stocks = 5;
    bool invincibleflag = false;
    float deathtime;
    public Rigidbody rb;
    public Text hp;
    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        // WASD入力から、XZ平面(水平な地面)を移動する方向(velocity)を得ます
        velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            velocity.z += 1;
        if (Input.GetKey(KeyCode.A))
            velocity.x -= 1;
        if (Input.GetKey(KeyCode.S))
            velocity.z -= 1;
        if (Input.GetKey(KeyCode.D))
            velocity.x += 1;


        // いずれかの方向に移動している場合
        if (velocity.magnitude > 0)
        {
            // プレイヤーの回転(transform.rotation)の更新
            // 無回転状態のプレイヤーのZ+方向(後頭部)を、移動の反対方向(-velocity)に回す回転とします
            transform.rotation = Quaternion.LookRotation(-velocity);

            // プレイヤーの位置(transform.position)の更新
            // 移動方向ベクトル(velocity)を足し込みます
            transform.position += velocity;
        }
        if (transform.position.y < -60) {
            stocks--;
            print(stocks);
            if(stocks == 0) {
                Destroy(gameObject);
                hp.text = "";
            } else {
                Vector3 pos = transform.position;
                pos.x = Random.Range(5,75);
                pos.y = 10;
                pos.z = Random.Range(5,75);
                transform.position = pos;
                deathtime = Time.time;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                invincibleflag = true;
                hp.text = "";
                for(int i=0;i<stocks;i++) {
                    hp.text += "■";
                }
            }
        }
        if(invincibleflag){
            if((deathtime + 3) < Time.time){
                rb.useGravity = true;
                invincibleflag = false;
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
