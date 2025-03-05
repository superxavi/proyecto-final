using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GanarJuego()
    {
        Debug.Log("¡Victoria! Puntaje alcanzado");
        Time.timeScale = 0f;
        // Aquí agregar lógica de UI o cambio de escena
    }

    public void PerderJuego()
    {
        Debug.Log("Derrota - Guardia te atrapó");
        Time.timeScale = 0f;
        // Aquí agregar lógica de UI o cambio de escena
    }
}