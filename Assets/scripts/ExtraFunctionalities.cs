using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraFunctionalities : MonoBehaviour {

	[SerializeField] KeyCode pauseButton;
	[SerializeField] KeyCode advanceTimeButton;
	[SerializeField] KeyCode screenShotButton;
	[SerializeField] KeyCode sequencedScreenShotButton;
	[SerializeField] Image advanceTimeImage;
	[SerializeField] int screenShotResolution = 1;
	[SerializeField] int sequencedNumber = 5;
	[SerializeField] bool resetNumber = false;

	int ScreenShotNum;
	float storedTimeScale = 1;

	void Start () 
	{
		if (resetNumber) 
		{
			PlayerPrefs.SetInt ("ssNumber", 0);
			resetNumber = false;
		}

		ScreenShotNum = PlayerPrefs.GetInt ("ssNumber");

		Time.timeScale = 1;
		SetPlaySpeed_Normal();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(pauseButton)) 
		{
			if(Time.timeScale != 0)
				SetPlaySpeed_Pause();
            else
            {
				if (storedTimeScale == 1)
					SetPlaySpeed_Normal();
				if(storedTimeScale == 3)
					SetPlaySpeed_Fast();
            }
		}

		if (Input.GetKeyDown(advanceTimeButton))
		{
			if (Time.timeScale == 0) return;

			if (Time.timeScale == 1)
				SetPlaySpeed_Fast();
			else
				SetPlaySpeed_Normal();
		}

		if (Input.GetKeyDown(screenShotButton)) {
			TakeScreenShot ();
		}

		if (Input.GetKeyDown(sequencedScreenShotButton)) {
			StartCoroutine (TakeSequencedScreenShot());
		}
	}

	//time management
	void SetPlaySpeed_Fast()
	{
		storedTimeScale = Time.timeScale;
		Time.timeScale = 3;
		advanceTimeImage.enabled = true;
	}
	void SetPlaySpeed_Normal()
	{
		storedTimeScale = Time.timeScale;
        Time.timeScale = 1;
		advanceTimeImage.enabled = false;
	}
	void SetPlaySpeed_Pause()
	{
		storedTimeScale = Time.timeScale;
		Time.timeScale = 0;
		advanceTimeImage.enabled = false;
	}

	public void TakeScreenShot() {

		ScreenCapture.CaptureScreenshot ("Screenshots/" + ScreenShotNum + ".png", screenShotResolution); 
		Debug.Log ("Screenshot taken!");
		ScreenShotNum++;
		PlayerPrefs.SetInt ("ssNumber", ScreenShotNum);
	}

	IEnumerator TakeSequencedScreenShot () {
		for (int i = 0; i < sequencedNumber; i++) {
			TakeScreenShot ();
			yield return new WaitForSeconds (0.5f);
		}

		StopCoroutine (nameof(TakeSequencedScreenShot));
	}
}