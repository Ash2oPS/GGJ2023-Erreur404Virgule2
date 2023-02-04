using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Indicators : MonoBehaviour
{
    [SerializeField] private Transform _colIndic, _colIndicToRotate, _dirIndic, _dirIndicCenter;

    [SerializeField] private bool _clockWise;

    private void Update()
    {
        if (_clockWise)
        {
            _colIndicToRotate.eulerAngles += new Vector3(0, 0, 0.1f);
            return;
        }

        _colIndicToRotate.eulerAngles -= new Vector3(0, 0, 0.1f);
    }

    public void SetColliderIndicatorRadius(float radius)
    {
        _colIndic.localScale = Vector3.one * radius;
    }

    public void SetOrientation(float angle)
    {
        _dirIndicCenter.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
}