using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour 
{
	[SerializeField] private GameObject waveWarning = null;
	[SerializeField] private GameObject prepareYourself = null;

	public GameObject waveCanvas;
	public GameObject waveCooldown;
	public GameObject InitialAnimation;
	public GameObject tutorialNameCanvas;
	public GameObject canvas;

    private void Start()
    {
		if (!canvas) Debug.LogError("fuck 0");
		if (!waveCanvas) Debug.LogError("fuck 1");// 
		if (!waveCooldown) Debug.LogError("fuck 2");//
		if (!InitialAnimation) Debug.LogError("fuck 3");
		if (!tutorialNameCanvas) Debug.LogError("fuck 4");//
    }

    public void PlayWaveWarning()
	{
		waveWarning.GetComponent<Animation>().Play("FadeWarningCanvas");
	}

	public void PlayPrepareYourSelf()
	{
		prepareYourself.GetComponent<Animation>().Play("FadeWarningCanvas");
	}

	public void PlayInitialLoadingAnimation()
	{
		InitialAnimation.GetComponent<Animation>().Play("FadeWarningCanvas");
	}

	public void SetWaveCanvasAlphaWithDelay(float delay)
	{
		Invoke(nameof(SetWaveCanvasAlpha), delay);
	}

	public void SetWaveCoolDownAlphaWithDelay(float delay)
	{
		Invoke(nameof(SetCanvasAfterAnimation), delay);
	}

	public void SetWaveCanvasAlpha(float val = 1)
	{
		waveCanvas.GetComponent<CanvasGroup>().alpha = val;
	}

	public void SetWaveCoolDownAlpha(float val = 1)
	{
		waveCooldown.GetComponent<CanvasGroup>().alpha = val;
	}

	public void SetTutorialNameCanvasAlpha(float val = 1)
	{
		tutorialNameCanvas.GetComponent<CanvasGroup>().alpha = val;
	}

	public void AppearWaveCanvas()
	{
		waveCanvas.GetComponent<Animation>().Play("Appear");
	}

	public void DisappearWaveCanvas()
	{
		waveCanvas.GetComponent<Animation>().Play("Disappear");
	}

	public void AppearWaveCoolDown()
	{
		waveCooldown.GetComponent<Animation>().Play("Appear");
	}

	public void DisappearWaveCoolDown()
	{
		waveCooldown.GetComponent<Animation>().Play("Disappear");
	}

	public void AppearTutorialNameCanvas()
	{
		tutorialNameCanvas.GetComponent<Animation>().Play("Appear");
	}

	public void SetCanvasAlpha(float val)
	{
		for (int i = 0; i < canvas.transform.childCount; i++)
		{
			canvas.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = val;
		}
	}

	public void PlayAppearCanvasWithDelay(float delay)
	{
		Invoke(nameof(PlayAppearCanvas), delay);
	}

	public void SetCanvasAfterAnimationWithDelay(float delay)
	{
		Invoke(nameof(SetCanvasAfterAnimation), delay);
	}

	public void SetCanvasAfterAnimation()
	{
		SetCanvasAlpha(1);
		InitialAnimation.SetActive(false);
		canvas              .GetComponent<CanvasGroup>().alpha = 0;
		waveCanvas          .GetComponent<CanvasGroup>().alpha = 0;
		waveCooldown        .GetComponent<CanvasGroup>().alpha = 0;
		tutorialNameCanvas  .GetComponent<CanvasGroup>().alpha = 0;
	}

	public void PlayAppearCanvas()
	{
		canvas.GetComponent<Animation>().Play("Appear");
	}

	public void PlayDisappearCanvas()
	{
		canvas.GetComponent<Animation>().Play("Disappear");
	}
}
