using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     截图 指定相机
///     非UI元素
/// </summary>
public class ScreenShot : MonoBehaviour
{
    private const float SCREEN_SHOT_WIDTH = 400;
    // 指定的相机
    public Camera camera;
    // 用来显示截图的RawImage 可无
    public RawImage image;
    // 图片保存的路径
    private string mPicturePath;

    // Use this for initialization
    private void Start()
    {
        mPicturePath = Application.dataPath + "/Resources/ScreenShot.png";
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 80, 50), "截图"))
            ScreenCapture();
    }

    private void ScreenCapture()
    {
        print(Screen.width + "高" + Screen.height);

        // 指定分辨率 解决卡顿
        var tempScale = SCREEN_SHOT_WIDTH/Screen.width;
        StartCoroutine(CaptureCamera(camera, new Rect(0, 0, Screen.width*tempScale, Screen.height*tempScale),
            mPicturePath));
        // StartCoroutine(CaptureCamera(camera, new Rect(0, 0, Screen.width, Screen.height), mPicturePath));
    }

    /// <summary>
    ///     截图 指定相机 范围 路径
    /// </summary>
    /// <param name="camera">The camera.</param>
    /// <param name="rect">The rect.</param>
    /// <param name="imgPath">The img path.</param>
    /// <returns></returns>
    private IEnumerator CaptureCamera(Camera camera, Rect rect, string imgPath)
    {
        yield return new WaitForEndOfFrame();
        // 创建一个RenderTexture对象
        var rt = new RenderTexture((int) rect.width, (int) rect.height, 24);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.targetTexture = rt;
        camera.Render();
        yield return new WaitForEndOfFrame();

        // 激活 读取像素。
        RenderTexture.active = rt;
        var screenShot = new Texture2D((int) rect.width, (int) rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        yield return new WaitForEndOfFrame();

        if (image != null)
            image.texture = screenShot;
        // 生成一个png图片文件
        var bytes = screenShot.EncodeToPNG();
        var filename = imgPath;
        File.WriteAllBytes(filename, bytes);
    }
}