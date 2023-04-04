using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RouteMesh : MonoBehaviour
{
    Mesh mesh;

    //Mesh variables
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    private void Awake() {
        //Creating original mesh
        mesh = new Mesh();
        mesh.name = "Route";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    //Function that takes in a start and end point and draws a cube mesh between them
    public void Generate(Vector3 startPoint, Vector3 endPoint) {

        //Defining Vertices
        vertices = new Vector3[8];    //Array is length * width (including the ends)

        //Starting point
        vertices[0] = new Vector3 (startPoint.x - 1, startPoint.y + 1, startPoint.z);
        vertices[1] = new Vector3 (startPoint.x + 1, startPoint.y + 1, startPoint.z);
        vertices[2] = new Vector3 (startPoint.x - 1, startPoint.y - 1, startPoint.z);
        vertices[3] = new Vector3 (startPoint.x + 1, startPoint.y - 1, startPoint.z);
        //Ending point
        vertices[4] = new Vector3 (endPoint.x - 1, endPoint.y + 1, endPoint.z);
        vertices[5] = new Vector3 (endPoint.x + 1, endPoint.y + 1, endPoint.z);
        vertices[6] = new Vector3 (endPoint.x - 1, endPoint.y - 1, endPoint.z);
        vertices[7] = new Vector3 (endPoint.x + 1, endPoint.y - 1, endPoint.z);


        //Defining Triangles
        triangles = new int[]
        {
            3, 2, 0,        //Start face
            1, 3, 0,
            4, 6, 7,        //End face
            4, 7, 5,
            3, 7, 6,        //Bottom face
            6, 2, 3,
            5, 3, 1,        //Forward face
            5, 7, 3,
            1, 0, 5,        //Top face
            0, 4, 5,
            6, 0, 2,        //Back face
            6, 4, 0
        };

        //Defining UV's
        uvs = new Vector2[]
        {
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),
        };

        //Drawing Mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    //Drawing gizmos on screen
    private void OnDrawGizmosSelected() {
        if (vertices == null) {
            return;
        }
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
