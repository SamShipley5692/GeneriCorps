using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraEffects : MonoBehaviour
{
    public Animator cameraAnimator;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Basic Rotation
            cameraAnimator.SetBool("CameraYes", false);
            cameraAnimator.SetBool("CameraNo", false);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // CAMERA YES ANIMATION
            cameraAnimator.SetBool("CameraYes", true);
            cameraAnimator.SetBool("CameraNo", false);

        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // CAMERA NO ANIMATION
            cameraAnimator.SetBool("CameraYes", true);
            cameraAnimator.SetBool("CameraNo", false);

        }


    }
}
