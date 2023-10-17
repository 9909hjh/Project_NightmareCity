using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Press : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(offText());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(PressAnyKey());
        }
    }

    // 아무 키나 누르면 실행되는 동작 방식 //
    [Tooltip("Fade 오브젝트 받아오기")]
    [SerializeField] private UI_Fade Fade;
    [Tooltip("Background_Objects 오브젝트")]
    [SerializeField] private GameObject Bg_Objs;
    [Tooltip("First 오브젝트")]
    [SerializeField] private GameObject first;
    [Tooltip("Tutorial 이미지 오브젝트")]
    [SerializeField] private GameObject tutorialImg;
    [Tooltip("Next 텍스트 오브젝트")]
    [SerializeField] private GameObject nextBtn;
    public IEnumerator PressAnyKey()
    {
        Fade.FadeIn();
        yield return new WaitForSeconds(1.5f);
        Bg_Objs.SetActive(false);
        first.SetActive(false);

        tutorialImg.SetActive(true);
        nextBtn.SetActive(true);
    }
    // ================================ //

    // 텍스트 깜빡이는 효과 //
    [Tooltip("PressAnyKey 텍스트 오브젝트")]
    [SerializeField] private Text text;
    public IEnumerator onText()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.2f);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(offText());
    }
    public IEnumerator offText()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.2f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(onText());
    }
    // ================== //
}