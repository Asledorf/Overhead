using UnityEngine;

public class CameraControllerScript : MonoBehaviour 
{
	public float panSpeed = 30f;
	public float panBoardThickness = 10f;
	public float scrollSpeed = 5f;
	public float scrollMaxSensibility = 1f;

	public float minX = 10f;
	public float maxX = 80f;
	public float minZ = 10f;
	public float maxZ = 80f;

	private void Update()
	{
		MoveScreen();
		ZoomLevelFromScroll();
		ClampCameraPosition();
	}

	private void MoveScreen()
	{
		int x = 0, z = 0;
		if
		(
			Input.GetKey(KeyCode.W)
			|| (Input.mousePosition.y >= (Screen.height - panBoardThickness) && Input.mousePosition.y <= Screen.height)
			|| Input.GetKey(KeyCode.UpArrow)
		) z++;

		if 
		( 
			Input.GetKey (KeyCode.S) 
			|| ( Input.mousePosition.y <= panBoardThickness && Input.mousePosition.y >= 0f ) 
			|| Input.GetKey(KeyCode.DownArrow)
		) z--;

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

		transform.Translate(panSpeed * Time.deltaTime * new Vector3(x,0,z).normalized, Space.Self);
	}

	private void ZoomLevelFromScroll()
	{
		float scroll = LimitScrollSensibility(Input.GetAxis("Mouse ScrollWheel") );

		//new code for scroll logic
		if (scroll != 0)
		{
			Camera.main.orthographicSize -= scroll;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 4, 12);
		}
	}

	private float LimitScrollSensibility(float scroll)
	{
		return Mathf.Clamp(scroll, (-1f) * scrollMaxSensibility, scrollMaxSensibility);
	}

	private void ClampCameraPosition()
	{
		float posX, posZ;
		posX = Mathf.Clamp(transform.position.x, minX, maxX);
		posZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
		transform.position = new Vector3(posX,transform.position.y,posZ);
	}
}