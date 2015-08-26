using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float playerSpeed = 0.1f;

	private Animator animator;
	public Camera cam;				// ссылка на нашу камеру
	private int animationState;					// поворот 
	private NetworkView networkView;
	public GameObject BuletPrefab;	// Персонаж игрока

	void Awake () {
		cam = GetComponentInChildren<Camera>();
		animator = (Animator)GetComponent<Animator> ();
		networkView = GetComponent<NetworkView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine) {
			if (Input.GetKeyDown (KeyCode.UpArrow))
				animator.SetInteger ("State", 1);
			if (Input.GetKeyDown (KeyCode.DownArrow))
				animator.SetInteger ("State", 2);
			if (Input.GetKeyDown (KeyCode.LeftArrow))
				animator.SetInteger ("State", 3);
			if (Input.GetKeyDown (KeyCode.RightArrow))
				animator.SetInteger ("State", 4);

			if(Input.GetKeyDown(KeyCode.Space))
			{
				var bulet = (GameObject)Network.Instantiate(BuletPrefab, 
				                                       transform.position, transform.rotation, 1);

				bulet.GetComponent<Bulet>().InitDirection(animator.GetInteger("State"));
			}

			if (!Input.anyKey)
				animator.SetInteger ("State", 0);
		} else {
			if(cam.enabled) { 
				cam.enabled = false; 
				cam.gameObject.GetComponent<AudioListener>().enabled = false;
			}
		}

		Move ();
	}

	// Вызывается с определенной частотой. Отвечает за сереализицию переменных
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting) {
			animationState = animator.GetInteger("State");
			syncPosition = transform.position;
			
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref animationState);
		} else {
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref animationState);

			animator.SetInteger ("State", animationState);
			transform.position = syncPosition;

			print(animationState);
		}
	}

	void Move()
	{
		var deltaX = 0.0f;
		var deltaY = 0.0f;

		var animationState = animator.GetCurrentAnimatorStateInfo(0).nameHash;

		if (animationState.ToString() == "-1915430808") {
			deltaY = playerSpeed;
		}

		if (animationState.ToString() == "1365136315") {
			deltaY = -playerSpeed;
		}

		if (animationState.ToString() == "935732456") {
			deltaX = -playerSpeed;
		}

		if (animationState.ToString() == "1497323511") {
			deltaX = playerSpeed;
		}

		transform.position = new Vector2(transform.position.x + deltaX,
		                                 transform.position.y + deltaY);  
	}
}
