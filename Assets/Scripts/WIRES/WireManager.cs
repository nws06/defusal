using UnityEngine;

public class WireManager : MonoBehaviour
{
    private static readonly Color[] _possibleWireColours = new Color[]
    {
        Color.red, 
        Color.blue,
        Color.yellow,
        Color.green,
        Color.black,
        Color.white
    };

    private static readonly int _minNumberOfWires = 3;
    private static readonly int _maxNumberOfWires = 5;

    [SerializeField] private WireController _wirePrefab;        // inspector 
    [SerializeField] private WireController[] _existingWires;   
    [SerializeField] private int _correctPosition; 
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
        //_numberOfWires = 4;
        _numberOfWires = Random.Range(_minNumberOfWires, _maxNumberOfWires + 1);
        _existingWires = new WireController[_numberOfWires];
        Vector3 location = new Vector3(0f, 4.2f, 0f);
        Color colour = new Color();

        for (int i = 0; i < _numberOfWires; i++)
        {
            // Get a random colour
            colour = _possibleWireColours[Random.Range(0, _possibleWireColours.Length)];

            // Spawn new wire
            _existingWires[i] = Instantiate(_wirePrefab, location, Quaternion.identity, transform);

            // Initialize new wire with position and colour
            _existingWires[i].Init(i + 1, colour);

            // Move to next location 
            location.y -= 2.4f;
        }

        CalculateCorrectPosition();
    }

    void CalculateCorrectPosition()
    {
        if (_numberOfWires == 3)
        {
            // All 3 wires are the same, cut wire 1
            if (AreSameColour(_existingWires[0], _existingWires[1]) && AreSameColour(_existingWires[0], _existingWires[2]))
                _correctPosition = 1;
            // If wire 1 is black, cut wire 1
            else if (AreSameColour(_existingWires[0], Color.black))
                _correctPosition = 1;
            // If wires 1 and 3 are the same, cut wire 2
            else if (AreSameColour(_existingWires[0], _existingWires[2]))
                _correctPosition = 2;
            // If there are any white wires, cut wire 1
            else if (GroupContainsColour(_existingWires, Color.white))
                _correctPosition = 1;
            // If any two wires are the same, cut wire 3
            else if (AnyTwoSame(_existingWires))
                _correctPosition = 3;
            // If there are no red wires, cut wire 2
            else if (CountColour(_existingWires, Color.red) == 0)
                _correctPosition = 2;
            // Otherwise, cut wire 3
            else 
                _correctPosition = 3;
        }
        else 
        {
            // If wire 1 is black, cut wire 2
            if (AreSameColour(_existingWires[0], Color.black))
                _correctPosition = 2;
            // If there are more than 1 red wires, cut the last red wire
            else if (CountColour(_existingWires, Color.red) > 1)
                _correctPosition = LastIndexOfColour(_existingWires, Color.red) + 1;
            // If wires 1 and 4 are the same, cut wire 4
            else if (AreSameColour(_existingWires[0], _existingWires[3]))
                _correctPosition = 4;
            // If there are any yellow wires, cut wire 2
            else if (GroupContainsColour(_existingWires, Color.yellow))
                _correctPosition = 2;
            // If there is exactly 1 blue wire, cut wire 1
            else if (CountColour(_existingWires, Color.blue) == 1) 
                _correctPosition = 1;
            // If any two wires are the same, cut wire 3
            else if (AnyTwoSame(_existingWires))
                _correctPosition = 3;
            // Otherwise, cut wire 2 
            else 
                _correctPosition = 2;
        }
    }



    bool AreSameColour(WireController wire1, WireController wire2)
    {
        return wire1.Colour == wire2.Colour;
    }

    bool AreSameColour(WireController wire, Color colour)
    {
        return wire.Colour == colour;
    }

    bool GroupContainsColour(WireController[] wires, Color colour)
    {
        foreach (WireController wire in wires)
        {
            if (AreSameColour(wire, colour))
                return true;
        }

        return false;
    }

    bool AnyTwoSame(WireController[] wires)
    {
        for (int i = 0; i < wires.Length; i++)
        {
            for (int j = i + 1; j < wires.Length; j++)
            {
                if (AreSameColour(wires[i], wires[j]))
                    return true;
            }
        }

        return false;
    }

    int CountColour(WireController[] wires, Color colour)
    {
        int count = 0;

        foreach (WireController wire in wires)
        {
            if (AreSameColour(wire, colour))
                count++;
        }

        return count;
    }

    int LastIndexOfColour(WireController[] wires, Color colour)
    {
        for (int i = wires.Length - 1; i >= 0; i--)
        {
            if (AreSameColour(wires[i], colour))
                return i;
        }

        // Last index not found 
        return -1;
    }



    void OnWireCut(WireController wire)
    {
        if (wire.Position == _correctPosition)
            print("Correct");
        else 
            print("KABOOM!!!");
    }



    void OnDisable()
    {
        WireController.WireCut -= OnWireCut;
    }
}
