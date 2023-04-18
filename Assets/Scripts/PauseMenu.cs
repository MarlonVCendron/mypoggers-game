using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void GoToMainMenu(){
        Time.timeScale = 1f;
        bool playOutput = EditorUtility.DisplayDialog("Voltar ao título", "Você tem certeza que quer voltar para a tela de título", "Sim", "Não");
        if (playOutput){
            SceneManager.LoadScene("Menu");
        }
        isPaused = false;
    }
    public void QuitGame(){
        bool playOutput = EditorUtility.DisplayDialog("Sair do jogo", "Você tem certeza que quer sair do jogo?", "Sim", "Não");
        if (playOutput){
            Application.Quit();
        }
    }
}
