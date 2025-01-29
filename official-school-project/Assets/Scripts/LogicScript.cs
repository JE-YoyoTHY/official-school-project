using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class LogicScript : MonoBehaviour
{
	//variable
	public UnityEvent freezeEndEvent;

	private float freezeTimer;

	// Start is called before the first frame update
	void Start()
    {
		gridColor();
    }

    // Update is called once per frame
    void Update()
    {
		if (isFreeze())
		{
			freezeMain();
		}

	}

	#region freeze

	private void freezeMain()
	{
		freezeTimer -= Time.deltaTime;

		if (freezeTimer <= 0)
		{
			freezeEndEvent.Invoke();
		}
	}

	public void setFreezeTime(float t)
	{
		freezeTimer = t;
	}

	public bool isFreeze()
	{
		if (freezeTimer > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	#endregion

	#region grid set up

	private void gridColor()
	{
		GameObject[] killFbZones = GameObject.FindGameObjectsWithTag("KillFireballWithoutExplode");
		foreach (GameObject killFbZone in killFbZones)
		{
			killFbZone.GetComponent<Tilemap>().color = Color.clear;
		}
	}

	#endregion

}
