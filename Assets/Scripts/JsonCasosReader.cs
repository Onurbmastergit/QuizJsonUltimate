using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class JsonCasosReader : MonoBehaviour
{
    [Serializable]
    public class Case
    {
        public string nome_do_caso;
        public string pergunta;
        public int resposta_correta;
        public string[] opcoes;

        public Dictionary<string, string> pistas = new Dictionary<string, string>();
    }

    public List<Case> listaCasos = new List<Case>();


    // Primeiro, tornar o Start, ou o m�todo que for fazer a requisi��o como IEnumetor (ass�ncrono):
    IEnumerator Start()
    {
        // URL do arquivo JSON remoto
        string url = "https://conradosaud.com.br/outros/game_detetive/casos.json";

        // Cria uma solicita��o (request) de busca (GET) usando UnityWebRequest
        // Isso se chama consulta HTTP. Nesse projeto ser� usado o GET para buscar o id na API do cliente
        // Mas tamb�m ser� usado POST para criar um usu�rio (que nunca jogou) e o PUT para alterar um que j� existe
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Envia a solicita��o e aguarda a resposta
        yield return request.SendWebRequest();

        // Obt�m os dados JSON da resposta
        string jsonData = request.downloadHandler.text;

        // Fa�a o que quiser com os dados JSON
        JObject json = JObject.Parse(jsonData);

        foreach (var caso in json)
        {
            var test = caso.Value.ToObject<Case>();
            listaCasos.Add(caso.Value.ToObject<Case>());
        }
    }
}
