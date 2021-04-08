using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleController : MonoBehaviour 
{
	public  bool isOn;

	public Color onColorBorder;
	public Color offColorBorder;

	public Image toggleBorderImage;
	public RectTransform toggle;

	public GameObject handle;
	private RectTransform handleTransform;

	private float handleSize;
	private float onPosX;
	private float offPosX;

	public float handleOffset;

	public float speed;
	static float t = 0.0f;

	private bool switching = false;
	private Image handleImage;
	private Action<bool> toggleAction = (status) => { Debug.Log(status); };


	void Awake()
	{
		handleTransform = handle.GetComponent<RectTransform>();
		RectTransform handleRect = handle.GetComponent<RectTransform>();
		handleImage = handle.GetComponent<Image>();
		handleSize = handleRect.sizeDelta.x;
		float toggleSizeX = toggle.sizeDelta.x;
		onPosX = (toggleSizeX / 2) - (handleSize/2) - handleOffset;
		offPosX = onPosX * -1;

	}

	public void SetAction(Action<bool> action) {
		toggleAction = action;
	}


	void Start()
	{
		if(isOn)
		{
			toggleBorderImage.color = onColorBorder;
			handleImage.color = onColorBorder;
			handleTransform.localPosition = new Vector3(onPosX, 0f, 0f);
		}
		else
		{
			toggleBorderImage.color = offColorBorder;
			handleImage.color = offColorBorder;
			handleTransform.localPosition = new Vector3(offPosX, 0f, 0f);
		}
	}

	void Update()
	{

		if(switching)
		{
			Toggle(isOn);
		}
	}

	public void Switching()
	{
		switching = true;
	}

	public void Toggle(bool toggleStatus)
	{	
		if(toggleStatus)
		{
			toggleBorderImage.color = SmoothColor(onColorBorder, offColorBorder);
			handleImage.color = SmoothColor(onColorBorder, offColorBorder);;
			handleTransform.localPosition = SmoothMove(handle, onPosX, offPosX);
		}
		else 
		{
			toggleBorderImage.color = SmoothColor(offColorBorder, onColorBorder);
			handleImage.color = SmoothColor(offColorBorder, onColorBorder);
			handleTransform.localPosition = SmoothMove(handle, offPosX, onPosX);
		}
			
	}


	Vector3 SmoothMove(GameObject toggleHandle, float startPosX, float endPosX)
	{
		
		Vector3 position = new Vector3 (Mathf.Lerp(startPosX, endPosX, t += speed * Time.deltaTime), 0f, 0f);
		StopSwitching();
		return position;
	}

	Color SmoothColor(Color startCol, Color endCol)
	{
		Color resultCol;
		resultCol = Color.Lerp(startCol, endCol, t += speed * Time.deltaTime);
		return resultCol;
	}

	CanvasGroup Transparency (GameObject alphaObj, float startAlpha, float endAlpha)
	{
		CanvasGroup alphaVal;
		alphaVal = alphaObj.gameObject.GetComponent<CanvasGroup>();
		alphaVal.alpha = Mathf.Lerp(startAlpha, endAlpha, t += speed * Time.deltaTime);
		return alphaVal;
	}

	void StopSwitching()
	{
		if(t > 1.0f)
		{
			switching = false;

			t = 0.0f;
			switch(isOn)
			{
			case true:
				isOn = false;
				break;

			case false:
				isOn = true;
				break;
			}
			toggleAction(isOn);
		}
	}

}
