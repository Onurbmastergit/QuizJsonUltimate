using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using static JsonCasosReader;

public enum locationNames
{
    financeiro,
    ambulatorio,
    bloco_cirurgico,
    posto_de_enfermagem,
    laboratorio,
    oncologia,
    recepcao,
    pronto_socorro,
    lanchonete,
    uti,
    enfermaria,
    diretoria,
    radiologia,
    farmacia,
    pediatria,
    repouso_medico,
}

public class LocationTrigger : MonoBehaviour
{
    public locationNames ln;

    JsonCasosReader jsonCasosReader;

    public bool clueUnlocked = false;
    public GameObject alert;

    public TextMeshProUGUI pista;
    public TextMeshProUGUI local;
    public ImageLoader imageLoader;

    public string imageUrl; // Declarar imageUrl como uma propriedade pública


    List<string> nomeLocais = new List<string>()
    {
        "Financeiro",
        "Ambulat�rio",
        "Bloco Cir�rgico",
        "Posto de Enfermagem",
        "Laborat�rio",
        "Oncologia",
        "Recep��o",
        "Pronto Socorro",
        "Lanchonete",
        "UTI",
        "Enfermaria",
        "Diretoria",
        "Radiologia",
        "Farm�cia",
        "Pediatria",
        "Repouso M�dico"
    };


    void Start()
    {
        // Atribua o componente JsonCasosReader no Editor ou encontre-o dinamicamente
        jsonCasosReader = FindObjectOfType<JsonCasosReader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CaseManager.Instance.localAtual = ln;
        Debug.Log($"Entrando no {ln} {CaseManager.Instance.localAtual}");
        CaseManager.Instance.isMenuOpen = true;
        PistaSelecionadaLT();
        if (imageLoader == null)
        {
            Debug.LogError("ImageLoader não atribuído ao LocationTrigger.");
            return;
        }
        if (!clueUnlocked)
        {
            // Carrega a imagem do local específico
            imageLoader.LoadRemoteImage(imageUrl);

        }
        if (clueUnlocked == true)
        {
            CaseManager.Instance.painelPistaRecolhida.SetActive(true);
        }
        else QuestionTrigger();
    }

    void QuestionTrigger()
    {
        CaseManager.Instance.painelPerguntas.SetActive(true);
        CaseManager.Instance.isMenuOpen = true;

        clueUnlocked = true;
        CaseManager.Instance.pistasDebloqueadas.Add(ln.ToString());

        if (clueUnlocked)
        {
            alert.SetActive(false);

            Debug.Log($"===== Trigger: PISTA {ln} DESBLOQUEADA =====");
        }

        if (CaseManager.Instance.unlockResolution)
        {
            Debug.Log("===== Trigger: LIBERA O CASO FINAL =====");
        }

        if (CaseManager.Instance.allVisited)
        {
            Debug.Log("===== Trigger: TELA DE CASO FINAL =====");
        }
    }

    public void PistaSelecionadaLT()
    {
        int casoSelecionado = GameManager.Instance.casoSelecionado;
        locationNames localAtual = CaseManager.Instance.localAtual;

        var casoAtual = jsonCasosReader.listaCasos[casoSelecionado];

        pista.text = casoAtual.pistas[localAtual.ToString()];

        local.text = nomeLocais[(int)localAtual];
    }
}
