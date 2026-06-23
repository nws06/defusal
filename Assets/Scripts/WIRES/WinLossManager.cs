using System.Collections;
using UnityEngine;

public class WinLossManager : MonoBehaviour
{
    [SerializeField] private WireManager _wireManager;  // inspector 
    [SerializeField] private Canvas _winScreen;         // inspector
    [SerializeField] private Canvas _loseScreen;        // inspector



    void Awake()
    {
        _winScreen.enabled = false;
        _loseScreen.enabled = false;

        WireManager.CorrectWireCut += OnCorrectWire;
        WireManager.IncorrectWireCut += OnIncorrectWire;
    }



    void OnCorrectWire()
    {
        StartCoroutine(WinScreen());
    }

    void OnIncorrectWire()
    {
        StartCoroutine(LoseScreen());
    }



    IEnumerator WinScreen()
    {
        _winScreen.enabled = true;
        yield return new WaitForSeconds(3f);
        _winScreen.enabled = false;

        _wireManager.StartNewLevel();
    }

    IEnumerator LoseScreen()
    {
        _loseScreen.enabled = true;
        yield return new WaitForSeconds(3f);
        _loseScreen.enabled = false;

        _wireManager.StartNewLevel();
    }



    void OnDisable()
    {
        WireManager.CorrectWireCut -= OnCorrectWire;
        WireManager.IncorrectWireCut -= OnIncorrectWire;
    }
}
