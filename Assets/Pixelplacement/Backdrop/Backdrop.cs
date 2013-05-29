// Copyright (c) 2010 Bob Berkebile
// Please direct any bugs/comments/suggestions to http://www.pixelplacement.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections;

[AddComponentMenu("Pixelplacement/Backdrop")]
public class Backdrop : MonoBehaviour
{
	const string SHADER_CODE = 
		"Shader \"UnlitAlpha\"{" +
			"Properties {" +
		 		"_Color (\"Color Tint (A = Opacity)\", Color) = (1,1,1,1) " +
		 		"_MainTex (\"Texture (A = Transparency)\", 2D) = \"\"" +
		 	"} " +
		 	"SubShader { " +
		 		"Tags {" +
		 			"Queue = Transparent" +
		 		"}" +
		 		"ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Pass {" +
		 			"SetTexture[_MainTex] {" +
		 				"Combine texture * constant ConstantColor[_Color]" +
		 			"}" +
		 		"}" +
		 	"}" +
		 "}";
	
	public Texture image;
	public float distance = 500;
	
	GameObject backdrop;
	Mesh mesh;
	float prevDistance;
	Vector3 prevRotation, prevPosition;
	Texture2D prevImage;
	
	void Start ()
	{
		//check that we are on a camera!
		if(camera == null){
			Debug.LogError("Backdrop must be used on a camera!");
			Destroy(this);
			return;
		}
		
		//create backdrop GameObject and components:
		backdrop = new GameObject("Backdrop");
		backdrop.AddComponent<MeshFilter>();
		backdrop.AddComponent<MeshRenderer>();
		backdrop.transform.parent = transform;
		
		backdrop.renderer.material = new Material(SHADER_CODE);
		
		mesh = backdrop.GetComponent<MeshFilter>().mesh;
		mesh.vertices = CalcVerts();
		mesh.uv = new Vector2[] {new Vector2(0,0), new Vector2(1,0), new Vector2(0,1), new Vector2(1,1)};
		mesh.triangles = new int[] {1,0,3,3,0,2};
		
	}
	
	void Update(){
		
		//reset texture if it has changed:
		if( backdrop.renderer.material.mainTexture != image ){
			backdrop.renderer.material.mainTexture = image;
		}
		
		//nothing needs to be recalculated if distance hasn't changed or a move/rotate was attempted on billboard moved without our permission:
		if(distance == prevDistance &&
		   backdrop.transform.position == prevPosition &&
		   backdrop.transform.localEulerAngles == prevRotation
		   ) return;
		
		//error for attempting a backdrop placement beyond camera's far clip plane:
		if(distance > camera.farClipPlane){
			Debug.LogError("Backdrop's distance is further than the camera's far clip plane. Extend the camera's far clip plane or reduce the billboard's distance.");
			return;
		}
		
		//error for attempting a backdrop placement before camera's near clip plane:
		if(distance < camera.nearClipPlane){
			Debug.LogError("Backdrop's distance is closer than the camera's near clip plane. Extend the distance or reduce the camera's near clip plane.");
			return;
		}
		
		
		//set backdrop's placement:
		backdrop.transform.position = transform.forward * distance;
		
		//calculate mesh:
		mesh.vertices = CalcVerts();
		mesh.RecalculateNormals();
		
		//readjust comparison values:
		prevDistance = distance;
		prevPosition = backdrop.transform.position;
		prevRotation = backdrop.transform.localEulerAngles;
	}
	
	Vector3[] CalcVerts()
	{
		return new Vector3[] {
			backdrop.transform.InverseTransformPoint(camera.ScreenToWorldPoint(new Vector3(0,0,distance))),
			backdrop.transform.InverseTransformPoint(camera.ScreenToWorldPoint(new Vector3(Screen.width,0,distance))),
			backdrop.transform.InverseTransformPoint(camera.ScreenToWorldPoint(new Vector3(0,Screen.height,distance))),
			backdrop.transform.InverseTransformPoint(camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,distance)))
		};
	}
}