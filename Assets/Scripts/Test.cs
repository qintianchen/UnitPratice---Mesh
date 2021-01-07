using System;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
	private int direction = 1;
	private void Update()
	{
		transform.localPosition += new Vector3(0.8f * direction, 0);
		if (transform.localPosition.x > 150)
			direction = -1;
		else if (transform.localPosition.x < -150)
			direction = 1;
	}
}