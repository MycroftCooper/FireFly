using GameKit;
using UnityEngine;

public class HannahCatchFireFlies : MonoBehaviour {
    public int firefliesNum;
    public SpritesManager SM;

    void Start() {
        firefliesNum = 0;
        if (SM == null)
            SM = GameObject.Find("SpritesManager").GetComponent<SpritesManager>();
    }


    public void OnTriggerEnter2D(Collider2D collision) {
        Sprites firefly = collision.gameObject.GetComponent<Sprites>();
        if (firefly == null) return;
        if (SM.IsMySprite(firefly)) {
            SM.Capture(firefly);
            firefliesNum++;
        }
        firefliesNum++;
        openGameCam();
    }

    private void openGameCam() {
        if (firefliesNum == 1) {
            EventCenter.instance.EventTrigger("Activate Rotate");
        }
        if (firefliesNum == 2) {
            EventCenter.instance.EventTrigger("Activate Move");
        }
    }
}
