using UnityEngine;

public class CPECallRecieve : MonoBehaviour 
{

    public void CPECallRecieveMethod()
    {
        Debug.Log("CPECallRecieve Method - Void");
    }

    public void CPECallRecieveMethodString(string msg)
    {
        Debug.Log("CPECallRecieve Method - " + msg);
    }

    public void CPECallRecieveMethodFloat(float val)
    {
        Debug.Log("CPECallRecieve Method - " + val.ToString());
    }

    public void CPECallRecieveMethod(int val)
    {
        Debug.Log("CPECallRecieve Method - "+val.ToString());
    }
}
