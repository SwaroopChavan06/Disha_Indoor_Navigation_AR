using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using Unity.Collections;

public class QrCodeRecenter : MonoBehaviour
{
    [SerializeField] private ARSession session;
    [SerializeField] private Transform sessionOrigin;
    [SerializeField] private ARCameraManager cameraManager;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField] private GameObject sphere; // Reference to the sphere object
    [SerializeField] private Vector3 sphereOffset = new Vector3(0f, 0.1f, 0.5f); // Offset for sphere position

    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();
    private bool canDetect = true;

    private void Update() {}

    private void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!canDetect || !cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.MirrorY
        };

        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);
        image.Convert(conversionParams, buffer);
        image.Dispose();

        cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        cameraImageTexture.LoadRawTextureData(buffer.ToArray());
        cameraImageTexture.Apply();
        buffer.Dispose();

        var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);

        if (result != null)
        {
            SetQrCodeRecenterTarget(result.Text);
            StartCoroutine(DelayDetection(10f)); // Wait for 10 seconds before next detection
        }
    }

    private void SetQrCodeRecenterTarget(string targetText)
    {
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(targetText.ToLower()));
        if (currentTarget != null)
        {
            // Log current session origin position and rotation
            Debug.Log($"Current sessionOrigin position: {sessionOrigin.position}, rotation: {sessionOrigin.rotation}");
            
            // Reset the AR session
            session.Reset();
            
            // Log the target position and rotation
            Debug.Log($"Target position: {currentTarget.PositionObject.transform.position}, rotation: {currentTarget.PositionObject.transform.rotation}");

            // Apply the target's position and rotation to the session origin
            sessionOrigin.position = currentTarget.PositionObject.transform.position;
            sessionOrigin.rotation = currentTarget.PositionObject.transform.rotation;

            // Change the position of the sphere
            Vector3 spherePosition = currentTarget.PositionObject.transform.position + sphereOffset;
            sphere.transform.position = spherePosition;
            sphere.transform.rotation = currentTarget.PositionObject.transform.rotation;

            // Delete all marker objects
            foreach (var marker in GameObject.FindGameObjectsWithTag("Marker"))
            {
                Destroy(marker);
            }

            // Log new session origin position and rotation
            Debug.Log($"New sessionOrigin position: {sessionOrigin.position}, rotation: {sessionOrigin.rotation}");
        }
    }

    private IEnumerator DelayDetection(float delay)
    {
        canDetect = false;
        yield return new WaitForSeconds(delay);
        canDetect = true;
    }

    public void ChangeActiveFloor(string floorEntrance)
    {
        SetQrCodeRecenterTarget(floorEntrance);
    }
}
