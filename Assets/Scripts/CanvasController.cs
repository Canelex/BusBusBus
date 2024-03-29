﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject panelGameOver;
    public GameObject buttonNextLevel;
    public GameObject buttonUnlockLevels;
    public GameObject buttonReplay;
    public GameObject buttonLevelSelect;
    public GameObject imageVictory;

    void Start()
    {
        panelGameOver.SetActive(false);
    }

    public void ShowGameCompleteUI()
    {
        imageVictory.SetActive(true);
        buttonLevelSelect.SetActive(true);
        Invoke("ShowCanvas", 0.75F);
    }

    public void ShowDefeatUI()
    {
        buttonReplay.SetActive(true);
        buttonLevelSelect.SetActive(true);

        Invoke("ShowCanvas", 0.75F); // Show UI in 3/4s.
    }

    public void ShowVictoryUI(bool nextLevelUnlocked)
    {
        UpdateVictoryUI(nextLevelUnlocked);
        Invoke("ShowCanvas", 0.75F); // Show UI in 3/4s.
    }

    public void UpdateVictoryUI(bool nextLevelUnlocked)
    {
        if (nextLevelUnlocked)
        {
            // Show next level button
            buttonUnlockLevels.SetActive(false);
            buttonNextLevel.SetActive(true);
        }
        else
        {
            // Show button to unlock more levels
            buttonNextLevel.SetActive(false);
            buttonUnlockLevels.SetActive(true);
        }

        buttonLevelSelect.SetActive(true);
    }

    void ShowCanvas()
    {
        AudioManager.Instance.Play("Pop"); // Play pop sound
        panelGameOver.SetActive(true);
    }
}
