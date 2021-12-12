using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScanse : MonoBehaviour {
    public void OnCollisionEnter2D(Collision2D collision) {
        Debug.LogError(collision.gameObject.name);
        if (collision.gameObject.name == "Hannah") {

            string name = SceneManager.GetActiveScene().name;
            Debug.LogError(collision.gameObject.name + name);
            switch (name) {
                case "1-0":
                    SceneManager.LoadScene("1-1");
                    break;
                case "1-1":
                    SceneManager.LoadScene("1-2");
                    break;
            }
        }
    }
}
