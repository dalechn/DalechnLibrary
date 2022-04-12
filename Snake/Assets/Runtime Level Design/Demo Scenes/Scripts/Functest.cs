using System.Collections;
using UnityEngine;

public partial class Functest 
{
    public Transform endMarker;
    public float speed = 1.0F;
    public float smoothDampTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private float dirVelocity = 0;

    private Transform startMarker;
    private float startTime;
    private float journeyLength;
    private bool init;

    private Vector3 tempPos;
    private Quaternion tempQua;
    private Vector3 inputDir;

    private void Start()
    {
        TransformTest();

        //StartCoroutine(CoroutineTest());

        //MaskTest();

        //FloatTest();

        //GimbalLockTest();

        //DataStructTest();

        //TestDel();

        //ObjTest();
    }

    private void Update()
    {
        InputTest();
    }

    private void TransformTest()
    {
        GameObject obj = new GameObject("StartMarker");
        startMarker = obj.transform;
        startMarker.position = tempPos = transform.position;

        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

        InvokeRepeating("DelayUpdate", 1, Time.deltaTime);
    }


    private void DelayUpdate()
    {
        if (!init)
        {
            startTime = Time.time;
            init = true;
        }
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        float step = speed * Time.deltaTime;

        //Move(step, fractionOfJourney);
        //CurveMove(step, fractionOfJourney);
        Rotate(step);

        //Align(step);
    }

    private void Move(float step, float fractionOfJourney)
    {
        //-------------------------------直线运动
        Vector3 center = (startMarker.position + endMarker.position) * 0.5F;
        center -= new Vector3(0, 1, 0);
        Vector3 riseRelCenter = startMarker.position - center;
        Vector3 setRelCenter = endMarker.position - center;

        tempPos = Vector3.Slerp(riseRelCenter, setRelCenter, fractionOfJourney);
        tempPos += center;

        //tempPos = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        //// 线性变化
        //tempPos = Vector3.MoveTowards(startMarker.position, endMarker.position, fractionOfJourney);
        //// 非匀减速曲线 无法使用
        //tempPos = Vector3.SmoothDamp(startMarker.position, endMarker.position, ref velocity, smoothDampTime);
        //// sininout
        //tempPos = new Vector3(Mathf.SmoothStep(startMarker.position.x, endMarker.position.x, fractionOfJourney), transform.position.y, transform.position.z);

        //// 非匀减速曲线 可以和lerp或者 movetoward同时用?
        //tempPos = Vector3.SmoothDamp(tempPos, endMarker.position, ref velocity, smoothDampTime);
        ////匀减速
        //tempPos = Vector3.Lerp(tempPos, endMarker.position, step);
        //tempPos = Vector3.Slerp(tempPos, endMarker.position, step);

        //// 线性变化 
        //tempPos = Vector3.MoveTowards(tempPos, endMarker.position, step);
        //// sininout 
        //tempPos = new Vector3(Mathf.SmoothStep(tempPos.x, endMarker.position.x, step), tempPos.y, tempPos.z);

        //tempPos = new Vector3(Mathf.Repeat(fractionOfJourney, 3), tempPos.y, tempPos.z);
        //tempPos = new Vector3(Mathf.PingPong(fractionOfJourney, 3), tempPos.y, tempPos.z);

        transform.position = tempPos;

    }

    [Space]
    public int numberOfSides = 10;
    public float polygonRadius = 5;


