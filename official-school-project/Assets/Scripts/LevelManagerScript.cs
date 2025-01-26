using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelManagerScript : MonoBehaviour
{
	//reference
	private GameObject myCameraTarget;
	private Transform camLB; // cam lb for camera left bottom boundary
	private Transform camRT; // cam rt for camera right top 
	private CinemachineVirtualCamera myVirtualCam;
	private PlayerControlScript player;
	
	/*//camera
	private Vector3 playerLastFramePos;
	private const int playerLastFramePosUpdateRate = 4;
	private int playerLastFramePosCounter;*/

	private bool isCurrentLevel;

	[HideInInspector] public UnityEvent levelSetUpEvent; //set up crystal, gate ...

	// Start is called before the first frame update
	void Start()
	{
		myCameraTarget = transform.GetChild(1).GetChild(3).gameObject; // 1 -> camera component, 3 -> camera target
		camLB = transform.GetChild(1).GetChild(0); // 1->camera component, 0 -> left bottom
		camRT = transform.GetChild(1).GetChild(1); // 1->camera component, 1 -> right top
		myVirtualCam = transform.GetChild(1).GetChild(2).GetComponent<CinemachineVirtualCamera>();  // child 1 -> camera component , 2 ->virtual cam
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlScript>();

		myVirtualCam.Follow = myCameraTarget.transform; 

		for(int i = 0; i < transform.GetChild(2).childCount; i++) // child 2 -> level objects
		{
			if(transform.GetChild(2).GetChild(i).tag == "RechargeCrystal") levelSetUpEvent.AddListener(transform.GetChild(2).GetChild(i).GetComponent<RechargeCrystalScript>().regainPower);
			if (transform.GetChild(2).GetChild(i).tag == "Gate") levelSetUpEvent.AddListener(transform.GetChild(2).GetChild(i).GetComponent<GateScript>().gateReset);
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (isCurrentLevel)
		{
			cameraMain();
		}
	}

	public void startLevel()
	{
		levelSetUpEvent.Invoke();
		isCurrentLevel = true;
		//transform.GetChild(3).GetComponent<CinemachineVirtualCamera>().Priority = 11;
		myVirtualCam.Priority = 11;
	}

	public void disableLevel()
	{
		isCurrentLevel = false;
		myVirtualCam.Priority = 10;
	}

	public void restartLevel()
	{
		player.transform.position = transform.GetChild(0).GetChild(0).transform.position; //child 0 -> basic, 0 -> respawn
		levelSetUpEvent.Invoke();
	}

	private void cameraMain()
	{
		myCameraTarget.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
		//i want camera to move if player's pos has changed, or cam will move, while player stands still, for instance : shoot fb upwards while on ground

		/*Vector3 deltaPos = player.transform.position - playerLastFramePos;
		playerLastFramePosCounter++;
		if(playerLastFramePosCounter >= playerLastFramePosUpdateRate)
		{
			playerLastFramePos = player.transform.position;
			playerLastFramePosCounter = 0;
		}
		//Debug.Log(deltaPos);

		if (Mathf.Abs(deltaPos.x) > 0) //0.001 can be changed
			myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, myCameraTarget.GetComponent<Rigidbody2D>().velocity.y);
		else
			//myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(0, myCameraTarget.GetComponent<Rigidbody2D>().velocity.y);
		
		if (Mathf.Abs(deltaPos.y) > 0)
			myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(myCameraTarget.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<Rigidbody2D>().velocity.y);
		else
			//myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(myCameraTarget.GetComponent<Rigidbody2D>().velocity.x, 0);
			*/

		if (myCameraTarget.transform.position.x < camLB.position.x || player.transform.position.x < camLB.position.x)
		{
			myCameraTarget.transform.position = new Vector3(camLB.position.x, myCameraTarget.transform.position.y, myCameraTarget.transform.position.z);
			myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(0, myCameraTarget.GetComponent<Rigidbody2D>().velocity.y);
		}
		if (myCameraTarget.transform.position.y < camLB.position.y || player.transform.position.y < camLB.position.y)
		{
			myCameraTarget.transform.position = new Vector3(myCameraTarget.transform.position.x, camLB.position.y, myCameraTarget.transform.position.z);
			myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(myCameraTarget.GetComponent<Rigidbody2D>().velocity.x, 0);
		}

		if (myCameraTarget.transform.position.x > camRT.position.x || player.transform.position.x > camRT.position.x)
		{
			myCameraTarget.transform.position = new Vector3(camRT.position.x, myCameraTarget.transform.position.y, myCameraTarget.transform.position.z);
			myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(0, myCameraTarget.GetComponent<Rigidbody2D>().velocity.y);
		}
		if (myCameraTarget.transform.position.y > camRT.position.y || player.transform.position.y > camRT.position.y)
		{
			myCameraTarget.transform.position = new Vector3(myCameraTarget.transform.position.x, camRT.position.y, myCameraTarget.transform.position.z);
			myCameraTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(myCameraTarget.GetComponent<Rigidbody2D>().velocity.x, 0);
		}
	}
}
