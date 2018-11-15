using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public Camera mainCamera;
    MainMenuCameraRotation mainCameraBehaviour;
    private Quaternion startingPointRotation;
    //private Quaternion fromRotation;
    private Vector3 startingPoint;
    private Vector3 customisationCameraPos = new Vector3(-1.307987f, 1.56591f, -15.09972f);
    private bool customisationAnimation;
    private bool restoreRotation;
    private bool zoomAnimationComplete;
    private float timeCount = 0.0f;

    public void Awake() {
        mainCameraBehaviour = mainCamera.GetComponent<MainMenuCameraRotation>();
        startingPointRotation = mainCamera.transform.rotation;
        startingPoint = mainCamera.transform.position;
    }

    public void Update() {
        //timeCount += Time.deltaTime;
        /*if (customisationAnimation) {
            CustomisationAnimationRotation();
        }*/
    }

    /* Buggy does not work
    public void CustomisationAnimationRotation() {
        customisationAnimation = true;
        zoomAnimationComplete = false;
        mainCameraBehaviour.rotationSpeed = 0;
        if (customisationAnimation && !restoreRotation) {
            restoreRotation = true;
        }
        if (restoreRotation) {
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, startingPointRotation, timeCount);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, startingPoint, timeCount);
            timeCount = timeCount + Time.deltaTime;
            if (mainCamera.transform.rotation == startingPointRotation && mainCamera.transform.position == startingPoint) {
                restoreRotation = false;
            }
        }
        if(restoreRotation == false && zoomAnimationComplete == false) {
            //mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, startingPointRotation, timeCount);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, customisationCameraPos, timeCount);
            if (mainCamera.transform.position == customisationCameraPos) {
                mainCameraBehaviour.rotationSpeed = 0;
                zoomAnimationComplete = true;
                customisationAnimation = false;
                Debug.Log("zoomComplete");
            }
        }
    }*/

    



    public void PlayGame() {
        SceneManager.LoadScene("CustomisationScreen");
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game has been quit");
    }

}
