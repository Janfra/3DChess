using UnityEngine;

[CreateAssetMenu(fileName = "Board Colours", menuName = "ScriptableObjects/BoardColours")]
public class SO_Colours : ScriptableObject
{
    [SerializeField] public Color colourOne;
    [SerializeField] public Color colourTwo;
    public Color colourHighlightOne;
    public Color colourHighlightTwo;
}
