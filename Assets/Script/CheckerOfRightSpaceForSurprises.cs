using UnityEngine;

public class CheckerOfRightSpaceForSurprises : MonoBehaviour
{
    public static CheckerOfRightSpaceForSurprises Instance;

    //SET POSITION MIN AND MAX FOR PUT SUPRISE
    Vector3 _startPoint;
    Vector3 _endPoint;
    float _distanseSartPointEndPoint;
    float _theNumberOfPointsWhereSuprisesArePlaced = 12;
    float _unitDistance;
    float _positionSurpriseAfterSet;

    private void Awake()
    {

        if (CheckerOfRightSpaceForSurprises.Instance == null)
        {
            CheckerOfRightSpaceForSurprises.Instance = this;
        }

    }
    void Start()
    {
        _startPoint = new Vector3(0, 6.6f, 0);
        _endPoint = new Vector3(0, -5f, 0);

        _distanseSartPointEndPoint = Vector3.Distance(_startPoint, _endPoint);
        _unitDistance = _distanseSartPointEndPoint / _theNumberOfPointsWhereSuprisesArePlaced;

    }

    public void MoveSurpriseManager(int startPosition, int endPosition)
    {
        float random_for_position_pakage = Random.Range(startPosition, endPosition) * _unitDistance;

        switch (SupriseManeger.Instance.Position_X_ToMakeSurprises)
        {
            case -1:
                _positionSurpriseAfterSet = -1.75f;
                break;
            case 0:
                _positionSurpriseAfterSet = 0f;
                break;
            case 1:
                _positionSurpriseAfterSet = 1.75f;
                break;


        }

        transform.position = new Vector2(_positionSurpriseAfterSet, _startPoint.y - random_for_position_pakage);

    }

}

