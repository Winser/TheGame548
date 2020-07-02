using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Ui_script : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider volume;
    public GameObject settings;
    public GameObject menu;
    public GameObject fader;
    private float a; // изменение цвета по альфа каналу
    void Start() 
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        a += 0.1f;
        fader.GetComponent<Renderer>().color = new Color(0, 0, 0, 255 - a);
        AudioListener.volume = volume.value;
        GameObject.Find("Volume_val").GetComponent<Text>().text = Convert.ToString(Mathf.Round(volume.value * 100));
        Debug.Log(a);
    }
    public void New_game()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void Settings()
    {
        menu.SetActive(false);
        settings.SetActive(!settings.activeSelf);
    }

    public void Quit ()
    {
        Application.Quit();
    }
    public void Cross_button()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }
    public void SetSound()
    {
        Menu_settings.sound = volume.value;
    }
}
