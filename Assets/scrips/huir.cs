using UnityEngine;

public class Huir : MonoBehaviour
{
    public float velocidad = 5f;
    public Transform objetivo;
    public float distanciaSegura = 10f;

    private float yInicial;

    void Start()
    {
        yInicial = transform.position.y;
    }

    void Update()
    {
        Vector3 direccion = transform.position - objetivo.position;

        if (direccion.magnitude > distanciaSegura)
        {
            direccion = Quaternion.Euler(0, Random.Range(-45, 45), 0) * direccion;
        }

        Vector3 movimiento = direccion.normalized * velocidad * Time.deltaTime;
        Vector3 nuevaPos = transform.position + movimiento;
        nuevaPos.y = yInicial;

        transform.position = Vector3.MoveTowards(transform.position, nuevaPos, velocidad * Time.deltaTime);

        if (direccion != Vector3.zero)
        {
            Quaternion rotacion = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5f);
        }
    }
}