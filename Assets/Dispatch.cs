using UnityEngine;

public class Dispatch : MonoBehaviour
{
    private struct CSPARAM
    {
        public const string KERNAL = "CSMain";
        public const string RESULT = "Result";

        public const int THREAD_NUMBER_X = 8;
        public const int THREAD_NUMBER_Y = 8;
        public const int THREAD_NUMBER_Z = 1;
    }

    public RenderTexture renderTexture;
    public ComputeShader computeShader;

    private int x;
    private int y;

    private int kernalID;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            DispatchComputeShader();
    }

    private void Initialize()
    {
        renderTexture = new RenderTexture(1024, 1024, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        x = renderTexture.width / CSPARAM.THREAD_NUMBER_X;
        y = renderTexture.height / CSPARAM.THREAD_NUMBER_Y;


        kernalID = computeShader.FindKernel(CSPARAM.KERNAL);
    }

    private void DispatchComputeShader()
    {
        computeShader.SetTexture(kernalID, CSPARAM.RESULT, renderTexture);
        computeShader.Dispatch(kernalID, x, y, CSPARAM.THREAD_NUMBER_Z);
    }
}