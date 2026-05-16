using UnityEngine;

public class BoundaryCheck: MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Garbage")) 
        {
            Destroy(collision.gameObject);
            Debug.Log("Sampah keluar layar dan dihapus");
        }
    }
}
