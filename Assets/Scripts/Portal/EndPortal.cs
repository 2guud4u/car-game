using UnityEngine;

public class EndPortal : MonoBehaviour
{
    private Vector3 portalSize = new Vector3(10f, 20f, 1f);
    private bool _open = false;
    
    void Update()
    {
        if (GameManager.Instance._soul >= GameManager.Instance.soulCondition && !_open){
            transform.localScale = portalSize;
            _open = true;
            GameManager.Instance.PortalOpened();
        }
        if (GameManager.Instance._soul < GameManager.Instance.soulCondition){
            transform.localScale = new Vector3(0f, 0f, 0f);
            _open = false;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (_open && collision.gameObject.tag == "Player"){
            GameManager.Instance.GameWin();
        }
    }
}
