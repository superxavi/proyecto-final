using UnityEngine;
using System.Collections; // Añade esto al inicio del archivo
public class GuardiaController : MonoBehaviour
{
    public enum Estado { Patrullando, Persiguiendo, Huyendo }
    public Estado estadoActual;
    public float tiempoHuir = 3f;

    [Header("Referencias")]
    public Transform jugador;
    private Intercepcion intercepcion;
    private Huir huir;
    private verJugador vision;

    void Start()
    {
        intercepcion = GetComponent<Intercepcion>();
        huir = GetComponent<Huir>();
        vision = GetComponent<verJugador>();

        intercepcion.objetivo = jugador.gameObject;
        huir.objetivo = jugador;

        CambiarEstado(Estado.Patrullando);
    }

    void Update()
    {
        if (estadoActual != Estado.Huyendo && vision.PlayerVisible())
        {
            CambiarEstado(Estado.Persiguiendo);
        }
    }

    public void IniciarHuida()
    {
        if (estadoActual != Estado.Huyendo)
        {
            StartCoroutine(HuirTemporalmente());
        }
    }

    IEnumerator HuirTemporalmente()
    {
        CambiarEstado(Estado.Huyendo);
        yield return new WaitForSeconds(tiempoHuir);
        CambiarEstado(vision.PlayerVisible() ? Estado.Persiguiendo : Estado.Patrullando);
    }

    void CambiarEstado(Estado nuevoEstado)
    {
        estadoActual = nuevoEstado;
        intercepcion.enabled = (estadoActual == Estado.Persiguiendo);
        huir.enabled = (estadoActual == Estado.Huyendo);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && estadoActual == Estado.Persiguiendo)
        {
            GameManager.Instance.PerderJuego();
        }
    }
}