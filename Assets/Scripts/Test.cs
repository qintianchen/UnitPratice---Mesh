using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Test : MonoBehaviour
{
	public delegate void MyAction();
	public event MyAction action;
	private void Start()
	{
		gameObject.AddComponent<Test2>();
	}

	void Print1()
	{
		Debug.Log("H 1");
	}

	void Print2()
	{
		Debug.Log("H 2");
	}

	public void InvokeAction()
	{
		action();
	}
}

public class Test2 : MonoBehaviour
{
	private void Start()
	{
		var test = gameObject.GetComponent<Test>();

		test.InvokeAction();
	}

	void Print1()
	{
		Debug.Log("Q 1");
	}

	void Print2()
	{
		Debug.Log("Q 2");
	}
}