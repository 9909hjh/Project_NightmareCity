using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_test : MonoBehaviour
{
    void Update()
    {
        Escape();
    }

    [Tooltip("ESC 누르면 나오는 오브젝트 그룹")]
    [SerializeField] private CanvasGroup optionGroup;
    public GameObject dieCamera;
    public GameObject crossHair;
    void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CameraOff();
            crossHair.SetActive(false);
            CanvasGroupOn(optionGroup); // 옵션 캔버스 그룹 온
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void CameraOn()
    {
        dieCamera.GetComponent<CameraMove>().enabled = true; // 죽었을 때 카메라 움직이지 않게 하려고 이름 저따구임
    }
    public void CameraOff()
    {
        dieCamera.GetComponent<CameraMove>().enabled = false; // 죽었을 때 카메라 움직이지 않게 하려고 이름 저따구임
    }
}