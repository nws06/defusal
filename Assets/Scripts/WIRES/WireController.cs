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
    private int _wireType = 0;



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
        _wireType = Random.Range(0, WireSprites.Length);
        _spriteRenderer.sprite = WireSprites[_wireType];
    }

    void SetColour(Color colour)
    {
        Colour = colour;
        _spriteRenderer.color = colour;
    }



    // Cut the wire 
    public void OnPointerDown(PointerEventData eventData)
    {
        _spriteRenderer.sprite = CutWireSprites[_wireType];
        WireCut?.Invoke(this);
    }
}
