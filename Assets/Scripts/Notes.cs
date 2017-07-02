using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour {

    float speed;
    public NotesModel nm;
    public Material hold;
    public Material slide;
    public GameObject notesPrefub;

    public void Create(NotesModel nm,float speed)
    {
        if (nm.type == 0 || nm.type == 1)
        {
            gameObject.transform.Rotate(new Vector3(90f, 0f, 0f));

            Vector3 pos = gameObject.transform.position;
            pos.x = (nm.start + nm.width / 2) - 8;
            gameObject.transform.position = pos;

            Vector3 scale = gameObject.transform.localScale;
            scale.x = nm.width;
            gameObject.transform.localScale = scale;

            this.speed = speed;
            this.nm = nm;
        }
        else
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
            if (nm.type == 2)
            {
                mesh.vertices = new Vector3[] {
                    new Vector3 (nm.start - 7.8f, 0, 0),
                    new Vector3 (nm.width - 7.2f, 0, 0),
                    new Vector3 (nm.start - 7.8f, 0, nm.hold * speed * 1.5f),
                    new Vector3 (nm.width - 7.2f, 0, nm.hold * speed * 1.5f),
                };
                mesh.uv = new Vector2[] {
                    new Vector2 (0, 0),
                    new Vector2 (1f, 0),
                    new Vector2 (0, 1f),
                    new Vector2 (1f, 1f),
                };
                mesh.triangles = new int[] {
                    0, 3, 1, 0, 2, 3
                };
                Vector3 v = gameObject.transform.position;
                v.y += 0.01f;
                GameObject n = Instantiate(notesPrefub, v, transform.rotation);
                Notes r = n.AddComponent<Notes>();
               // r.Create(new NotesModel(nm.start, nm.end,0,nm.bpm, nm.split), speed);
                
                v.z += nm.hold * speed * 1.5f ;
                GameObject n2 = Instantiate(notesPrefub, v, transform.rotation);
                Notes r2 = n2.AddComponent<Notes>();
                //r2.Create(new NotesModel(nm.start, nm.end, 0, nm.bpm, nm.split), speed);
            }
        

            if (nm.type == 3)
            {
                Vector3[] ver = new Vector3[nm.slide.Count * 2 + 2];
                int[] tri = new int[nm.slide.Count * 6];
                ver[0] = new Vector3(nm.start - 7.8f, 0, 0);
                ver[1] = new Vector3(nm.width - 7.2f, 0, 0);

                float f = 1f / 12;
                
                Vector2[] uv = new Vector2[(nm.slide.Count + 1) * 2];
                int iii = 0;
                int iiii = 4;
                for (int j = 0;j < uv.Length  ; j++)
                {
                    uv[j] = new Vector2(0,f * iii * iiii);
                    uv[++j] = new Vector2(1, f * iii * iiii);
                    iii++;
                    iiii--;
                }
                
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

                mesh.uv = uv;
                Debug.Log(ver.Length);
                Debug.Log(uv.Length);
                mesh.triangles = tri;
            }

        

            var filter = GetComponent<MeshFilter>();
            mesh.name = "Notes Mesh";
            filter.sharedMesh = mesh;

            MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
            if (nm.type == 2)
                meshrenderer.material = hold;
            else if (nm.type == 3)
                meshrenderer.material = slide;

            this.speed = speed;
            this.nm = nm;
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0,-20 * Time.deltaTime * speed);
    }
}
