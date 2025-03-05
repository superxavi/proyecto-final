using UnityEngine;

public class Intercepcion : MonoBehaviour
{
    public GameObject objetivo;  // Asignar al jugador en el Inspector
    public GameObject prediccion;
    public float velocidad = 5f;

    // Variables de cálculo
    private Vector3 velRelativa;
    private Vector3 miVelocidad;
    private Vector3 velObjetivo;
    private Vector3 miPosAnterior;
    private Vector3 objPosAnterior;
    private Vector3 distanciaRelativa;
    private float tiempoIntercepcion;
    private Vector3 posFutura;

    void Start()
    {
        // Inicialización segura de posiciones anteriores
        miPosAnterior = transform.position;
        objPosAnterior = objetivo.transform.position;

        // Inicializar velocidades en cero
        miVelocidad = Vector3.zero;
        velObjetivo = Vector3.zero;

        // Validación de referencia crítica
        if (objetivo == null)
        {
            Debug.LogError("¡No hay objetivo asignado en Intercepcion!");
            enabled = false;  // Desactiva el script si no hay objetivo
        }
    }

    void Update()
    {
        // 1. Cálculo de velocidades con validación de deltaTime
        if (Time.deltaTime > Mathf.Epsilon)  // Evita división por cero
        {
            miVelocidad = (transform.position - miPosAnterior) / Time.deltaTime;
            velObjetivo = (objetivo.transform.position - objPosAnterior) / Time.deltaTime;
        }
        else
        {
            miVelocidad = Vector3.zero;
            velObjetivo = Vector3.zero;
        }

        // Actualizar posiciones anteriores
        miPosAnterior = transform.position;
        objPosAnterior = objetivo.transform.position;

        // 2. Cálculo de velocidad relativa
        velRelativa = velObjetivo - miVelocidad;

        // 3. Validación de magnitud para evitar NaN
        if (velRelativa.magnitude <= Mathf.Epsilon || float.IsNaN(velRelativa.magnitude))
        {
            // Modo de persecución directa si no hay movimiento relativo
            posFutura = objetivo.transform.position;
        }
        else
        {
            // Cálculo normal de intercepción
            distanciaRelativa = objetivo.transform.position - transform.position;
            tiempoIntercepcion = distanciaRelativa.magnitude / velRelativa.magnitude;
            posFutura = objetivo.transform.position + velObjetivo * tiempoIntercepcion;
        }

        // 4. Validación final de posición futura
        if (IsValidPosition(posFutura))
        {
            // Movimiento suave hacia la posición
            transform.position = Vector3.MoveTowards(
                transform.position,
                posFutura,
                velocidad * Time.deltaTime
            );
        }
        else
        {
            // Fallback: Persecución directa
            transform.position = Vector3.MoveTowards(
                transform.position,
                objetivo.transform.position,
                velocidad * Time.deltaTime
            );
        }

        // 5. Rotación hacia la dirección de movimiento
        Vector3 direccion = (posFutura - transform.position).normalized;
        if (direccion != Vector3.zero && !float.IsNaN(direccion.x))
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                Time.deltaTime * 5f
            );
        }

        // Actualización de predicción visual (opcional)
        if (prediccion != null && IsValidPosition(posFutura))
        {
            prediccion.transform.position = posFutura;
        }
    }

    // Método para validar posiciones
    private bool IsValidPosition(Vector3 pos)
    {
        return !float.IsNaN(pos.x) && !float.IsNaN(pos.y) && !float.IsNaN(pos.z);
    }
}