using UnityEngine;
using System.IO.Ports;

public class Atari : MonoBehaviour
{
    private AudioSource sound01;
    void Start()
    {
        sound01 = GetComponent<AudioSource>();
        Open();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            sound01.PlayOneShot(sound01.clip);
        }
    }

    int[] led = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Notes")
        {
            sound01.PlayOneShot(sound01.clip);
            for (int i = collision.gameObject.GetComponent<Notes>().nm.start; i < collision.gameObject.GetComponent<Notes>().nm.width + 1; i++)
            {
                led[i] = 1;
            }
            string str = "";
            foreach (int i in led) str += i;
            Write(str + ";");
        }
        else
        {
            Debug.Log(collision.contacts[0].point.x);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Notes")
        {

            for (int i = collision.gameObject.GetComponent<Notes>().nm.start; i < collision.gameObject.GetComponent<Notes>().nm.width + 1; i++)
            {
                led[i] = 0;
            }
            string str = "";
            foreach (int i in led) str += i;
            Write(str + ";");
        }
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    foreach(ContactPoint cp in collision.contacts)
    //    {
    //        Debug.Log(cp.point.x);
    //    }
        
    //}

    private SerialPort serialPort;
    private void Open()
    {
        serialPort = new SerialPort("COM8", 9600, Parity.None, 8, StopBits.One);
        serialPort.Open();

    }

    public void Write(string message)
    {
        try
        {
            serialPort.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}