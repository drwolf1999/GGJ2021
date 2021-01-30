using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	//
	RaycastHit2D[] raycastHit2D;
	Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Debug.DrawRay(transform.position, Direction(), Color.red);
	}

	private Vector2 Direction()
	{
		Debug.Log(transform.rotation.eulerAngles.z);
		switch (transform.rotation.eulerAngles.z)
		{
			case 0:
				return Vector2.up;
			case 270:
				return Vector2.right;
			case 180:
				return Vector2.down;
			case 90:
				return Vector2.left;
		}
		return Vector2.up;
	}

	public GameObject GetAdjacentDoor()
	{
		raycastHit2D = Physics2D.RaycastAll(transform.position, Direction(), 1f);
		GameObject ret = null;
		foreach (RaycastHit2D hit in raycastHit2D)
		{
			if (hit.collider.gameObject.name.Contains("Door"))
			{
				ret = hit.collider.gameObject;
			}
		}
		return ret;
	}

	public void Open()
	{
		animator.SetBool("doorClosed", false);
	}

	public void Close()
	{
		animator.SetBool("doorClosed", true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		EdgeCollider2D edgeCollider2D = GetComponent<EdgeCollider2D>();
		if (edgeCollider2D != null)
		{
			Debug.Log("EDGE: " + collision.tag);
			if (collision.CompareTag("Player"))
			{
				StageController stageController = GameObject.Find("MapLoader").GetComponent<StageController>();
				stageController.BeforeNextStage();
			}
		}
	}
}