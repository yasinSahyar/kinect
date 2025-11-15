using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().CollectItem();
            Destroy(gameObject);
        }
    }
}
