using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TittleScreen : MonoBehaviour
{
    public ConfirmationDialog confirmationDialog;

    public void StartGame(){
        SceneManager.LoadScene("Game");
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
