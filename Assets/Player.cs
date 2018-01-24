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
    public GameObject attackEffect;
    bool allowAttack = true;
    float attackInterval;

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
            else if (Input.GetKey(KeyCode.A))
                velocity.x -= 1;
            else if (Input.GetKey(KeyCode.S))
                velocity.z -= 1;
            else if (Input.GetKey(KeyCode.D))
                velocity.x += 1;



        //攻撃処理

        if (Input.GetKey(KeyCode.Space) && allowAttack){
            GameObject attackObject = Instantiate(attackEffect) as GameObject; 
           
            float x = Mathf.Round(transform.position.x/10)*10;
            float z = Mathf.Round(transform.position.z/10)*10;


            Vector3 forword = transform.forward * -10;
            attackObject.transform.position = new Vector3(x, transform.position.y-0.5f, z) + forword;

            Quaternion rotation = this.transform.localRotation;
            Vector3 rotationAngles = rotation.eulerAngles;
            rotationAngles.x = rotationAngles.x + 90.0f;
            rotationAngles.z = rotationAngles.z + 90.0f;
            rotationAngles.y = rotationAngles.y + -90.0f;

            rotation = Quaternion.Euler(rotationAngles);
            attackObject.transform.localRotation = rotation;

            attackInterval = Time.time;
            allowAttack = false;
        }

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
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }

        //攻撃のインターバル
        if (!(allowAttack)){
            if( (attackInterval+1.5) < Time.time){
                allowAttack = true;
            }
        }

    }
}
