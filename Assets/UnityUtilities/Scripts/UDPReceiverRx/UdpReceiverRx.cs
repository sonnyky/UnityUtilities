using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UniRx;
using UdpReceiverUniRx;
using UnityOSC;
using System;
using Zaboom;

public class UdpReceiverRx : MonoBehaviour
{
    private const int listenPort = 7777;
    private static UdpClient myClient;
    private bool isAppQuitting;
    public IObservable<UdpState> _udpSequence;

    private bool dataAvailable = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _udpSequence = Observable.Create<UdpState>(observer =>
        {
            Debug.Log(string.Format("_udpSequence thread: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
            try
            {
                myClient = new UdpClient(listenPort);
            }
            catch (SocketException ex)
            {
                observer.OnError(ex);
            }
            IPEndPoint remoteEP = null;
            myClient.EnableBroadcast = true;
            myClient.Client.ReceiveTimeout = 5000;
            while (!isAppQuitting)
            {
                try
                {
                    remoteEP = null;
                    people_position pos = people_position.Parser.ParseFrom(myClient.Receive(ref remoteEP));
                    Debug.Log("pos is :" + pos);
                    //var receivedMsg = "hey its constant";
                    //var receivedMsg = System.Text.Encoding.ASCII.GetString(myClient.Receive(ref remoteEP));
                    //observer.OnNext(new UdpState(remoteEP, receivedMsg));
                    dataAvailable = true;

                }
                catch (SocketException)
                {
                    dataAvailable = false;
                    //Debug.Log("UDP::Receive timeout");
                }
            }
            observer.OnCompleted();
            return null;
        })
            .SubscribeOn(Scheduler.ThreadPool)
            .Publish()
            .RefCount();
    }

    void OnApplicationQuit()
    {
        isAppQuitting = true;
        myClient.Client.Blocking = false;
    }
    public bool CheckDataAvailability()
    {
        return dataAvailable;
    }

}
