using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
   private Rigidbody rb; 
   private int count;
   private float movementX;
   private float movementY;
   private bool isGrounded;
   private bool isInvulnerable = false;

   public float speed = 15f; 
   public float jumpForce = 10f;
   public TextMeshProUGUI countText;
   public GameObject winTextObject;
   public float rampBoostMultiplier = 5f;

   private enum PlayerState { moving, jumping, invulnerable }
   private PlayerState currentState;

   public Material normalMaterial;   // Material normal
   public Material invincibleMaterial;

   private Renderer playerRenderer;
   private Animator animator;

   void Start()
   {
      rb = GetComponent<Rigidbody>();
      count = 0; 
      playerRenderer = GetComponent<Renderer>(); // Obtiene el renderer del objeto
      animator = GetComponent<Animator>(); // Obtiene el Animator
      playerRenderer.material = normalMaterial; // Establece el material inicial
      SetCountText();
      winTextObject.SetActive(false);
   }
 
   void OnMove(InputValue movementValue)
   {
      Vector2 movementVector = movementValue.Get<Vector2>();
      movementX = movementVector.x; 
      movementY = movementVector.y; 
   }

   void OnJump(InputValue jumpValue)
   {
      if (isGrounded)
      {
         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
         isGrounded = false;
      }
   }
   
   void SetCountText() 
   {
      countText.text = "Count: " + count.ToString();
      if (count >= 10)
      {
         winTextObject.SetActive(true);
         Destroy(GameObject.FindGameObjectWithTag("Enemy"));
      }
   }

   private void FixedUpdate() 
   {
      Vector3 movement = new Vector3(movementX, 0.0f, movementY);
      rb.AddForce(movement * speed);
   }

   void OnTriggerEnter(Collider other) 
   {
      if (other.gameObject.CompareTag("PickUp")) 
      {
         other.gameObject.SetActive(false);
         count++;

         // Verificar si ha recogido 5 objetos
         if (count % 5 == 0)
         {
            StartCoroutine(BecomeInvulnerable());
         }

         SetCountText();
      }
   }

   private IEnumerator BecomeInvulnerable()
   {
      isInvulnerable = true;
      currentState = PlayerState.invulnerable;
      playerRenderer.material = invincibleMaterial; // Cambia el material
      animator.SetBool("itemInvencible", true); // Activa animación de invulnerabilidad

      yield return new WaitForSeconds(3f);

      isInvulnerable = false;
      currentState = PlayerState.moving;
      playerRenderer.material = normalMaterial; // Vuelve al material normal
      animator.SetBool("itemInvencible", false); // Desactiva animación
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("Enemy"))
      {
         if (!isInvulnerable)
         {
            Destroy(gameObject); 
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
         }
      }
      else if (collision.gameObject.CompareTag("Ramp"))
      {
         Vector3 boost = new Vector3(movementX, 0.0f, movementY) * rampBoostMultiplier;
         rb.AddForce(boost, ForceMode.Impulse);
      }
      else if (collision.gameObject.CompareTag("Ground"))
      {
         isGrounded = true;
      }
   }
}
