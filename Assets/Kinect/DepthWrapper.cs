using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Level of indirection for the depth image,
/// provides:
/// -a frames of depth image (no player information),
/// -an array representing which players are detected,
/// -a segmentation image for each player,
/// -bounds for the segmentation of each player.
/// </summary>


public enum ZigResolution
{
    QQVGA_160x120,
    QVGA_320x240,
    VGA_640x480,
}

public class ResolutionData
{
    protected ResolutionData(int width, int height)
    {
        Width = width;
        Height = height;
    }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public static ResolutionData FromZigResolution(ZigResolution res)
    {
        switch (res) {
            default: //fallthrough - default to QQVGA
            case ZigResolution.QQVGA_160x120:
                return new ResolutionData(160, 120);
            case ZigResolution.QVGA_320x240:
                return new ResolutionData(320, 240);
            case ZigResolution.VGA_640x480:
                return new ResolutionData(640, 480);
        }
        
    }
}


public class DepthWrapper: MonoBehaviour {
	
	public DeviceOrEmulator devOrEmu;
	private Kinect.KinectInterface kinect;
	
	private struct frameData
	{
		public short[] depthImg;
		public bool[] players;
		public bool[,] segmentation;
		public int[,] bounds;
	}
	
	public int storedFrames = 1;
	
	private bool updatedSeqmentation = false;
	private bool newSeqmentation = false;
	
	private Queue frameQueue;
	
	/// <summary>
	/// Depth image for the latest frame
	/// </summary>
	[HideInInspector]
	public short[] depthImg;
	/// <summary>
	/// players[i] true iff i has been detected in the frame
	/// </summary>
	[HideInInspector]
	public bool[] players;
	/// <summary>
	/// Array of segmentation images [player, pixel]
	/// </summary>
	[HideInInspector]
	public bool[,] segmentations;
	/// <summary>
	/// Array of bounding boxes for each player (left, right, top, bottom)
	/// </summary>
	[HideInInspector]
	//right,left,up,down : but the image is fliped horizontally.
	public int[,] bounds;
	
