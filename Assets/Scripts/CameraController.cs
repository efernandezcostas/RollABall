using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define la clase CameraController que hereda de MonoBehaviour para interactuar con Unity
public class CameraController : MonoBehaviour
{
    // Referencia al objeto jugador para que la cámara pueda seguirlo
    public GameObject player;

    // Desplazamiento entre la cámara y el jugador (para posicionar la cámara a una distancia adecuada)
    private Vector3 offset;
    
    // Velocidad de rotación de la cámara
    public float rotationSpeed = 100f;

    // Desplazamiento de altura cuando la cámara está en modo primera persona
    public float firstPersonHeightOffset = 0.5f;

    // Ángulos de rotación de la cámara
    private float rotationX = 0f;
    private float rotationY = 0f;

    // Indicador de si la cámara está en modo primera persona o no
    private bool isFirstPerson = false;
    private Camera cameraComponent; // Componente de cámara

    // Este método se llama cuando comienza el juego
    void Start()
    {
        // Inicializamos la cámara y su desplazamiento
        cameraComponent = GetComponent<Camera>(); // Obtener el componente Camera
        offset = transform.position - player.transform.position; 

        // Establece el modo inicial de la cámara (no es primera persona)
        isFirstPerson = false;
        // Establecer el FOV por defecto en tercera persona (60)
        cameraComponent.fieldOfView = 60f;
    }

    // Este método se llama cada cuadro para verificar las entradas del usuario
    void Update()
    {
        // Si el jugador presiona la tecla 'R', cambia el modo de la cámara (primera persona o no)
        if (Input.GetKeyDown(KeyCode.R))
        {
            isFirstPerson = !isFirstPerson;
            // Cambiar el FOV cuando cambiamos de modo
            if (isFirstPerson)
            {
                cameraComponent.fieldOfView = 90f; // FOV de 90 en primera persona
            }
            else
            {
                cameraComponent.fieldOfView = 60f; // FOV de 60 en tercera persona
            }
        }

        // Si está en modo primera persona, controla la rotación de la cámara usando las teclas A, D, W, S
        if (isFirstPerson)
        {
            // Rotación hacia la izquierda (A)
            if (Input.GetKey(KeyCode.A))
            {
                rotationY -= rotationSpeed * Time.deltaTime;
            }
            // Rotación hacia la derecha (D)
            if (Input.GetKey(KeyCode.D))
            {
                rotationY += rotationSpeed * Time.deltaTime;
            }
            // Rotación hacia arriba (W)
            if (Input.GetKey(KeyCode.W))
            {
                rotationX -= rotationSpeed * Time.deltaTime;
            }
            // Rotación hacia abajo (S)
            if (Input.GetKey(KeyCode.S))
            {
                rotationX += rotationSpeed * Time.deltaTime;
            }

            // Crea una nueva rotación basada en los valores de rotationX y rotationY
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

            // Aplica la rotación a la cámara
            transform.rotation = rotation;
        }
    }

    // Este método se llama después de que se han hecho todos los cálculos de actualización, para ajustar la posición de la cámara
    void LateUpdate()
    {
        // Si está en modo primera persona, la cámara sigue al jugador pero a una altura específica
        if (isFirstPerson)
        {
            transform.position = player.transform.position + Vector3.up * firstPersonHeightOffset;
        }
        else
        {
            // Si no está en primera persona, la cámara mantiene su distancia original del jugador
            Vector3 desiredPosition = player.transform.position + offset;

            // Mueve la cámara a la posición deseada
            transform.position = desiredPosition;

            // Hace que la cámara mire al jugador
            transform.LookAt(player.transform.position);
        }
    }
}
