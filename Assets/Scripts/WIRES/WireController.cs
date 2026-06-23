using UnityEngine;
using UnityEngine.EventSystems;

public class WireController : MonoBehaviour, IPointerDownHandler
{
    public static event System.Action<WireController> WireCut;

    public static Sprite[] WireSprites;
    public static Sprite[] CutWireSprites;

    [field: SerializeField] public int Position { get; private set; }
    [field: SerializeField] public Color Colour { get; private set; }

    private SpriteRenderer _spriteRenderer;
    /// <summary>
    /// The type of wire sprite, used to match up with the correct cut wire sprite
    /// </summary>
    private int _spriteType = 0;



    void Awake()
    {
        // Gather components and sprites
        if (_spriteRenderer == null)
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        WireSprites ??= Resources.LoadAll<Sprite>("Wires/wires");
        CutWireSprites ??= Resources.LoadAll<Sprite>("Wires/cut wires");
    }



    public void Init(int position, Color colour)
    {
        Position = position; 
        RandomizeSprite();
        SetColour(colour);
    }

    void RandomizeSprite()
    {
        _spriteType = Random.Range(0, WireSprites.Length);
        _spriteRenderer.sprite = WireSprites[_spriteType];

        // The last sprite type cannot be in the fifth position
        if (Position == 5 && _spriteType == WireSprites.Length - 1)
            RandomizeSprite();
    }

    void SetColour(Color colour)
    {
        Colour = colour;
        _spriteRenderer.color = colour;
    }



    // Cut the wire 
    public void OnPointerDown(PointerEventData eventData)
    {
        _spriteRenderer.sprite = CutWireSprites[_spriteType];
        WireCut?.Invoke(this);
    }
}
