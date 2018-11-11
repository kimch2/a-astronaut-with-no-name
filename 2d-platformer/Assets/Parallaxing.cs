using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	private Transform mainCamera;
	private Vector3 previousCameraPosition;
	private float[] parallaxScales;

	public Transform[] Backgrounds;
	public float Smoothing = 1f;

	void Awake () {
		mainCamera = Camera.main.transform;
	}

	void Start () {
		previousCameraPosition = mainCamera.position;
        FillParallaxScales();
	}
	
	void Update () {
		ShiftBackgrounds();
		previousCameraPosition = mainCamera.position;
	}

	void FillParallaxScales() {
        parallaxScales = new float[Backgrounds.Length];

        int cont = 0;
        foreach (var background in Backgrounds)
        {
            parallaxScales[cont] = background.position.z * -1;
            cont++;
        }
	}

	void ShiftBackgrounds() {
        float parallax;
        float backgroundTargetPositionX;
        Vector3 backgroundTargetPosition;

        int cont = 0;
        foreach (var background in Backgrounds)
        {
            parallax = (previousCameraPosition.x - mainCamera.position.x) * parallaxScales[cont];

            backgroundTargetPositionX = background.position.x + parallax;

            backgroundTargetPosition = new Vector3(backgroundTargetPositionX, background.position.y, background.position.z);

            background.position = Vector3.Lerp(background.position, backgroundTargetPosition, Smoothing * Time.deltaTime);

            cont++;
        }
	}
}
