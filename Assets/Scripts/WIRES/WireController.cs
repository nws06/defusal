using UnityEngine;
using UnityEngine.EventSystems;

public class WireController : MonoBehaviour, IPointerDownHandler
{
    public static event System.Action<WireController> WireCut;

    public static Sprite[] WireSprites;
    public static Sprite[] CutWireSprites;

    [field: SerializeField] public int Position { get; private set; }
    [field: SerializeField] public WireColour Colour { get; private set; }

    private static readonly int _numberOfColours = 7;

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



    // Initialize the wire at a given position in the sequence (1-5)
    public void Init(int position)
    {
        Position = position;
        RandomizeSprite();
        RandomizeColour();
    }

    void RandomizeSprite()
    {
        _wireType = Random.Range(0, WireSprites.Length);
        _spriteRenderer.sprite = WireSprites[_wireType];
    }

    void RandomizeColour()
    {
        Colour = (WireColour) Random.Range(0, WireManager.PossibleWireColours.Count);
        _spriteRenderer.color = WireManager.PossibleWireColours[Colour];
    }



    // Cut the wire 
    public void OnPointerDown(PointerEventData eventData)
    {
        _spriteRenderer.sprite = CutWireSprites[_wireType];
        WireCut?.Invoke(this);
    }
}
