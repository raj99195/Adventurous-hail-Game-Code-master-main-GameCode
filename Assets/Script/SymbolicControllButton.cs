using UnityEngine;

/// <summary>
/// Set symbolic buttons positon responsive
/// </summary>
public class SymbolicControllButton : MonoBehaviour
{
    [SerializeField] GameObject _symbolicBackground_R, _symbolicBackground_L;

    [SerializeField] float _distanceFromSide;
    [SerializeField] float _maxDistanceFromCenter;

    void Start()
    {
        setBackgroundsPos();
    }

    void setBackgroundsPos()
    {
        float rightOfScreen = Camera.main.aspect * Camera.main.orthographicSize;
        
        float x = rightOfScreen - _distanceFromSide;

        if (x > _maxDistanceFromCenter)
           x = _maxDistanceFromCenter;

        _symbolicBackground_R.transform.position = new Vector3(x, _symbolicBackground_R.transform.position.y, 0);
        _symbolicBackground_L.transform.position = new Vector3(-x, _symbolicBackground_R.transform.position.y, 0);
    }

}
