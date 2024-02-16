using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Threading;

public class SistemaPerguntas : MonoBehaviour
{
    private JsonPerguntasReader jsonPerguntasReader;

    public TextMeshProUGUI score;

    public TextMeshProUGUI perguntaText;
    public TextMeshProUGUI comentarioText;

    public TextMeshProUGUI respostaOpcao0;
    public TextMeshProUGUI respostaOpcao1;
    public TextMeshProUGUI respostaOpcao2;
    public TextMeshProUGUI respostaOpcao3;
    public TextMeshProUGUI respostaOpcao4;

    int perguntaSelecionada;
    List<int> perguntasUsadas = new List<int>();

    public GameObject perguntas;
    public GameObject respostas;
    public GameObject comentario;
    public GameObject pista;

    public int timer = 60;
    public TextMeshProUGUI timerText;

    void Start()
    {
        jsonPerguntasReader = FindObjectOfType<JsonPerguntasReader>();

        StartCoroutine(Timer());
        GerarPergunta();
    }

    public void GerarPergunta()
    {
        Aleatorizador();
    timer = 60;

    var perguntaAtual = jsonPerguntasReader.listaPerguntas[perguntaSelecionada];

    perguntaText.text = perguntaAtual.pergunta.ToString();
    comentarioText.text = perguntaAtual.comentario.ToString();

    // Embaralhar as opções de resposta
    List<string> opcoesEmbaralhadas = new List<string>(perguntaAtual.opcoes);
    Shuffle(opcoesEmbaralhadas);

    // Atribuir as opções embaralhadas aos textos das opções
    respostaOpcao0.text = opcoesEmbaralhadas[0];
    respostaOpcao1.text = opcoesEmbaralhadas[1];
    respostaOpcao2.text = opcoesEmbaralhadas[2];
    respostaOpcao3.text = opcoesEmbaralhadas[3];
    respostaOpcao4.text = opcoesEmbaralhadas[4];
}

// Função para embaralhar uma lista
private void Shuffle<T>(List<T> list)
{
    int n = list.Count;
    while (n > 1)
    {
        n--;
        int k = UnityEngine.Random.Range(0, n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
    }
}

    void Aleatorizador()
    {
        if (perguntasUsadas.Count == jsonPerguntasReader.listaPerguntas.Count)
        {
            perguntasUsadas.Clear();
        }

        perguntaSelecionada = UnityEngine.Random.Range(0, jsonPerguntasReader.listaPerguntas.Count - 1);

        if (perguntasUsadas.Contains(perguntaSelecionada))
        {
            Aleatorizador();
            return;
        }

        perguntasUsadas.Add(perguntaSelecionada);
    }

    void RespostaErrada()
    {
        timer = 0;
        respostas.SetActive(false);
        comentario.SetActive(true);
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(timer);

            if (perguntas.activeSelf && timer > -1)
            {
                if (timer >= 0) timerText.text = timer.ToString();
                timer--;
            }
            else if (timer == -1) RespostaErrada();
        }
    }

    public void BotaoResposta(int alternativa)
    {
        var respostaCorreta = jsonPerguntasReader.listaPerguntas[perguntaSelecionada].resposta_correta;

        GameManager.Instance.perguntasRespondidas++;

        if (respostaCorreta == alternativa)
        {
            GameManager.Instance.perguntasAcertadas++;
            CaseManager.Instance.playerScore++;
            score.text = score.text + "* ";

            if (CaseManager.Instance.playerScore >= 10)
            {
                Thread.Sleep(500);
                CaseManager.Instance.playerScore = 0;
                score.text = "";
                perguntas.SetActive(false);
                pista.SetActive(true);
            }

            GerarPergunta();
        }
        else RespostaErrada();
    }

    public void FecharComentario()
    {
        respostas.SetActive(true);
        comentario.SetActive(false);
        GerarPergunta();
    }
}
