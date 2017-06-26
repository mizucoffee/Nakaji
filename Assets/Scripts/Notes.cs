using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {

    float speed;
    public NotesModel nm;

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
            mesh.triangles = new int[] {
                //0, 2, 1, 1, 2, 3
                0, 3, 1, 0, 2, 3
            };
        }
        if (nm.type == 2)
        {
            mesh.vertices = new Vector3[] {
                new Vector3 (nm.start - 7.8f, 0, 0),
                new Vector3 (nm.end - 7.2f, 0, 0),
                new Vector3 (nm.start - 7.8f, 0, nm.hold * speed * 1.5f + 1),
                new Vector3 (nm.end - 7.2f, 0, nm.hold * speed * 1.5f + 1),
            };
            mesh.triangles = new int[] {
                //0, 2, 1, 1, 2, 3
                0, 3, 1, 0, 2, 3
            };
        }
        

        if (nm.type == 3)
        {
            Vector3[] ver = new Vector3[nm.slide.Count * 2 + 2];
            int[] tri = new int[nm.slide.Count * 6];
            ver[0] = new Vector3(nm.start - 7.8f, 0, 0);
            ver[1] = new Vector3(nm.end - 7.2f, 0, 0);

            int i = 2;
            int c = 0;
            int countStep = 0;
            foreach (SlideModel sm in nm.slide)
            {
                tri[c++] = i - 2;
                tri[c++] = i + 1;
                tri[c++] = i - 1;
                tri[c++] = i - 2;
                tri[c++] = i;
                tri[c++] = i + 1;
                countStep = countStep + sm.step;
                ver[i] = new Vector3(sm.start - 8.8f, 0, countStep * speed * 1.5f + 1);
                i++;
                ver[i] = new Vector3(sm.end - 8.2f, 0, countStep * speed * 1.5f + 1);
                i++;
            }
            mesh.vertices = ver;
            Debug.Log(ver.Length);
            mesh.triangles = tri;
            foreach (int ii in tri)
                Debug.Log(ii);
        }

        

        var filter = GetComponent<MeshFilter>();
        mesh.name = "Notes Mesh";
        filter.sharedMesh = mesh;

        MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
        if (nm.type == 0)
            meshrenderer.material.color = new Color(2.5f, 0, 0);
        else if (nm.type == 1)
            meshrenderer.material.color = new Color(2.5f, 2.5f, 0);
        else if (nm.type == 2)
            meshrenderer.material.color = new Color(2.5f, 1.25f, 0);
        else if (nm.type == 3)
            meshrenderer.material.color = new Color(1.5f, 2.5f, 2.5f);

        this.speed = speed;
        this.nm = nm;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0,-nm.bpm / 40 * nm.split * Time.deltaTime * speed);
    }
}
