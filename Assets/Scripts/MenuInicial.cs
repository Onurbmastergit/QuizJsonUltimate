using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Networking;

public class MenuInicial : MonoBehaviour
{
    public GameObject painelInicial;
    public GameObject video;
    public VideoPlayer videoPlayer;

    public string videoURL = "URL_do_seu_video.mp4";

    private void Start()
    {
        if (!GameManager.Instance.gameStarted)
        {
            painelInicial.SetActive(true);
            GameManager.Instance.gameStarted = true;
        } else painelInicial.SetActive(false);
    }

    public void Entrar()
    {
        painelInicial.SetActive(false);
    }

    public void Sair()
    {
        Debug.Log("Comando: sair da p�gina web atual para a desejada");
        Application.OpenURL("https://www.example.com");
    }

    public void Voltar()
    {
        painelInicial.SetActive(true);
    }

    public void CasoSelecionado(int i)
    {
        if(GameManager.Instance == null)
        {
            return;
        }
        GameManager.Instance.casoSelecionado = i;
        Debug.Log($"Caso Selecionado: {i}");
    }

    public void StartGame()
    {
         video.SetActive(true);

        // Inicia a corrotina para reproduzir o vídeo antes de iniciar o jogo
        StartCoroutine(PlayVideoAndLoadScene());
       
    }
    IEnumerator PlayVideoAndLoadScene()
    {
        // Define a URL do vídeo a ser reproduzido
        videoPlayer.url = videoURL;

        // Prepara o vídeo
        videoPlayer.Prepare();

        // Aguarda até que o vídeo esteja pronto para ser reproduzido
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Reproduz o vídeo
        videoPlayer.Play();

    }
    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayButton()
    {
        videoPlayer.Play();

    }
    public void PauseButton()
    {
        videoPlayer.Pause();
    }
    public void RestartButton()
    {
         // Parar o vídeo
        videoPlayer.Stop();

        // Voltar para o início
        videoPlayer.frame = 0;

        // Reproduzir o vídeo novamente
        videoPlayer.Play();
    }
  
}
