using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject escPanel;

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider volumeSlider;
    
    private const float FadeInDuration = 1f;

    public bool open;

    private void Start()
    {
        open = false;
        winPanel.SetActive(false);
        winPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !open && !escPanel.activeSelf)
        {
            open = true;
            var canvasGroup = escPanel.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            escPanel.SetActive(true);
            StartCoroutine(_fadePanel(canvasGroup, canvasGroup.alpha, 1));
        }
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

    public void CloseEscPanel()
    {
        if (!open || !escPanel.activeSelf) return;
        open = false;
        escPanel.SetActive(false);
    }

    public void OnSpeedChange()
    {
        Time.timeScale = speedSlider.value / 10;
    }

    public void OnVolumeChange()
    {
        AudioListener.volume = volumeSlider.value;
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
