using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float playerSpeed = 0.1f;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = (Animator)GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow))
			animator.SetInteger("State",1);
		if(Input.GetKeyDown(KeyCode.DownArrow))
			animator.SetInteger("State",2);
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			animator.SetInteger("State",3);
		if(Input.GetKeyDown(KeyCode.RightArrow))
			animator.SetInteger("State",4);

		if(!Input.anyKey)
			animator.SetInteger("State",0);

		Move ();
	}

	void Move()
	{
		var deltaX = 0.0f;
		var deltaY = 0.0f;

		var animationState = animator.GetCurrentAnimatorStateInfo(0).nameHash ;

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
