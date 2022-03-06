using UnityEngine;

public class Spin : MonoBehaviour
{
	public float RotationSpeed = 1;
	public float Angle = 90;
	void Update()
	{
		//transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));

		//float _ = Mathf.Lerp(-Angle, Angle, RotationSpeed * Time.deltaTime);
		//transform.Rotate(Vector3.up * _);


		float rY = Mathf.SmoothStep(0, Angle, Mathf.PingPong(Time.time * RotationSpeed, 1));
		transform.rotation = Quaternion.Euler(0, rY+90, 0);
	}
}