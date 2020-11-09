using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;

    private const float FadeInDuration = 1f;

    public bool open;

    private void Start()
    {
        open = false;
        winPanel.SetActive(false);
        winPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void OpenWinPanel(string winingTeam)
    {
        open = true;
        winPanel.GetComponentInChildren<TMP_Text>().text = winingTeam + " has won!";
        var canvasGroup = winPanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        winPanel.SetActive(true);
        StartCoroutine(_fadePanel(canvasGroup, canvasGroup.alpha, 1));
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator _fadePanel(CanvasGroup panel, float start, float end)
    {
        var counter = 0f;
        while (counter < FadeInDuration)
        {
            counter += Time.deltaTime;
            panel.alpha = Mathf.Lerp(start, end, counter / FadeInDuration);
            yield return null;
        }
    }
}
