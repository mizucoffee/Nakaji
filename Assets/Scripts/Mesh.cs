using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]
public class Mesh : MonoBehaviour
{
    MeshCollider mc;
    private void Start ()
	{
        int from1 = -3;
        int from2 = 0;
        int to1 = -3;
        int to2 = 0;
        int size = 15;

		UnityEngine.Mesh mesh = new UnityEngine.Mesh ();
		mesh.vertices = new Vector3[] {
			new Vector3 (from1, 0, 0),
			new Vector3 (from2, 0, 0),
			new Vector3 (to1, 0, size),
			new Vector3 (to2, 0, size),
		};
		mesh.triangles = new int[] {
			1, 2, 3,0, 2, 1
		};

		var filter = GetComponent<MeshFilter> ();
        mesh.name = "Notes Mesh";
		filter.sharedMesh = mesh;
        Debug.Log(mesh.bounds.size.x);
        Debug.Log(mesh.bounds.size.y);
        Debug.Log(mesh.bounds.size.z);
        
        mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        
    }
    void FixedUpdate()
	{
		transform.position += new Vector3(0, 0, 120 / -3 / 5 * 1.5f * Time.deltaTime * 3 / 3);
        mc.convex = true;
	}
}