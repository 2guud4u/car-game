using UnityEngine;

public class EndPortal : MonoBehaviour
{
    private Vector3 portalSize = new Vector3(10f, 20f, 1f);
    private bool _open = false;
    private int soulCondition = 1;
    
    void Update()
    {
        if (GameManager.Instance._soul >= soulCondition){
            transform.localScale = portalSize;
            _open = true;
        }
        if (GameManager.Instance._soul < soulCondition){
            transform.localScale = new Vector3(0f, 0f, 0f);
            _open = false;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (_open){
            GameManager.Instance.GameOver();
        }
    }
}
