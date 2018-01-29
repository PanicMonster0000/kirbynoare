using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour{
    [SerializeField] private Vector3 velocity;              // 移動方向
    bool invincibleflag = false;
    double deathtime;
    public Rigidbody rb;
	public Text pre;
    public GameObject attackEffect;
	public Text hp;
    bool allowAttack = true;
    double attackInterval;
    public Color playerColor;
    [SyncVar(hook = "Synchp")]public int stocks = 5;

    // Use this for initialization
    void Start () {
       // playerColor = new Color(netId.Value, netId.Value,  netId.Value, 1.0f);
       // ChangeColorOfGameObject(this.gameObject, playerColor);
        rb = GetComponent<Rigidbody>();
		hp = Instantiate (pre,new Vector3(0,0,-93.3820251f),pre.transform.rotation,GameObject.Find ("Canvas").transform);
		hp.transform.localScale = (new Vector3(0.15f,0.15f,0.2f));
		hp.transform.localRotation = new Quaternion (0, 0, 0, 1);
		hp.rectTransform.anchorMax = new Vector2 (0.15f * netId.Value, 1f);
		hp.rectTransform.anchorMin = new Vector2 (0.15f * netId.Value, 1f);
		hp.text = maketext ();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

        // WASD入力から、XZ平面(水平な地面)を移動する方向(velocity)を得ます
        velocity = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) 
                velocity.z += 0.5f;
            else if (Input.GetKey(KeyCode.A))
                velocity.x -= 0.5f;
            else if (Input.GetKey(KeyCode.S))
                velocity.z -= 0.5f;
            else if (Input.GetKey(KeyCode.D))
                velocity.x += 0.5f;



        //攻撃処理

        if (Input.GetKey(KeyCode.Space) && allowAttack){
			Cmdfire ();
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
            } else {
                Vector3 pos = transform.position;
                pos.x = Random.Range(5,75);
                pos.y = 7;
                pos.z = Random.Range(5,75);
                transform.position = pos;
                deathtime = Time.time;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                invincibleflag = true;

                if (!isServer)
                    Cmdhp(stocks);
                else
                    RpcChangeStock(stocks);
                

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

	void OnDestroy() {
		Destroy (hp);
	}

	[Command]
    [Server]
	void Cmdfire(){
		GameObject attackObject = Instantiate(attackEffect) as GameObject; 

		NetworkServer.Spawn (attackObject);

		float x = Mathf.Round(transform.position.x/10)*10;
		float z = Mathf.Round(transform.position.z/10)*10;


		Vector3 forword = transform.forward * -10;
		attackObject.transform.position = new Vector3(x, transform.position.y-0.5f, z) + forword;
        Renderer atren = attackObject.GetComponent<Renderer>();
        atren.material.color = playerColor;

        Quaternion rotation = this.transform.localRotation;
		Vector3 rotationAngles = rotation.eulerAngles;
		rotationAngles.x = rotationAngles.x + 90.0f;
		rotationAngles.z = rotationAngles.z + 90.0f;
		rotationAngles.y = rotationAngles.y + -90.0f;
		rotation = Quaternion.Euler(rotationAngles);
		attackObject.transform.localRotation = rotation;
	}

	void Synchp(int stocks) {
		hp.text = maketext();
	}

	[Command]
	void Cmdhp(int sersto){
		stocks = sersto;
		hp.text = maketext();
	}

    [ClientRpc]
    void RpcChangeStock(int stock){
        stocks = stock;
        hp.text = maketext();
    }

	string maketext(){
		string text = "";
		text = "player" + netId.ToString() + "\n";
		for (int i = 0; i < stocks; i++) {
			text += "■";
		}
		return text;
	}

    void ChangeColorOfGameObject(GameObject targetObject, Color color)
    {

        //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialの色を変える
        foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
        {
            foreach (Material material in targetRenderer.materials)
            {
                material.color = color;
            }
        }

        //入力されたオブジェクトの子にも同様の処理を行う
        for (int i = 0; i < targetObject.transform.childCount; i++)
        {
            ChangeColorOfGameObject(targetObject.transform.GetChild(i).gameObject, color);
        }

    }
}