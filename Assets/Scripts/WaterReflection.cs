using UnityEngine;

public class WaterReflection : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject backgroundReflection;
    [SerializeField] private RenderTexture reflectionTexture;
    [SerializeField] private Camera reflectionCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBackgroundSize();
        SetBackgroundReflectionSize();
        SetReflectionTextureSize();
        SetCameraSize();
    }

    private void SetCameraSize()
    {
        Transform tf = reflectionCamera.transform;
        reflectionCamera.targetTexture = reflectionTexture;
        reflectionCamera.orthographicSize = backgroundReflection.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 cameraCenter = new Vector3((ScreenData.backgroundDimensions.left + ScreenData.backgroundDimensions.right) / 2f,
            (ScreenData.backgroundDimensions.bottom + reflectionCamera.orthographicSize * 2 + ScreenData.backgroundDimensions.bottom) / 2f, tf.position.z);
        tf.position = cameraCenter;
        
        reflectionCamera.enabled = false;
        reflectionCamera.enabled = true;
        reflectionCamera.Render();
    }

    private void SetReflectionTextureSize()
    {
        reflectionTexture.width = Screen.width;
        reflectionTexture.height = (int)(Screen.height * (ScreenData.seaDimensions.height / ScreenData.screenDimensions.height));
    }

    private void SetBackgroundReflectionSize()
    {
        var spriteRenderer = backgroundReflection.GetComponent<SpriteRenderer>();
        Transform tf = backgroundReflection.transform;
        
        Vector3 seaCenter = new Vector3((ScreenData.seaDimensions.left + ScreenData.seaDimensions.right) / 2f,
            (ScreenData.seaDimensions.top + ScreenData.seaDimensions.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(ScreenData.seaDimensions.width / spriteRenderer.bounds.size.x, ScreenData.seaDimensions.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }

    private void SetBackgroundSize()
    {
        var spriteRenderer = background.GetComponent<SpriteRenderer>();
        Transform tf = background.transform;

        float newScale = ScreenData.backgroundDimensions.width / spriteRenderer.bounds.size.x;
        //Vector3 newScale = new Vector3(ScreenData.backgroundDimensions.width / spriteRenderer.bounds.size.x, ScreenData.backgroundDimensions.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = new Vector3(newScale, newScale, tf.localScale.z);
        
        Vector3 backgroundCenter = new Vector3((ScreenData.backgroundDimensions.left + ScreenData.backgroundDimensions.right) / 2f,
            (ScreenData.backgroundDimensions.bottom + spriteRenderer.bounds.size.y + ScreenData.backgroundDimensions.bottom) / 2f, tf.position.z);
        tf.position = backgroundCenter;

        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
