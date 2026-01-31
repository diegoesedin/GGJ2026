using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public int FollowerNumber;
    public MaskType CurrentMaskType;
    [SerializeField] private SpriteRenderer _renderer;
    private List<PersonView> _people = new List<PersonView>();

    public void CalculateEncounter(PersonView person)
    {
        if (person.IsMaskless || IsConversion(CurrentMaskType, person.MaskType))
        {
            AddFollower(person);
            person.SpriteRenderer.color = MaskColor(CurrentMaskType);
        }
        else
        {
            DestroyFollower();
            Destroy(person.gameObject);
        }
    }

    private void AddFollower(PersonView person)
    {
        _people.Add(person);
    }

    private void DestroyFollower()
    {
        if (_people.Count == 0)
        {
            LoseGame();
            return;
        }
        PersonView follower = _people[_people.Count - 1];
        _people.RemoveAt(_people.Count - 1);
        Destroy(follower.gameObject);
    }

    private void LoseGame()
    {
        FindAnyObjectByType<GameOverMenu>( FindObjectsInactive.Include).gameObject.SetActive(true);
        Destroy(gameObject);
        Debug.Log("Game Over");
    }

    private bool IsConversion(MaskType playerMask, MaskType enemyMask)
    {
        return ((int)playerMask + 1) % 4 == (int)enemyMask;
    }

    public void ChangeMask()
    {
        CurrentMaskType = (MaskType)(((int)CurrentMaskType + 1) % 4);
        _renderer.color = MaskColor(CurrentMaskType);
        foreach (var person in _people)
        {
            //person.MaskType = CurrentMaskType;
            person.GetComponent<SpriteRenderer>().color = MaskColor(CurrentMaskType);
        }
    }

    private Color MaskColor(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red:
                return Color.red;
            case MaskType.Green:
                return Color.green;
            case MaskType.Blue:
                return Color.blue;
            case MaskType.Yellow:
                return Color.yellow;
            default:
                return Color.white;
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