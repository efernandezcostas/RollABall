using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;   // Referencia al jugador
    public float rotationSpeed = 0f; // Velocidad de rotación
    private Vector3 offset;    // Distancia inicial entre la cámara y el jugador

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // Rotación automática alrededor del jugador
        Quaternion camTurnAngle = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up); // Rotar alrededor del eje Y
        offset = camTurnAngle * offset; // Aplicar la rotación al offset

        // Actualizar posición de la cámara
        transform.position = player.transform.position + offset;

        // Mirar siempre hacia el jugador
        transform.LookAt(player.transform.position);
    }
}
