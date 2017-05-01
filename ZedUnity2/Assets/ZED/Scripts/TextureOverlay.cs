using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class TextureOverlay : MonoBehaviour
{
    /// <summary>
    /// The screen is the quad where the textures are displayed
    /// </summary>
    public GameObject canvas;
    /// <summary>
    /// It's the main material, used to set the color and the depth
    /// </summary>
    private Material matRGB;

    /// <summary>
    /// All the textures displayed are in 16/9
    /// </summary>
    private float aspect = 16.0f/9.0f;

    /// <summary>
    /// The main camera is the camera controlled by the ZED
    /// </summary>
    private Camera mainCamera;



    //The zed perspective, depends on whether this script applies to
    //the left camera or right
    private Texture2D camZed;

    //Left camera = 0, right camera = 1
    public int whichCamera = 0;



    Texture2D depthXYZZed;
    CommandBuffer buffer;

    //public RenderTexture mask;
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.aspect = aspect;
    }


    void Start()
    {
        //Set textures to the shader
        matRGB = canvas.GetComponent<Renderer>().material;
        sl.ZEDCamera zedCamera = sl.ZEDCamera.GetInstance();

        //Create two textures and fill them with the ZED computed images
        if (whichCamera == 0){
          camZed = zedCamera.CreateTextureImageType(sl.VIEW.LEFT);
        }
        else if (whichCamera == 1){
          camZed = zedCamera.CreateTextureImageType(sl.VIEW.RIGHT);
        }
        depthXYZZed = zedCamera.CreateTextureMeasureType(sl.MEASURE.XYZ);
        matRGB.SetTexture("_CameraTex", camZed);
        matRGB.SetTexture("_DepthXYZTex", depthXYZZed);

        if (zedCamera.CameraIsReady)
        {
            mainCamera.fieldOfView = zedCamera.GetFOV() * Mathf.Rad2Deg;
            mainCamera.projectionMatrix = zedCamera.Projection;

            scale(canvas.gameObject, GetFOVFromProjectionMatrix(mainCamera.projectionMatrix));
        }
        else
        {
            scale(canvas.gameObject, mainCamera.fieldOfView);
        }
    }



    /// <summary>
    /// Get back the FOV from the Projection matrix, to bypass a round number
    /// </summary>
    /// <param name="projection"></param>
    /// <returns></returns>
    float GetFOVFromProjectionMatrix(Matrix4x4 projection)
    {
        return Mathf.Atan(1 / projection[1, 1]) * 2.0f;
    }


    /// <summary>
    /// Scale a screen in front of the camera, where all the textures will be rendered.
    /// </summary>
    /// <param name="screen"></param>
    /// <param name="fov"></param>
    private void scale(GameObject screen, float fov)
    {
        float height = Mathf.Tan(0.5f * fov) * 2.0f;
        screen.transform.localScale = new Vector3(height * aspect, height, 1);
    }
}
