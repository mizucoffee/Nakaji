using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {

    float speed;
    public NotesModel nm;
    MeshCollider mc;

    public void Create(NotesModel nm,float speed)
    {
        //if(nm.color == 2)
        //{
        //    transform.localScale = new Vector3(nm.size - 0.2f, 0.01f, nm.hold * speed * 1.5f + 1);
        //    Debug.Log(nm.hold);
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (transform.localScale.z - 1) / 2);
        //}
        //else
        //    transform.localScale = new Vector3(nm.size - 0.2f, 0.01f, 1);
        

        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        if (nm.type == 0 || nm.type == 1)
        {
            mesh.vertices = new Vector3[] {
                new Vector3 (nm.start - 7.8f, 0, 0),
                new Vector3 (nm.end - 7.2f, 0, 0),
                new Vector3 (nm.start - 7.8f, 0, 1),
                new Vector3 (nm.end - 7.2f, 0, 1),
            };
        }
        if (nm.type == 2)
        {
            mesh.vertices = new Vector3[] {
                new Vector3 (nm.start - 7.8f, 0, 0),
                new Vector3 (nm.end - 7.2f, 0, 0),
                new Vector3 (nm.start - 7.8f, 0, (nm.hold) * speed * 1.5f + 1),
                new Vector3 (nm.end - 7.2f, 0, (nm.hold) * speed * 1.5f + 1),
            };
        }

        mesh.triangles = new int[] {
            1, 2, 3,0, 2, 1
        };

        var filter = GetComponent<MeshFilter>();
        mesh.name = "Notes Mesh";
        filter.sharedMesh = mesh;

        mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;

        MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
        if (nm.type == 0)
            meshrenderer.material.color = new Color(2.5f, 0, 0);
        else if (nm.type == 1)
            meshrenderer.material.color = new Color(2.5f, 2.5f, 0);
        else if (nm.type == 2)
            meshrenderer.material.color = new Color(2.5f, 1.25f, 0);

        this.speed = speed;
        this.nm = nm;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0,-nm.bpm / 40 * nm.split * Time.deltaTime * speed);
    }
}