    private void CurveMove(float step, float fractionOfJourney)
    {
        ////-------------------------------------------ClampMagnitude
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Vector3 newPos = transform.position + movement;

        //Vector3 offset = newPos - polygonCenter;
        //transform.position = polygonCenter + Vector3.ClampMagnitude(offset, polygonRadius);

        // //----------------------------------曲线运动
        if (fractionOfJourney >= Mathf.PI * 2f) fractionOfJourney -= Mathf.PI * 2f;
        // 3个轴只有一个sin或者cos做来回运动 , 有至少一个sin和cos做圆周运动
        Vector3 startCorner = new Vector3(-polygonRadius, 0, -polygonRadius) + startMarker.position; // 把初始位置偏移,因为cos(0) = 1;
        //transform.position = startCorner + new Vector3(
        //     Mathf.Cos(fractionOfJourney) * polygonRadius,
        //     Mathf.Sin(fractionOfJourney) * polygonRadius,
        //     Mathf.Cos(fractionOfJourney) * polygonRadius);

        ////------------------------------------ 正弦/余弦运动
        transform.position = startMarker.position + new Vector3(
            fractionOfJourney * polygonRadius,
            Mathf.Sin(fractionOfJourney * Mathf.PI) * polygonRadius,
            fractionOfJourney * polygonRadius);

        Vector3 previousCorner = startMarker.position;
        Vector3 currentCorner = startMarker.position;
        for (int i = 1; i < numberOfSides; i++)
        {
            float cornerAngle = 2f * Mathf.PI / (float)numberOfSides * i;

            currentCorner = startCorner + new Vector3(
                Mathf.Cos(cornerAngle) * polygonRadius,
                Mathf.Sin(cornerAngle) * polygonRadius,
            Mathf.Cos(cornerAngle) * polygonRadius);

            Debug.DrawLine(currentCorner, previousCorner);

            previousCorner = currentCorner;
        }

        Debug.DrawLine(startMarker.position, previousCorner);

    }

    private Vector3 GetDir()
    {
        //Vector3 targetDirection = endMarker.position - transform.position;
        //Vector3 _normal = transform.up;
        //Vector3.OrthoNormalize(ref _normal, ref targetDirection);

        var cam = Camera.main;

        //方法1
        Vector3 targetDirection = cam.transform.TransformDirection(inputDir);
        //Vector3 targetDirection = cam.transform.rotation * inputDir;

        Vector3 _normal = transform.up;
        Vector3.OrthoNormalize(ref _normal, ref targetDirection);

        //方法2 ,只可用于x,y,z平面
        //Vector3 targetDirection = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized * inputDir.z +cam.transform.right* inputDir.x;
        //if (targetDirection.magnitude > 1) targetDirection.Normalize();

        //方法3
        //Vector3 targetDirection = Vector3.ProjectOnPlane(cam.transform.forward, transform.up).normalized * inputDir.z +
        //    Vector3.ProjectOnPlane(cam.transform.right, transform.up).normalized * inputDir.x;

        //if (targetDirection.magnitude > 1) targetDirection.Normalize();

        //if (inputDir == Vector3.zero)
        //{
        //    //Vector3 centerPos = cam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //    //targetDirection = cam.ScreenPointToRay(centerPos).direction;

        //    //targetDirection = cam.transform.position + cam.transform.forward * 100 - transform.position;
        //    targetDirection = cam.transform.forward;

        //    Vector3 _normal = transform.up;
        //    Vector3.OrthoNormalize(ref _normal, ref targetDirection);
        //}

        //Debug.DrawLine(transform.position, transform.position + targetDirection);

        return targetDirection;
    }

    private void Rotate(float step)
    {
        Vector3 _target = GetDir();

        // 测试1
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, _target, step, 0);
        //tempQua = Quaternion.LookRotation(newDirection);

        //测试2
        //float relativeAngle = 30f * Mathf.Deg2Rad;
        //Vector3 targetForward = new Vector3(Mathf.Sin(relativeAngle), 0, Mathf.Cos(relativeAngle));
        //Quaternion targetQua = Quaternion.LookRotation(targetForward) * endMarker.rotation;

        Quaternion targetQua = Quaternion.LookRotation(_target);
        tempQua = Quaternion.Slerp(tempQua, targetQua, step );

        transform.rotation = tempQua;

        //测试3
        //if (inputDir != Vector3.zero)
        //    RotateToDirection(_target, step);

        //测试4
        //transform.Rotate(new Vector3(0, step, 0), Space.Self);
        //transform.RotateAround(endMarker.position, endMarker.up, step);
        //transform.LookAt(endMarker.position);

