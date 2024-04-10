using System.Collections;
using System.IO.Ports;
using UnityEngine;

public class ArduinoConnector : MonoBehaviour
{
    SerialPort sp;
    public string portName = "COM3"; // Verander naar jouw COM-poort
    public int baudRate = 9600;
    private bool canPlaySounds = true;
    private Coroutine inputTimeoutCoroutine;
    private float inputTimeout = 5.0f; // Stel in op de maximale tijd in seconden om op input te wachten


    void Start()
    {
        sp = new SerialPort(portName, baudRate);
        try
        {
            sp.Open();
            sp.ReadTimeout = 1;
        }
        catch (System.Exception)
        {
            Debug.LogError("Kon seriële poort niet openen");
        }

        if (sp != null && sp.IsOpen)
        {
            inputTimeoutCoroutine = StartCoroutine(InputTimeoutRoutine());
        }
    }


    void Update()
    {
        if (sp != null && sp.IsOpen)
        {
            try
            {
                if (sp.BytesToRead > 0)
                {
                    int message = sp.ReadByte();
                    // Debug.Log(message);
                    switch (message)
                    {
                        case 1:
                            Debug.Log("bericht ontvangen");
                            ResetInputTimeout(); // Reset de timer bij elke ontvangen input
                            break;
                        case 2:
                            Debug.Log("bericht 2 ontvangen");
                            ResetInputTimeout(); // Reset de timer bij elke ontvangen input
                            break;
                        case 3:
                            if (canPlaySounds)
                            {
                                Debug.Log("Tilt sensor");
                                Sounds.StartScreamSFX.Invoke(); // Voorbeeldactie
                                ResetInputTimeout(); // Reset de timer bij elke ontvangen input
                            }
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
    }

    IEnumerator InputTimeoutRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(inputTimeout);
            // Voer actie uit als er binnen de ingestelde tijd geen input is ontvangen
            Debug.Log("Geen input ontvangen binnen de tijd, voer actie uit.");
            Sounds.StartSwearing.Invoke();

            // Voeg hier de actie toe die moet worden uitgevoerd als er geen input is
            canPlaySounds = !canPlaySounds; // Voorbeeldactie
        }
    }

    void ResetInputTimeout()
    {
        if (inputTimeoutCoroutine != null)
        {
            Sounds.StopSwearingAction.Invoke();
            StopCoroutine(inputTimeoutCoroutine);
            inputTimeoutCoroutine = StartCoroutine(InputTimeoutRoutine());
        }
    }

    void OnDestroy()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }
}
