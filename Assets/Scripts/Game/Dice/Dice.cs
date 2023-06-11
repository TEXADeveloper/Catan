using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private DiceManager dm;
    [SerializeField] private byte id;
    Vector3 startPos = Vector3.zero;
    private int num = 0;

    public void SetNum(int value)
    {
        num = value;
    }

    void Start()
    {
        startPos = transform.position;
    }

    public void ThrowDice()
    {
        transform.position = startPos;
        rb.useGravity = true;
        rb.AddForce(Vector3.up * Random.Range(-25f, 0f), ForceMode.Impulse);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        rb.angularVelocity = new Vector3(x, y, z).normalized * Random.Range(7f, 10f);
    }

    void Update()
    {
        if (rb.velocity == Vector3.zero && num != 0)
        {
            dm.SetDiceValue(num, id);
            num = 0;
            rb.useGravity = false;
        }
    }
}
