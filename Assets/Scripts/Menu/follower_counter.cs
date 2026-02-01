using UnityEngine;
using TMPro;

public class FollowerCountUI : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _player;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = "- " + _player.People.Count.ToString() + " -";
    }
}