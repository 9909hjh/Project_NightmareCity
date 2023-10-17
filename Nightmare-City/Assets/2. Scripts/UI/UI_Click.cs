using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum BTNType
{
    Next,
    Restart,
    MainMenu,
    Back,
    Continue_Yes,
    Continue_No
}

public class UI_Click : MonoBehaviour
{
    [Tooltip("각 버튼의 기능 선택")]
    [SerializeField] private BTNType currentType;
    public UI_test uiTest;
    public GameObject crossHair;
    [Tooltip("ESC 누르면 나오는 오브젝트 그룹")]
    [SerializeField] private CanvasGroup optionGroup;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Next:
                SceneManager.LoadScene("1stage");
                break;
            case BTNType.Restart:
                SceneManager.LoadScene(PlayerCtrl.stageNum + "stage");
                PlayerCtrl.key = 0;
                break;
            case BTNType.MainMenu:
                SceneManager.LoadScene("Main Menu");
                PlayerCtrl.stageNum = 1;
                PlayerCtrl.key = 0;
                break;
            case BTNType.Back:
                uiTest.CanvasGroupOff(optionGroup);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                crossHair.SetActive(true);
                uiTest.CameraOn();
                break;
            case BTNType.Continue_Yes:
                SceneManager.LoadScene(PlayerCtrl.stageNum + "stage");
                PlayerCtrl.key = 0;
                break;
            case BTNType.Continue_No:
                SceneManager.LoadScene("Main Menu");
                PlayerCtrl.stageNum = 1;
                PlayerCtrl.key = 0;
                break;
        }
    }
}