using UnityEngine;
using System.IO.Ports;

public class Atari : MonoBehaviour
{
    void Start()
    {
        Open();
    }
    
    int[] led = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Note")
        {
            int p = collision.gameObject.GetComponent<Notes>().nm.end - collision.gameObject.GetComponent<Notes>().nm.start;
            int s = collision.gameObject.GetComponent<Notes>().nm.start;

            for (int i = p; i < p + s; i++)
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
            int p = collision.gameObject.GetComponent<Notes>().nm.end - collision.gameObject.GetComponent<Notes>().nm.start ;
            int s = collision.gameObject.GetComponent<Notes>().nm.start;


            for (int i = p; i < p + s; i++)
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