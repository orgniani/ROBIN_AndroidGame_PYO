using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridActionManager : MonoBehaviour
{
    [SerializeField] private List<Transform> statBoxes;

    [SerializeField] private List<GameObject> options;

    [SerializeField] private List<GameObject> buttons;

    public void UpdateMenuPosition(int turn)
    {
        transform.position = statBoxes[turn - 1].position;
    }


}
