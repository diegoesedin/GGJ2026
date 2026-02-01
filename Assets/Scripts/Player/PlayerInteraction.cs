using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour, IMaskHolder
{
    public int FollowerNumber;
    public MaskType CurrentMaskType;
    [SerializeField] private SpriteRenderer _renderer;
<<<<<<< Updated upstream
=======
    
    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController blueController;
    [SerializeField] private RuntimeAnimatorController greenController;
    [SerializeField] private RuntimeAnimatorController redController;
    [SerializeField] private RuntimeAnimatorController yellowController;

    [SerializeField] private Animator _maskUI;

>>>>>>> Stashed changes
    private List<PersonView> _people = new List<PersonView>();

    public MaskType MaskType => CurrentMaskType;

    public void CalculateEncounter(PersonView person)
    {
        if (person.IsMaskless || IsConversion(CurrentMaskType, person.CurrentMaskType))
        {
            AddFollower(person);
            person.SpriteRenderer.color = MaskColor.GetMaskColor(CurrentMaskType);
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
        _renderer.color = MaskColor.GetMaskColor(CurrentMaskType);
        foreach (var person in _people)
        {
            //person.CurrentMaskType = CurrentMaskType;
            person.GetComponent<SpriteRenderer>().color = MaskColor.GetMaskColor(CurrentMaskType);
        }


        
    }

<<<<<<< Updated upstream
=======
    private void UpdatePlayerAnimator()
    {
        switch (CurrentMaskType)
        {
            case MaskType.Blue:
                _animator.runtimeAnimatorController = blueController;
                _maskUI.Play("blue");
                break;
            case MaskType.Green:
                _animator.runtimeAnimatorController = greenController;
                //_maskUI.Play("green");
                break;
            case MaskType.Red:
                _animator.runtimeAnimatorController = redController;
                _maskUI.Play("red");
                break;
            case MaskType.Yellow:
                _animator.runtimeAnimatorController = yellowController;
                _maskUI.Play("yellow");
                break;
        }
    }
>>>>>>> Stashed changes
    
}


public enum MaskType
{
    Red,
    Green,
    Blue,
    Yellow,
}