using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TittleScreen : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("Game");
    }

    public void QuitGame(){
        bool playOutput = EditorUtility.DisplayDialog("Sair do jogo", "Você tem certeza que quer sair do jogo?", "Sim", "Não");
        if (playOutput){
            Application.Quit();
        }
    }
}
