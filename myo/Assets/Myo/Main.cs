using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class Main : MonoBehaviour {

    private static string   COM_PORT_NAME = "COM3";
    private static int      BAUD_RATE = 9600;
    private static float    MIN_POSE_DURATION = 0.25f;

    private static byte ANGLE_OPEN = 0;
    private static byte ANGLE_CLOSE = 255;

    private Thalmic.Myo.Pose pose;
    private SerialPort stream;
    private ThalmicMyo myoRef;
    private float poseDuration;

    private static Dictionary<Thalmic.Myo.Pose, Thalmic.Myo.Pose> POSE_OVERRIDES = new Dictionary<Thalmic.Myo.Pose, Thalmic.Myo.Pose>()
    {
        {Thalmic.Myo.Pose.Fist, Thalmic.Myo.Pose.Fist},
        {Thalmic.Myo.Pose.Rest, Thalmic.Myo.Pose.Rest},
        {Thalmic.Myo.Pose.DoubleTap, Thalmic.Myo.Pose.Fist},
        {Thalmic.Myo.Pose.WaveIn, Thalmic.Myo.Pose.Rest},
        {Thalmic.Myo.Pose.WaveOut, Thalmic.Myo.Pose.Rest},
        {Thalmic.Myo.Pose.Unknown, Thalmic.Myo.Pose.Rest},
        {Thalmic.Myo.Pose.FingersSpread, Thalmic.Myo.Pose.Rest}
    };

	// Use this for initialization
	void Start () {

        pose = Thalmic.Myo.Pose.Rest;
        poseDuration = 0.0f;

        stream = new SerialPort(COM_PORT_NAME, BAUD_RATE);
		stream.WriteTimeout = 100;
		stream.Open();

        myoRef = null;
	}

	// Update is called once per frame
	void Update () {
		if (myoRef == null) {
			myoRef = FindObjectOfType<ThalmicMyo> ();
		} else {

            Thalmic.Myo.Pose overriddenPose = POSE_OVERRIDES[myoRef.pose];

            if (overriddenPose != pose && poseDuration > MIN_POSE_DURATION)
            {
                byte[] output = new byte[1];
                output[0] = ANGLE_OPEN;

                switch (overriddenPose)
                {
                    case Thalmic.Myo.Pose.Rest:
                        output[0] = ANGLE_OPEN;
                        break;
                    case Thalmic.Myo.Pose.Fist:
                        output[0] = ANGLE_CLOSE;
                        break;
                }

                pose = overriddenPose;
                poseDuration = 0.0f;

                //send hand command
                Debug.Log("Pose: " + pose.ToString() + "\nActual: " + myoRef.pose.ToString() + "\nAngle: " + output[0]);
                stream.Write(output, 0, 1);
            }
            else
            {
                poseDuration += Time.deltaTime;
            }
		}
	}

	void OnDestroy()
	{
		stream.Close ();
	}
}
