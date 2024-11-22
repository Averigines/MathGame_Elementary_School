using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject waterDistortion;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject backgroundReflection;
    [SerializeField] private RenderTexture reflectionTexture;
    [SerializeField] private Camera reflectionCamera;
    void Start()
    {
        SetWaterSize();
        SetWaterDistortionSize();
        SetPlayerPosition();
        SetBackgroundSize();
        SetBackgroundReflectionSize();
        SetReflectionTextureSize();
        SetCameraSize();
    }

    private void SetWaterSize()
    {
        var spriteRenderer = water.GetComponent<SpriteRenderer>();
        Transform tf = water.transform;
        
        Vector3 seaCenter = new Vector3((ScreenData.seaArea.left + ScreenData.seaArea.right) / 2f,
            (ScreenData.seaArea.top + ScreenData.seaArea.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(ScreenData.seaArea.width / spriteRenderer.bounds.size.x, ScreenData.seaArea.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }
    
    private void SetWaterDistortionSize()
    {
        var spriteRenderer = waterDistortion.GetComponent<SpriteRenderer>();
        Transform tf = waterDistortion.transform;
        
        Vector3 seaCenter = new Vector3((ScreenData.seaArea.left + ScreenData.seaArea.right) / 2f,
            (ScreenData.seaArea.top + ScreenData.seaArea.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(ScreenData.seaArea.width / spriteRenderer.bounds.size.x, ScreenData.seaArea.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }

    private void SetPlayerPosition()
    {
        Transform tf = player.transform;
        float posX = (ScreenData.seaArea.left + ScreenData.seaArea.right) / 2f;
        float posY = ScreenData.seaArea.top - 0.35f;
        tf.position = new Vector3(posX, posY, tf.position.z);
    }
    
    private void SetBackgroundSize()
    {
        var spriteRenderer = background.GetComponent<SpriteRenderer>();
        Transform tf = background.transform;

        float newScale = ScreenData.backgroundArea.width / spriteRenderer.bounds.size.x;
        //Vector3 newScale = new Vector3(ScreenData.backgroundDimensions.width / spriteRenderer.bounds.size.x, ScreenData.backgroundDimensions.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = new Vector3(newScale, newScale, tf.localScale.z);
        
        Vector3 backgroundCenter = new Vector3((ScreenData.backgroundArea.left + ScreenData.backgroundArea.right) / 2f,
            (ScreenData.backgroundArea.bottom + spriteRenderer.bounds.size.y + ScreenData.backgroundArea.bottom) / 2f, tf.position.z);
        tf.position = backgroundCenter;
    }
    
    private void SetBackgroundReflectionSize()
    {
        var spriteRenderer = backgroundReflection.GetComponent<SpriteRenderer>();
        Transform tf = backgroundReflection.transform;
        
        Vector3 seaCenter = new Vector3((ScreenData.seaArea.left + ScreenData.seaArea.right) / 2f,
            (ScreenData.seaArea.top + ScreenData.seaArea.bottom) / 2f, tf.position.z);
        tf.position = seaCenter;
        
        Vector3 newScale = new Vector3(ScreenData.seaArea.width / spriteRenderer.bounds.size.x, ScreenData.seaArea.height / spriteRenderer.bounds.size.y, tf.localScale.z);
        tf.localScale = newScale;
    }
    
    private void SetReflectionTextureSize()
    {
        reflectionTexture.width = Screen.width;
        reflectionTexture.height = (int)(Screen.height * (ScreenData.seaArea.height / ScreenData.screenArea.height));
    }
    
    private void SetCameraSize()
    {
        Transform tf = reflectionCamera.transform;
        reflectionCamera.targetTexture = reflectionTexture;
        reflectionCamera.orthographicSize = backgroundReflection.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 cameraCenter = new Vector3((ScreenData.backgroundArea.left + ScreenData.backgroundArea.right) / 2f,
            (ScreenData.backgroundArea.bottom + reflectionCamera.orthographicSize * 2 + ScreenData.backgroundArea.bottom) / 2f, tf.position.z);
        tf.position = cameraCenter;
        
        reflectionCamera.enabled = false;
        reflectionCamera.enabled = true;
        reflectionCamera.Render();
    }
}
