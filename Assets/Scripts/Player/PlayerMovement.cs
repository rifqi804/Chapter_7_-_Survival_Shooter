using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Attribute")]
    public float speed = 6f;
    Vector3 movement;
    float camRayLength = 100f;

    [Header("References")]
    Animator animator;
    Rigidbody playerRB;
    int floorLayerMask;

    private void Awake()
    {
        //mendapatkan reference ke komponen animator
        animator = GetComponent<Animator>();
        //mendapatkan reference ke komponen rigidbody
        playerRB = GetComponent<Rigidbody>();
        //mendapatkan nilai mask dari layer Floor
        floorLayerMask = LayerMask.GetMask("Floor");

       
    }

    private void FixedUpdate()
    {
        //mendapatkan input horizontal dari player
        float horizontal = Input.GetAxisRaw("Horizontal");

        //mendapatkan input vertical dari player
        float vertical = Input.GetAxisRaw("Vertical");

        Move(horizontal, vertical);
        Turning();
        Animating(horizontal, vertical);
    }

    public void Move(float horizontal, float vertical)
    {
        //set arah x dan z variable movement
        movement.Set(horizontal, 0f, vertical);

        //normalize vector agar tidak berjalan lebih cepat kalau vertical dan horizontal dipencet bersamaan, lalu kalikan speed
        movement = movement.normalized * speed * Time.deltaTime;

        //pindah player ke posisi sesuai dengan perhitungan
        playerRB.MovePosition(transform.position + movement);
    }

    private void Turning()
    {
        //buat ray dari camera ke posisi mouse
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //menyimpan hit dari raycast
        RaycastHit floorHit;

        //lakukan raycast dan cek jika kena
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorLayerMask))
        {
            //hitung vector direction dari floor ke player
            Vector3 playerToMouseDir = floorHit.point - transform.position;
            playerToMouseDir.y = 0;

            //dapatkan rotation ke direction tersebut
            Quaternion newRotation = Quaternion.LookRotation(playerToMouseDir);

            //rotasikan player
            playerRB.MoveRotation(newRotation);
        }
    }

    public void Animating(float horizontal, float vertical)
    {
        //tentukan jika ada input dari player
        bool isWalking = horizontal != 0 || vertical != 0;
        animator.SetBool("IsWalking", isWalking);
    }
}
