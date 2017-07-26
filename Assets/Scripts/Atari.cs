using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class Atari : MonoBehaviour
{
    private AudioSource sound01;
    private Thread thread;
    private bool isRunning = false;
    private string message;
    private bool isNewMessageReceived = false;
    public GameObject[] tap;

    void Start()
    {
        sound01 = GetComponent<AudioSource>();
        Open();
    }

    void OnDestroy()
    {
        Close();
    }

    private void Close()
    {
        isNewMessageReceived = false;
        isRunning = false;

        if (thread != null && thread.IsAlive)
        {
            thread.Join();
        }

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            sound01.PlayOneShot(sound01.clip);
        }
        
        if (isNewMessageReceived)
        {

            char[] cs = Convert.ToString(Convert.ToInt32(message, 16), 2).ToCharArray();
            Array.Reverse(cs);
            cs = new String(cs).PadRight(8, '0').ToCharArray();

            for (int i = 0; i < cs.Length; i++)
                if (cs[i] == '0')
                    tap[i].SetActive(false);
                else if (cs[i] == '1')
                    tap[i].SetActive(true);
        }
        isNewMessageReceived = false;
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
        serialPort = new SerialPort(@"\\.\COM11", 2000000, Parity.None, 8, StopBits.One);
        serialPort.Open();
        isRunning = true;
        thread = new Thread(Read);
        thread.Start();

    }

    private void Read()
    {
        while (isRunning&& serialPort != null && serialPort.IsOpen)
        {
            try
            {
                message = serialPort.ReadLine();
                isNewMessageReceived = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
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