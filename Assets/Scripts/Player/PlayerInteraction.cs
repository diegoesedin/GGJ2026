using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public int FollowerNumber;
    public MaskType CurrentMaskType;
    [SerializeField] private SpriteRenderer _renderer;

    public void CalculateEncounter(EnemyGroup enemyGroup)
    {
        if (IsConversion(CurrentMaskType, enemyGroup.CurrentMaskType))
        {
            FollowerNumber += enemyGroup.FollowerNumber;
        }
        else FollowerNumber -= enemyGroup.FollowerNumber;
        Destroy(enemyGroup.gameObject);
    }

    private bool IsConversion(MaskType playerMask, MaskType enemyMask)
    {
        return ((int)playerMask + 1) % 4 == (int)enemyMask;
    }

    public void ChangeMask()
    {
        CurrentMaskType = (MaskType)(((int)CurrentMaskType + 1) % 4);
        switch (CurrentMaskType)
        {
            case MaskType.Red:
                _renderer.color = Color.red;
              break;
            case MaskType.Green:
                _renderer.color = Color.green;
              break;
            case MaskType.Blue:
                _renderer.color = Color.blue;
                break;
            case MaskType.Yellow:
                _renderer.color = Color.yellow;
                break;

        }
    }
}


public enum MaskType
{
    Red,
    Green,
    Blue,
    Yellow,
}