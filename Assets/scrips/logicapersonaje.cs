using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LogicaPersonaje : MonoBehaviour
{
    private int score = 0;
    public GameObject foodPrefab;
    public GuardiaController guardia; // Asignar desde el Inspector

    void Start()
    {
        SpawnFood();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            score += 10;
            Destroy(other.gameObject);
            SpawnFood();

            guardia.IniciarHuida(); // Activar huida del guardia

            if (score >= 50)
            {
                GameManager.Instance.GanarJuego();
                StartCoroutine(SaveScore());
            }
        }
    }

    void SpawnFood()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-50f, 50f),
            2f,
            Random.Range(-40f, 40f)
        );
        Instantiate(foodPrefab, randomPos, Quaternion.identity);
    }

    IEnumerator SaveScore()
    {
        WWWForm form = new WWWForm();
        string playerName = "Jugador de juego";

        string json = JsonUtility.ToJson(new ScoreData(playerName, score));
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest webRequest = new UnityWebRequest("http://localhost/save_score.php", "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Respuesta: " + webRequest.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class ScoreData
    {
        public string player_name;
        public int score;

        public ScoreData(string name, int score)
        {
            this.player_name = name;
            this.score = score;
        }
    }






}