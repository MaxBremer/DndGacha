using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrow : MonoBehaviour
{
    public bool isLeft;

    private BaseInfoPanel myParentPanel;

    // Start is called before the first frame update
    void Start()
    {
        myParentPanel = GetComponentInParent<BaseInfoPanel>();
    }

    private void OnMouseDown()
    {
        myParentPanel.AbilChange(isLeft);
    }
}
