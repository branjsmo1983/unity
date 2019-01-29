using UnityEngine;

public abstract class  GenericHammerLike : MonoBehaviour
{

    [SerializeField]
    protected Rigidbody myRigidbody;

    protected Vector3 upPosition;

    protected float startTime;

    protected float riseSpeed;
    protected float waitingTime;

    protected float currentWaitingTime;

    protected bool isWaiting;

    protected Vector3 startPosition;

    protected bool canFall;

    protected virtual void Awake () {
        upPosition = transform.position;
        MyEventManager.instance.AddListener (MyIndexEvent.initializeScene , OnInitializeScene);
    }

    protected virtual void Update () {
        if (isWaiting) {
            WaitingForRise ();
        } else {
            RisingUp ();
        }
    }

    void OnDestroy () {
        if (MyEventManager.instance != null) {
            MyEventManager.instance.RemoveListener (MyIndexEvent.initializeScene , OnInitializeScene);
        }
    }

    public virtual void OnInitializeScene (MyEventArgs e) {
        InitializeMe (e.myLevelData.WaitingTime_HammerLike , e.myLevelData.RiseTime_HammerLike);
    }

    public virtual void InitializeMe (float waitingTime , float riseTime) {
        riseSpeed = 1 / riseTime;
        this.waitingTime = waitingTime;
    }

    public virtual void ResetMe () {
        transform.position = upPosition;
        FallDown ();
    }

    protected virtual void RisingUp () {
        transform.position = Vector3.Lerp (startPosition , upPosition , (Time.time - startTime) * riseSpeed);
        if (transform.position.Equals (upPosition)) {
            if (canFall) {
                FallDown ();
            }
        }
    }

    protected virtual void WaitingForRise () {
        currentWaitingTime -= Time.deltaTime;
        if (currentWaitingTime <= 0) {
            RiseUp ();
        }
    }

    protected virtual void FallDown () {
        isWaiting = true;
        currentWaitingTime = waitingTime;
        myRigidbody.isKinematic = false;
        myRigidbody.AddForce (Physics.gravity * 4f , ForceMode.Impulse);
    }

    protected virtual void RiseUp () {
        myRigidbody.isKinematic = true;
        isWaiting = false;
        startTime = Time.time;
        startPosition = transform.position;
    }

    void OnCollisionEnter (Collision collision) {
        if (isWaiting && myRigidbody.velocity.y != 0) {
            if (collision.gameObject.CompareTag ("Player")) {
                if (collision.contacts[0].point.y > collision.gameObject.transform.position.y) {
                    MyEventManager.instance.CastEvent (MyIndexEvent.defeat , new MyEventArgs ());
                }
            }
        }
    }
}
