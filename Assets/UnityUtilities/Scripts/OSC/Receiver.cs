using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdpReceiverUniRx;
using UnityEngine.UI;
using System;
using Zaboom;

namespace WordAdventure
{

    public class Receiver : MonoBehaviour
    {

        [SerializeField]
        private UdpReceiverRx _udpReceiverRx;
        private IObservable<UdpState> myUdpSequence;

        void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            _udpReceiverRx = GetComponent<UdpReceiverRx>();
            myUdpSequence = _udpReceiverRx._udpSequence;

            myUdpSequence
                .ObserveOnMainThread()
                .Subscribe(x =>
                {
                    //print(x.UdpMsg);
                    //string[] str2;
                    //str2 = x.UdpMsg.Split(new char[] { ',' });
                    people_position pos = people_position.Parser.ParseFrom(System.Text.Encoding.UTF8.GetBytes(x.UdpMsg));
                })
                .AddTo(this);
        }
    }
}