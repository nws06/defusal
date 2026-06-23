using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    public static readonly Dictionary<WireColour, Color> PossibleWireColours = new()
    {
        {WireColour.red, Color.red},
        {WireColour.blue, Color.blue},
        {WireColour.yellow, Color.yellow},
        {WireColour.purple, Color.purple},
        {WireColour.green, Color.green},
        {WireColour.black, Color.black},
        {WireColour.white, Color.white}
    };

    private static readonly int _minNumberOfWires = 3;
    private static readonly int _maxNumberOfWires = 5;

    [SerializeField] private WireController _wire;        // inspector 
    [SerializeField] private WireController[] _wires;   
    private int _numberOfWires;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("RESETTING ----------");

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            SpawnWires();
        }
    }



    void Awake()
    {
        WireController.WireCut += OnWireCut;
        SpawnWires();
    }

    void SpawnWires()
    {
        _numberOfWires = _minNumberOfWires;
        //_numberOfWires = Random.Range(_minNumberOfWires, _maxNumberOfWires + 1);
        _wires = new WireController[_numberOfWires];
        Vector3 location = new Vector3(0f, 4.2f, 0f);

        for (int i = 0; i < _numberOfWires; i++)
        {
            _wires[i] = Instantiate(_wire, location, Quaternion.identity, transform);
            _wires[i].Init(i + 1);

            location.y -= 2.4f;
        }
    }



    void OnWireCut(WireController wire)
    {
        switch (_numberOfWires)
        {
            case 3:
                CheckWireCut3Wires(wire);
                break;
            case 4:
            case 5:
                CheckWireCut4or5Wires(wire);
                break;
            default:
                Debug.LogError("WireManager: Invalid number of wires.");
                break;
        }
    }


    void CheckWireCut3Wires(WireController wire)
    {
        // If all three wires are the same colour, cut wire 1 
        if (_wires[0].Colour == _wires[1].Colour && _wires[0].Colour == _wires[2].Colour)
            CheckPosition(wire, 1);

        // If wire 1 is black, cut wire 1
        else if (_wires[0].Colour == WireColour.black)
            CheckPosition(wire, 1);

        // If wires 1 and 3 are the same colour, cut wire 2
        else if (_wires[0].Colour == _wires[2].Colour)
            CheckPosition(wire, 2);

        // If any two wires are the same colour, cut wire 3
        else if (_wires[0].Colour == _wires[1].Colour || _wires[0].Colour == _wires[2].Colour || _wires[1].Colour == _wires[2].Colour)
            CheckPosition(wire, 3);

        // If wire 2 is purple, cut wire 1
        else if (_wires[1].Colour == WireColour.purple)
            CheckPosition(wire, 1);

        // If there are no red wires, cut wire 2
        else if (_wires[0].Colour != WireColour.red && _wires[1].Colour != WireColour.red && _wires[2].Colour != WireColour.red)
            CheckPosition(wire, 2);
        
        // Otherwise, cut wire 3
        else 
            CheckPosition(wire, 3);
    }

    void CheckWireCut4or5Wires(WireController wire)
    {
        
    }



    void CheckPosition(WireController wire, int requiredPosition)
    {
        if (wire.Position == requiredPosition)
            print("Correct");
        else
            print("KABOOM!");
    }
}
