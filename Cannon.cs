using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prototype
/// position prediction
/// </summary>

public class Cannon : MonoBehaviour
{
    public GameObject TargetPos;
    public float rotSpeed;
    public GameObject bullet;

    public int totalBullet;

    public float nextShootTime = 5;
    public bool doubleBarrel;
    public GameObject barrel1;
    public GameObject barrel2;

    private float _deltaTime2;
    private Transform _turret;
    private float time;
    

    // Start is called before the first frame update
    void Start()
    {
        _turret = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float newRot = GetAngleToTarget(_turret.root.position, TargetPos.transform.position);
        _turret.rotation = Quaternion.Lerp(_turret.rotation, Quaternion.Euler(0, 0, newRot), rotSpeed * Time.fixedDeltaTime);

        Debug.DrawRay(_turret.position, _turret.right * 80, Color.red);

        Vector2 tPos = TargetPos.transform.position;
        Vector2 tVelo = TargetPos.GetComponent<Rigidbody2D>().velocity; // !!!
        float dis = Vector2.Distance(_turret.transform.position, tPos);
        float travelTime = dis / 30; // 30 is a bullet speed;

        if (Time.time > time + nextShootTime)
        {
            time = Time.time;

           // Debug.Log("Shoot");

            newRot = GetAngleToTarget(_turret.position, tPos + tVelo * travelTime);
            if(!doubleBarrel)
            Shoot(_turret.transform.position, Quaternion.Euler(0,0,newRot), travelTime, tPos + tVelo * travelTime);
            else
            {
                Shoot(barrel1.transform.position, Quaternion.Euler(0, 0, newRot), travelTime, tPos + tVelo * travelTime);
                Shoot(barrel2.transform.position, Quaternion.Euler(0, 0, newRot), travelTime, tPos + tVelo * travelTime);
            }
        }

        DrawTargetPredictedPosition(tPos, tVelo, travelTime);
    }

    private void DrawTargetPredictedPosition(Vector2 pos, Vector2 velocity, float travTime)
    {
        Debug.DrawLine(_turret.position, pos + velocity * travTime, Color.green);

        Debug.DrawLine(_turret.position, pos + velocity, Color.yellow);
    }

    private void Shoot(Vector2 instPos, Quaternion predRot, float ttime, Vector2 pos)
    {
        GameObject can = Instantiate(bullet, instPos, predRot);
        can.GetComponent<CannonBullet>().owner = transform.root.gameObject;
        can.GetComponent<CannonBullet>().bulletNr = ++totalBullet;
        can.GetComponent<CannonBullet>().timeToTarget = ttime;
    }

    private float GetAngleToTarget(Vector3 startPos, Vector3 targetPos)
    {
        var dir = targetPos - startPos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return angle;
    }
}
