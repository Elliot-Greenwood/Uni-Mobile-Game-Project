using UnityEngine;

public class ChestScript : MonoBehaviour
{
    Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Anim.SetTrigger("WinChest");
        }
    }


    

}