	// Use this for initialization
	void Start () {
		kinect = devOrEmu.getKinect();
		//allocate space to store the data of storedFrames frames.
		frameQueue = new Queue(storedFrames);
		for(int ii = 0; ii < storedFrames; ii++){	
			frameData frame = new frameData();
			frame.depthImg = new short[320 * 240];
			frame.players = new bool[Kinect.Constants.NuiSkeletonCount];
			frame.segmentation = new bool[Kinect.Constants.NuiSkeletonCount,320*240];
			frame.bounds = new int[Kinect.Constants.NuiSkeletonCount,4];
			frameQueue.Enqueue(frame);
		}
		
		
		
		
		if (target == null) {
            target = renderer;
        }
        textureSize = ResolutionData.FromZigResolution(TextureSize);
        texture = new Texture2D(textureSize.Width, textureSize.Height);
        texture.wrapMode = TextureWrapMode.Clamp;
        depthHistogramMap = new float[MaxDepth];
        depthToColor = new Color32[MaxDepth];
        outputPixels = new Color32[textureSize.Width * textureSize.Height];
        

        if (null != target) {
            target.material.mainTexture = texture;
        }
		
		
		
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LateUpdate()
	{
		updatedSeqmentation = false;
		newSeqmentation = false;
	}
	/// <summary>
	/// First call per frame checks if there is a new depth image and updates,
	/// returns true if there is new data
	/// Subsequent calls do nothing have the same return as the first call.
	/// </summary>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool pollDepth()
	{
		//Debug.Log("" + updatedSeqmentation + " " + newSeqmentation);
		if (!updatedSeqmentation)
		{
			updatedSeqmentation = true;
			if (kinect.pollDepth())
			{
				newSeqmentation = true;
				frameData frame = (frameData)frameQueue.Dequeue();
				depthImg = frame.depthImg;
				players = frame.players;
				segmentations = frame.segmentation;
				bounds = frame.bounds;
				frameQueue.Enqueue(frame);
				processDepth();
			}
		}
		
		return newSeqmentation;
	}
	
	private void processDepth()
	{
		for(int player = 0; player < Kinect.Constants.NuiSkeletonCount; player++)
		{
			//clear players
			players[player] = false;
			//clear old segmentation images
			for(int ii = 0; ii < 320 * 240; ii++)
			{
				segmentations[player,ii] = false;
			}
			//clear old bounds
			for(int ii = 0; ii < 4; ii++)
			{
				bounds[player,ii] = 0;
			}
		}
		for(int ii = 0; ii < 320 * 240; ii++)
		{
			//get x and y coords
			int xx = ii % 320;
			int yy = ii / 320;
			//extract the depth and player
			depthImg[ii] = (short)(kinect.getDepth()[ii] >> 3);
			int player = (kinect.getDepth()[ii] & 0x07) - 1;
			if (player > 0)
			{
				if (!players[player])
				{
					players[player] = true;
					segmentations[player,ii] = true;
					bounds[player,0] = xx;
					bounds[player,1] = xx;
					bounds[player,2] = yy;
					bounds[player,3] = yy;
				}
				else
				{
					segmentations[player,ii] = true;
					bounds[player,0] = Mathf.Min(bounds[player,0],xx);
					bounds[player,1] = Mathf.Max(bounds[player,1],xx);
					bounds[player,2] = Mathf.Min(bounds[player,2],yy);
					bounds[player,3] = Mathf.Max(bounds[player,3],yy);
				}
			}
		}
	}
public Renderer target;
    public ZigResolution TextureSize = ZigResolution.QQVGA_160x120;
    public Color32 BaseColor = Color.yellow;
    Texture2D texture;
    ResolutionData textureSize;

    float[] depthHistogramMap;
    Color32[] depthToColor;
    Color32[] outputPixels;
    const int MaxDepth = 10000;
	// Use this for initialization
	

    void UpdateHistogram(short[] rawDepthMap)
    {
        int i, numOfPoints = 0;

        System.Array.Clear(depthHistogramMap, 0, depthHistogramMap.Length);
        int depthIndex = 0;
        // assume only downscaling
        // calculate the amount of source pixels to move per column and row in
        // output pixels
        int factorX = 320/textureSize.Width;
        int factorY = ((240 / textureSize.Height) - 1) * 320;
        for (int y = 0; y < textureSize.Height; ++y, depthIndex += factorY) {
            for (int x = 0; x < textureSize.Width; ++x, depthIndex += factorX) {
                short pixel = rawDepthMap[depthIndex];
                if (pixel != 0) {
                    depthHistogramMap[pixel]++;
                    numOfPoints++;
                }
            }
        }
        depthHistogramMap[0] = 0;
        if (numOfPoints > 0) {
            for (i = 1; i < depthHistogramMap.Length; i++) {
                depthHistogramMap[i] += depthHistogramMap[i - 1];
            }
            depthToColor[0] = Color.black;
            for (i = 1; i < depthHistogramMap.Length; i++) {
                float intensity = (1.0f - (depthHistogramMap[i] / numOfPoints));
                //depthHistogramMap[i] = intensity * 255;
                depthToColor[i].r = (byte)(BaseColor.r * intensity);
                depthToColor[i].g = (byte)(BaseColor.g * intensity);
                depthToColor[i].b = (byte)(BaseColor.b * intensity);
                depthToColor[i].a = 255;//(byte)(BaseColor.a * intensity);
            }
        }
        

    }

    void UpdateTexture(short[] rawDepthMap)
    {
        
        int depthIndex = 0;
        int factorX = 320 / textureSize.Width;
        int factorY = ((240 / textureSize.Height) - 1) * 2;
        // invert Y axis while doing the update
        for (int y = textureSize.Height - 1; y >= 0 ; --y, depthIndex += factorY) {
            int outputIndex = y * textureSize.Width;
            for (int x = 0; x < textureSize.Width; ++x, depthIndex += factorX, ++outputIndex) {
                outputPixels[outputIndex] = depthToColor[rawDepthMap[depthIndex]];
            }
        }
        
    }

  
        
  

    

}


