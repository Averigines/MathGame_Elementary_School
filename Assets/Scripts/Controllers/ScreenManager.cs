using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private float seaHeight = 0.7f;
    [SerializeField] private float fishSpaceToWaterEdgeX = 1f;
    [SerializeField] private float fishSpaceToWaterEdgeY = 1f;
    
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject waterDistortion;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject backgroundReflection;
    [SerializeField] private RenderTexture reflectionTexture;
    [SerializeField] private Camera reflectionCamera;
    
    public struct ScreenArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static ScreenArea screenArea;
    
    public struct SeaArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static SeaArea seaArea;
    
    public struct BackgroundArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    public static BackgroundArea backgroundArea;
    
    private struct FishArea
    {
        public float bottom;
        public float top;
        public float left;
        public float right;
        public float height;
        public float width;
    }
    private static FishArea fishArea;

    private Camera _mainCamera;
    
    void Awake()
    {
        _mainCamera = Camera.main;
        SetScreenAreas();
        SetObjectDimensions();
    }

    private void SetScreenAreas()
    {
        CalculateScreenArea();
        CalculateSeaArea();
        CalculateBackgroundArea();
        CalculateFishArea();
    }
    
    private void SetObjectDimensions()
    {
        SetWaterSize();
        SetWaterDistortionSize();
        SetPlayerPosition();
        SetBackgroundSize();
        SetBackgroundReflectionSize();
        SetReflectionTextureSize();
        SetCameraSize();
    }
    
    private void CalculateScreenArea()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));
        
        screenArea.bottom = bottomLeft.y;
        screenArea.top = topRight.y;
        screenArea.left = bottomLeft.x;
        screenArea.right = topRight.x;
        screenArea.height = screenArea.top - screenArea.bottom;
        screenArea.width = screenArea.right - screenArea.left;
    }
    
    private void CalculateSeaArea()
    {
        seaArea.bottom = screenArea.bottom;
        seaArea.top = screenArea.bottom + seaHeight * screenArea.height;
        seaArea.left = screenArea.left;
        seaArea.right = screenArea.right;
        seaArea.height = seaArea.top - seaArea.bottom;
        seaArea.width = seaArea.right - seaArea.left;
    }

    private void CalculateBackgroundArea()
    {
        backgroundArea.left = screenArea.left;
        backgroundArea.right = screenArea.right;
        backgroundArea.width = backgroundArea.right - backgroundArea.left;
        
        backgroundArea.bottom = seaArea.top;
        backgroundArea.top = screenArea.top;
        backgroundArea.height = backgroundArea.top - backgroundArea.bottom;
        
    }

    private void CalculateFishArea()
    {
        fishArea.bottom = seaArea.bottom + fishSpaceToWaterEdgeY;
        fishArea.top = seaArea.top - fishSpaceToWaterEdgeY;
        fishArea.left = seaArea.left + fishSpaceToWaterEdgeX;
        fishArea.right = seaArea.right - fishSpaceToWaterEdgeX;
        fishArea.height = fishArea.top - fishArea.bottom;
        fishArea.width = fishArea.right - fishArea.left;
    }

    private void SetWaterSize()
    {
        var spriteRenderer = water.GetComponent<SpriteRenderer>();
        Transform tf = water.transform;
        
        Vector3 seaCenter = new Vector3((seaArea.left + seaArea.right) / 2f,
            (seaArea.top + seaArea.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(seaArea.width / spriteRenderer.bounds.size.x * 1.05f, seaArea.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }
    
    private void SetWaterDistortionSize()
    {
        var spriteRenderer = waterDistortion.GetComponent<SpriteRenderer>();
        Transform tf = waterDistortion.transform;
        
        Vector3 seaCenter = new Vector3((seaArea.left + seaArea.right) / 2f,
            (seaArea.top + seaArea.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(seaArea.width / spriteRenderer.bounds.size.x * 1.05f, seaArea.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }

    private void SetPlayerPosition()
    {
        Transform tf = player.transform;
        float posX = (seaArea.left + seaArea.right) / 2f;
        float posY = seaArea.top - 0.26f;
        tf.position = new Vector3(posX, posY, tf.position.z);
        Debug.Log("Parent: " + tf.position);
        Debug.Log("Child: " + tf.GetComponentInChildren<SpriteRenderer>().transform.localPosition);
    }
    
    private void SetBackgroundSize()
    {
        var spriteRenderer = background.GetComponent<SpriteRenderer>();
        Transform tf = background.transform;

        var aspectRatioSprite = spriteRenderer.bounds.size.x / spriteRenderer.bounds.size.y;
        var aspectRatioBGArea = backgroundArea.width / backgroundArea.height;

        float newScale = 1f;
        if (aspectRatioSprite > aspectRatioBGArea)
        {
            newScale = backgroundArea.height / spriteRenderer.bounds.size.y;
        }
        else
        {
            newScale = backgroundArea.width / spriteRenderer.bounds.size.x;
        }
        
        tf.localScale = new Vector3(newScale, newScale, tf.localScale.z);

        Vector3 backgroundCenter = new Vector3((backgroundArea.left + backgroundArea.right) / 2f,
            (backgroundArea.bottom + spriteRenderer.bounds.size.y + backgroundArea.bottom) / 2f, tf.position.z);
        tf.position = backgroundCenter;
    }
    
    private void SetBackgroundReflectionSize()
    {
        var spriteRenderer = backgroundReflection.GetComponent<SpriteRenderer>();
        Transform tf = backgroundReflection.transform;
        
        Vector3 seaCenter = new Vector3((seaArea.left + seaArea.right) / 2f,
            (seaArea.top + seaArea.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(seaArea.width / spriteRenderer.bounds.size.x * 1.05f, seaArea.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }
    
    private void SetReflectionTextureSize()
    {
        var bgRenderer = background.GetComponent<SpriteRenderer>();
        
        Vector3 bottomleft = _mainCamera.WorldToScreenPoint(bgRenderer.bounds.min);
        Vector3 topright = _mainCamera.WorldToScreenPoint(bgRenderer.bounds.max);

        reflectionTexture.width = (int)topright.x - (int)bottomleft.x;
        reflectionTexture.height = (int)topright.y - (int)bottomleft.y;
    }
    
    private void SetCameraSize()
    {
        Transform tf = reflectionCamera.transform;
        reflectionCamera.targetTexture = reflectionTexture;
        reflectionCamera.orthographicSize = background.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 cameraCenter = new Vector3((backgroundArea.left + backgroundArea.right) / 2f,
            (backgroundArea.bottom + reflectionCamera.orthographicSize * 2 + backgroundArea.bottom) / 2f, tf.position.z);
        tf.position = cameraCenter;
        
        reflectionCamera.enabled = false;
        reflectionCamera.enabled = true;
        reflectionCamera.Render();
    }

    public static Vector2 GetFishAreaX(float left, float right)
    {
        Vector2 fishAreaX = new Vector2(fishArea.left + left * (fishArea.right - fishArea.left),
            fishArea.left + right * (fishArea.right - fishArea.left));

        return fishAreaX;
    }
    
    public static Vector2 GetFishSpawnPos(float posY, Vector2 fishAreaX)
    {
        float spawnPositionX = Utils.GetRandomNumber(fishAreaX.x, fishAreaX.y);
        float spawnPositionY = fishArea.top + posY * (fishArea.bottom - fishArea.top);
        Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);

        return spawnPosition;
    }

    public static bool CheckIfFishClusterNeedsToTurn(DirectionX currDirectionX, Vector3 pos, Vector2 fishAreaX)
    {
        if (pos.x < fishAreaX.x && currDirectionX == DirectionX.Left) return true;
        if (pos.x > fishAreaX.y && currDirectionX == DirectionX.Right) return true;
        return false;
    }
}
