using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour 
{
	public float panSpeed = 30f;
	public float panBoardThickness = 10f;
	public float scrollSpeed = 5f;
	public float scrollMaxSensibility = 1f;
	public float moduleDimension;
	public float minY = 10f;
	public float maxY = 80f;

	float frontRation;
	float backRation;
	float rightRation;
	float leftRation;
	float minXInMaxY = 10f;
	float maxXInMaxY = 80f;
	float minZInMaxY = 10f;
	float maxZInMaxY = 80f;
	float minX = 10f;
	float maxX = 80f;
	float minZ = 10f;
	float maxZ = 80f;
	float epsilon = 0.0001f;
	float posX;
	float posY;
	float posZ;

	Animator anim;

	private void Update()
	{
		MoveScreen();
		ZoomScroll();
		LimitPosition();

		if (Input.GetKeyDown(KeyCode.C) && Application.isEditor) //seems to be debug?
			ShakeBySpeed(1f, 1f);
	}

	private void Start()
	{
		minXInMaxY = (-1f) * moduleDimension - 8.5f;
		maxXInMaxY = moduleDimension - 23f;
		minZInMaxY = (-1f) * moduleDimension - 16.5f;
		maxZInMaxY = moduleDimension - 33f;

		frontRation = (maxZInMaxY - moduleDimension) / maxY;
		backRation  = (minZInMaxY + moduleDimension) / maxY;
		leftRation  = (minXInMaxY + moduleDimension) / maxY;
		rightRation = (maxXInMaxY - moduleDimension) / maxY;

		anim = GetComponentInChildren<Animator>();
	}

	//move the camera white wasd or with mouse in the border
	private void MoveScreen()
	{
		int x = 0, y = 0;
		if
		(
			Input.GetKey(KeyCode.W)
			|| (Input.mousePosition.y >= (Screen.height - panBoardThickness) && Input.mousePosition.y <= Screen.height)
			|| Input.GetKey(KeyCode.UpArrow)
		) y++;

		if 
		( 
			Input.GetKey (KeyCode.S) 
			|| ( Input.mousePosition.y <= panBoardThickness && Input.mousePosition.y >= 0f ) 
			|| Input.GetKey(KeyCode.DownArrow)
		) y--;

		if
		(
			Input.GetKey(KeyCode.D) ||
			(Input.mousePosition.x >= Screen.width - panBoardThickness && Input.mousePosition.x <= Screen.width) ||
			Input.GetKey(KeyCode.RightArrow)
		) x++;

		if
		(
			Input.GetKey(KeyCode.A)
			|| (Input.mousePosition.x <= panBoardThickness && Input.mousePosition.x >= 0f)
			|| Input.GetKey(KeyCode.LeftArrow)
		) x--;

		transform.Translate(panSpeed * Time.deltaTime * new Vector3(x,y).normalized, Space.Self);
	}

	//Transform mouse scroll into zoom and defines min and max distances
	private void ZoomScroll()
	{
		float scroll = LimitScrollSensibility(Input.GetAxis("Mouse ScrollWheel") );

		//new code for scroll logic
		if (scroll != 0)
		{
			Camera.main.orthographicSize -= scroll;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 4, 12);
		}

		if ( transform.position.y > minY - epsilon && transform.position.y < maxY + epsilon )
			ZoomMovement(scroll);
	}

	private float LimitScrollSensibility(float scroll)
	{
		return Mathf.Clamp(scroll, (-1f) * scrollMaxSensibility, scrollMaxSensibility);
	}

	
	//Camera zoom moviment
	private void ZoomMovement(float scroll)
	{
		Vector3 triedPosition = transform.position;
		triedPosition.y -= scroll * scrollSpeed * 200 * Time.deltaTime;

		if ( triedPosition.y > minY - epsilon && triedPosition.y < maxY + epsilon )
		{
			ZoomVerticalMovement(triedPosition);
			ZoomHorizontalMovement(scroll);
			return;
		}
		if ( triedPosition.y <= minY - epsilon )
		{
			float triedMoviment = Mathf.Abs(transform.position.y - triedPosition.y );
			float maxDisplacement = Mathf.Abs(transform.position.y - (minY - epsilon));
			float movimentRation = maxDisplacement / triedMoviment;
			triedPosition.y = minY;
			ZoomVerticalMovement( triedPosition );
			ZoomHorizontalMovement( scroll * movimentRation );
			return;
		}
		if (triedPosition.y >= maxY + epsilon)
		{
			float triedMoviment = Mathf.Abs( transform.position.y - triedPosition.y );
			float maxDisplacement = Mathf.Abs( transform.position.y - (maxY + epsilon) );
			float movimentRation = maxDisplacement / triedMoviment;
			triedPosition.y = maxY;
			ZoomVerticalMovement( triedPosition );
			ZoomHorizontalMovement(scroll * movimentRation);
			return;
		}
	}

	private void ZoomVerticalMovement(Vector3 pos)
	{
		transform.position = pos;
	}

	private void ZoomHorizontalMovement(float scroll)
	{
		//Vector3 pos = transform.position;
		transform.Translate (Vector3.forward * scroll * scrollSpeed * 200 * Time.deltaTime, Space.Self);
	}

	private void LimitPosition()
	{
		SetPos();
		SetCameraEdgesForTheHeigh();
		ClampCameraPosition();
		transform.position = new Vector3 (posX, posY, posZ);
	}

	private void SetPos()
	{
		posX = transform.position.x;
		posY = transform.position.y;
		posZ = transform.position.z;
	}

	private void SetCameraEdgesForTheHeigh()
	{
		minX = leftRation * posY - moduleDimension;
		minZ = backRation * posY - moduleDimension;
		maxX = rightRation * posY + moduleDimension;
		maxZ = frontRation * posY + moduleDimension;
	}

	private void ClampCameraPosition()
	{
		posX = Mathf.Clamp(posX, minX, maxX);
		posY = Mathf.Clamp(posY, minY, maxY);
		posZ = Mathf.Clamp(posZ, minZ, maxZ);
	}

	/// <summary>
	/// Function to shake the camera. It's animation-based, then it's not dependent of camera's transform
	/// </summary>
	/// <param name="speed">The speed of the animation playing</param>
	/// <param name="percentageOfAnimationToShow">Which percentage of the animation should play</param>
	public void ShakeBySpeed(float speed, float percentageOfAnimationToShow)
	{
		//float t0 = Time.time;
		anim.SetFloat("speed", speed);
		StartCoroutine(Shake(percentageOfAnimationToShow));
		/*
		float a = 0.1f;
		float w = 1f;
		float phi = 0f;
		while (anim.GetBool("shake"))
		{
			anim.SetFloat("speed", a * Mathf.Cos(w * (Time.time - t0)));
		}*/
	}

	IEnumerator Shake(float t)
	{
		anim.SetBool("shake", true);
		yield return new WaitForSeconds(t);
		anim.SetBool("shake", false);
	}
}
