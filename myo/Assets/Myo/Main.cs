using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Main : MonoBehaviour {

	SerialPort stream = null;
	ThalmicMyo myoRef = null;

	// Use this for initialization
	void Start () {
		stream = new SerialPort("/dev/cu.usbmodem1d1161", 9600);
		Debug.Log ("serial" + stream.WriteTimeout);
		stream.WriteTimeout = 100;
		stream.Open ();
	}

	// Update is called once per frame
	void Update () {
		if (myoRef == null) {
			myoRef = FindObjectOfType<ThalmicMyo> ();
		} else {
			if (stream.BytesToWrite < stream.WriteBufferSize) {
				float acc = ((myoRef.accelerometer.x / 5.0f) * 90.0f);
				acc = Mathf.Clamp (acc, -90.0f, 90.0f);
				byte[] output = new byte[1]{ (byte)(90.0f + acc) };
				Debug.Log (output [0]);
				stream.Write (output, 0, 1);

				stream.DiscardInBuffer ();
			} else {
				stream.DiscardOutBuffer ();
			}
		}
	}

	void OnDestroy()
	{
		stream.Close ();
	}
}
