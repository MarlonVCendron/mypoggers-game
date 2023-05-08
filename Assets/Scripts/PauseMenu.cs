using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    public AudioSource ambientSound;
    public AudioSource rainSound;
    public AudioSource bichoSound;
    public ConfirmationDialog confirmationDialog;

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
        ambientSound.Pause();
        rainSound.Pause();
        bichoSound.Pause();
    }
    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ambientSound.Play();
        rainSound.Play();
        bichoSound.Play();
    }
    public void GoToMainMenu(){
        confirmationDialog.Show("Voltar ao titulo", "Voce tem certeza que quer voltar para a tela de titulo?",
            () => {
                SceneManager.LoadScene("Menu");
                isPaused = false;
            },
            () => {
                isPaused = false;
            }
        );
    }
    public void QuitGame(){
        confirmationDialog.Show("Sair do jogo", "Voce tem certeza que quer sair do jogo?",
            () => {
                Application.Quit();
            },
            () => {}
        );
    }
}
