using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject optionsPanel;
    public GameObject levelPanel;

    public void OpenMenu()
    {
        optionsPanel.SetActive(false);
        levelPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        levelPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void OpenLevels()
    {
        optionsPanel.SetActive(false);
        levelPanel.SetActive(true);
        menuPanel.SetActive(false);
    }
}
