using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Gun, MachineGun, Bullet };
    public Type type;
    public int value;

    public int RotSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.up * RotSpeed * Time.deltaTime);
    }

}
