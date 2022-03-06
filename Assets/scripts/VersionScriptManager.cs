using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VersionScriptManager : MonoBehaviour
{
	//this entire fucking script should be replaced with 4 Mathf.PingPong(); calls over the CanvasGroup alpha values using coroutines...

	public float fadeTime = 1.5f;
	public float fadeSpeed = 0.8f;
	public float startCountdown = 2f;
	public float welcomeTextTime = 5f;
	public float Countdown1 = 1f;
	public float Countdown2 = 1f;

	private bool fadeVersionText = false;
	private bool fadeName1 = false;
	private bool fadeName2 = false;
	private bool fadeName3 = false;

	private int fadeDirVersionText = 1;
	private int fadeDirName1 = 1;
	private int fadeDirName2 = 1;
	private int fadeDirName3 = 1;

	private GameObject versionText;
	private GameObject name_1;
	private GameObject name_2;
	private GameObject name_3;

	private string version;

	private void Start()
	{
		versionText = GameObject.Find("VersionText");
		name_1 = GameObject.Find("Name1");
		name_2 = GameObject.Find("Name2");
		name_3 = GameObject.Find("Name3");
		version = GameObject.Find("_VERSION").GetComponent<_Version>().GetVersion();

		StartCoroutine(VersionSceneFlow());

		versionText.SetActive(false);
		name_1.SetActive(false);
		name_2.SetActive(false);
		name_3.SetActive(false);

		versionText.GetComponent<CanvasGroup>().alpha = 0;
		name_1.GetComponent<CanvasGroup>().alpha = 0;
		name_2.GetComponent<CanvasGroup>().alpha = 0;
		name_3.GetComponent<CanvasGroup>().alpha = 0;

		versionText.GetComponent<Text>().text = "Overhead - v" + version;
	}

	private IEnumerator VersionSceneFlow()
	{
		yield return new WaitForSeconds(startCountdown);
		yield return new WaitForSeconds(fadeTime);

		versionText.SetActive(true);
		fadeDirVersionText = 1;
		fadeVersionText = true;
		yield return new WaitForSeconds(fadeTime);
		yield return new WaitForSeconds(fadeTime);
		fadeDirVersionText = -1;
		yield return new WaitForSeconds(fadeTime);

		name_1.SetActive(true);
		fadeDirName1 = 1;
		fadeName1 = true;
		yield return new WaitForSeconds(fadeTime);
		name_2.SetActive(true);
		fadeDirName2 = 1;
		fadeName2 = true;
		yield return new WaitForSeconds(fadeTime);
		fadeDirName1 = -1;
		name_3.SetActive(true);
		fadeDirName3 = 1;
		fadeName3 = true;
		yield return new WaitForSeconds(fadeTime);
		fadeDirName2 = -1;
		yield return new WaitForSeconds(fadeTime);
		fadeDirName3 = -1;
		yield return new WaitForSeconds(fadeTime);

		yield return new WaitForSeconds(fadeTime);
		yield return new WaitForSeconds(startCountdown);
		SceneManager.LoadScene("MainMenu");
	}


	void Update()
	{
		CanvasGroup x;
		if (fadeVersionText)
		{
			x = versionText.GetComponent<CanvasGroup>();
			x.alpha += fadeDirVersionText * fadeSpeed * Time.deltaTime;
			x.alpha = Mathf.Clamp01(x.alpha);
		}
		if (fadeName1)
		{
			x = name_1.GetComponent<CanvasGroup>();
			x.alpha += fadeDirName1 * fadeSpeed * Time.deltaTime;
			x.alpha = Mathf.Clamp01(x.alpha);
		}
		if (fadeName2)
		{
			x = name_2.GetComponent<CanvasGroup>();
			x.alpha += fadeDirName2 * fadeSpeed * Time.deltaTime;
			x.alpha = Mathf.Clamp01(x.alpha);
		}
		if (fadeName3)
		{
			x = name_3.GetComponent<CanvasGroup>();
			x.alpha += fadeDirName3 * fadeSpeed * Time.deltaTime;
			x.alpha = Mathf.Clamp01(x.alpha);
		}
	}
}