        RotationLimit();
    }

    private void RotateToDirection(Vector3 newDirection, float step)
    {
        {
            Vector3 euler = transform.rotation.eulerAngles;

            //Vector3 targetEuler = Quaternion.LookRotation(newDirection).eulerAngles;

            float longitude = Mathf.Atan2(newDirection.x, newDirection.z) * Mathf.Rad2Deg;
            float latitude = -Mathf.Asin(newDirection.y / newDirection.magnitude) * Mathf.Rad2Deg;

            Vector3 targetEuler = new Vector3(latitude, longitude, 0);

            euler.y = Mathf.LerpAngle(euler.y, targetEuler.y, step);
            //euler.y = Mathf.MoveTowardsAngle(euler.y, targetEuler.y, step);
            //euler.y = Mathf.SmoothDampAngle(euler.y, targetEuler.y, ref dirVelocity, smoothDampTime);

            euler.x = Mathf.LerpAngle(euler.x, targetEuler.x, step);

            transform.rotation = Quaternion.Euler(euler);
            //transform.rotation = Quaternion.AngleAxis(euler.y, Vector3.up) *Quaternion.AngleAxis(euler.x, Vector3.right);

        }


        {
            //Vector3 relative = transform.InverseTransformPoint(endMarker.position);
            Vector3 relative = transform.InverseTransformDirection(newDirection);
            //Vector3 relative = Quaternion.Inverse(transform.rotation) * newDirection;

            //双轴旋转都很奇怪..
            float longitude = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            float latitude = -Mathf.Asin(relative.y / relative.magnitude) * Mathf.Rad2Deg;

            //transform.rotation = transform.rotation * Quaternion.AngleAxis(longitude * step, transform.up)
            //    * Quaternion.AngleAxis(latitude * step, transform.right);

            //transform.rotation = transform.rotation * Quaternion.Euler(latitude * step, longitude * step, 0.0f);

        }

        float destAngle = Mathf.Atan2(newDirection.x, newDirection.z) * Mathf.Rad2Deg;
        float originAngle = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(originAngle, destAngle);
        //Debug.Log(deltaAngle + " " + Vector3.Angle(transform.forward, newDirection) + " " + Vector3.SignedAngle(transform.forward, newDirection, transform.up));

    }

    private void RotationLimit()
    {
        Vector3 fixedAxis = Vector3.up;
        Vector3 transformAxis = transform.up;
        float minAngle = 0;
        float maxAngle = 45;

        var angle = Vector3.Angle(fixedAxis, transformAxis);

        // 方法1.1
        //if (angle < minAngle)
        //{
        //    var limitedSwingRotation = Vector3.RotateTowards(transformAxis, -fixedAxis, (minAngle - angle) * Mathf.Deg2Rad, 1.0f);

        //    transform.rotation = Quaternion.FromToRotation(transformAxis, limitedSwingRotation) * transform.rotation;
        //}
        //else if (angle > maxAngle)
        //{
        //    var limitedSwingRotation = Vector3.RotateTowards(transformAxis, fixedAxis, (angle - maxAngle) * Mathf.Deg2Rad, 1.0f);

        //    transform.rotation = Quaternion.FromToRotation(transformAxis, limitedSwingRotation) * transform.rotation;
        //}

        // 方法1.2
        //if (angle > maxAngle)
        //{
        //    Quaternion swingRotation = Quaternion.FromToRotation(fixedAxis, transformAxis);
        //    Quaternion limitedSwingRotation = Quaternion.RotateTowards(Quaternion.identity, swingRotation, maxAngle);

        //    transform.rotation = Quaternion.FromToRotation(transformAxis, limitedSwingRotation * fixedAxis) * transform.rotation;

        //}
    }

    private void Align(float step)
    {
        Vector3 normal = Vector3.right;

        //Quaternion destRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, normal)); //x
        Quaternion destRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, normal), normal);

        //Quaternion destRot = Quaternion.FromToRotation(Vector3.forward, Vector3.ProjectOnPlane(transform.forward, normal)); //x
        //Quaternion destRot = Quaternion.FromToRotation(transform.forward, Vector3.ProjectOnPlane(transform.forward, normal)) * transform.rotation;  //x
        //Quaternion destRot = Quaternion.FromToRotation(Vector3.up, normal);
        //Quaternion destRot = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;

        //Quaternion destRot = Quaternion.LookRotation(normal, normal); // up轴被限制了

       // 方法2
       Vector3 transformAxis = transform.up;
        float maxAngle = 45;

        Quaternion upRotation = Quaternion.FromToRotation(transformAxis, Vector3.up) * transform.rotation;
        Quaternion rotationTarget = Quaternion.RotateTowards(upRotation, Quaternion.FromToRotation(transformAxis, normal) * transform.rotation, maxAngle);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, step);

    }

}