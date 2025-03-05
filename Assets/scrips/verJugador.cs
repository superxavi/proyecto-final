using UnityEngine;

public class verJugador : MonoBehaviour
{
    public Transform objetivo;
    public float rangoVision = 50f;
    public float anguloVision = 30f;

    public bool PlayerVisible()
    {
        float distancia = Vector3.SqrMagnitude(transform.position - objetivo.position);
        if (distancia > rangoVision * rangoVision) return false;

        Vector3 direccionJugador = objetivo.position - transform.position;
        float angulo = Vector3.Angle(transform.forward, direccionJugador);
        if (angulo > anguloVision) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direccionJugador.normalized, out hit, rangoVision))
        {
            return hit.transform.CompareTag("Player");
        }
        return false;
    }
}