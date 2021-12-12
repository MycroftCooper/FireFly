using UnityEngine;

public class TotemController : MonoBehaviour {
    HannahStatusController hsc;
    public Animator animator;
    public PhotoAbleEntityController paec;
    ParticleSystem particle;
    TransfomTools tools;
    private bool isInTrigger;
    public bool IsActiving {
        get {
            return animator.GetBool("IsActiving");
        }
        set {
            if (IsActiving != value)
                animator.SetBool("IsActiving", value);
        }
    }

    void Start() {
        hsc = GameObject.Find("Hannah").GetComponent<HannahStatusController>();
        particle = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        tools = GetComponent<TransfomTools>();
        IsActiving = false;
        isInTrigger = false;
        paec = PhotoAbleEntityController.GetInstance;
    }

    void Update() {
        if (!isInTrigger)
            return;
        playerInput();
    }

    public void LoadScane() {
        if (paec.CanPotoGODict.Count == 0)
            return;
        foreach (var item in paec.CanPotoGODict) {
            tools.moveToPositionByCoroutine(item.Key.transform, item.Value.OriginPosition, 5, 8, null);
            item.Key.transform.rotation = Quaternion.Euler(item.Value.OriginRoation.x, item.Value.OriginRoation.y, item.Value.OriginRoation.z);
            item.Key.transform.localScale = item.Value.OriginScale;
        }
        hsc.HP = hsc.HPUpper;
        hsc.MP = hsc.MPUpper;
    }

    void restoreCharacter() {
        hsc.HP = hsc.HPUpper;
        hsc.MP = hsc.MPUpper;
        hsc.MotionState = HannahStatusController.MotionStates.Dying;
    }

    void playerInput() {
        if (Input.GetKeyDown(KeyCode.R)) {
            IsActiving = true;
            restoreCharacter();
            particle.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name != "Hannah") return;
        isInTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name != "Hannah") return;
        isInTrigger = false;
    }
}
