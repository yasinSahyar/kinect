using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Transformu her frame'de x, y ve z eksenlerinde 15, 30 ve 45 derece hızla döndürür.
        // Time.deltaTime, dönüşün frame hızından bağımsız, zamana bağlı ve pürüzsüz olmasını sağlar.
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
