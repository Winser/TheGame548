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
    public GameObject fader_set_off;
    private float timer;
    void Start() 
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if(timer>= 3)
        {
            fader_set_off.SetActive(false);
        }
        AudioListener.volume = volume.value;
        GameObject.Find("Volume_val").GetComponent<Text>().text = Convert.ToString(Mathf.Round(volume.value * 100));

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
