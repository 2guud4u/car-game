using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;

    Vector3 _ySpin = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        _ySpin *= speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_ySpin * Time.deltaTime);
    }
}
