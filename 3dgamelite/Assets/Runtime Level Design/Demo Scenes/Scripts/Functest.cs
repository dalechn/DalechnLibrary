using System.Collections;
using UnityEngine;

[Invector.vClassHeader("FuntestRigidbody")]
public partial class Functest : vMonoBehaviour
{
    public Transform endMarker;
    public float speed = 1.0F;
    public float smoothDampTime = 0.3f;

    [Space]
    public int numberOfSides = 10;
    public float polygonRadius = 5;

    public GenericInput horizontalInput;
    public GenericInput verticalInput;

    private Vector3 velocity = Vector3.zero;
    private float dirVelocity = 0;

    private Transform startMarker;
    private float startTime;
    private float journeyLength;
    private bool init;

    private Vector3 tempPos;
    private Vector3 inputDir;

    private Vector3 rotateDir;

    protected virtual void Start()
    {
        TransformTest();

        //StartCoroutine(CoroutineTest());

        //MaskTest();

        //FloatTest();

        //GimbalLockTest();

        //DataStructTest();

        //TestDel();

        //ObjTest();

        int test1 = 0; // in 必须初始化,方法内不可修改
        int test2; // out 无需初始化,方法内必须修改
        int test3 =2; // ref 必须初始化,无限制
        InOutRefTest(test1,out test2,ref test3);
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


    protected virtual void DelayUpdate()
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

        //if (inputDir != Vector3.zero)
        Rotate(step);

        //Align(step);
    }


    private void InputTest()
    {
        //// 0鼠标左键,1鼠标右键,2鼠标中键 
        //if (Input.GetMouseButton(0))
        //{
        //    Debug.Log("左键按住");
        //}
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("Space按住");
        //}
        //if (Input.GetButtonDown("Horizontal"))
        //{
        //    Debug.Log("Horizontal按下");
        //}
        //if (Input.GetButtonUp("Fire3"))
        //{
        //    Debug.Log("Shift抬起");
        //}

        // gravity(使用键盘):输入复位的速度, dead(适用joystick):小于这个值会直接设置为0 ,sensitivity:响应速度,snap: 按下反方向时立即变为从0开始
        // 
        //自定义axis和GetButton需要在InputManager里配置
        //Horizontal和Vertical映射到w，a，s，d或者箭头键。
        //Fire1，Fire2，Fire3,Jump分别映射到Control(Ctrl),Option(Alt),Command(Shift),Space
        //MouseX和MouseY映射到鼠标移动的增量。

        //Debug.Log("Horizontal Axis,AxisRaw: " + Input.GetAxis("Horizontal") + " " + Input.GetAxisRaw("Horizontal")); //范围(-1,1), 范围-1,0,1(只有3个值)
        //Debug.Log("Mouse X,Y: " + Input.GetAxis("Mouse X") + " " + Input.GetAxis("Mouse Y")); //鼠标沿着屏幕X,Y移动时触发
        //Debug.Log("Mouse ScrollWheel: " + Input.GetAxis("Mouse ScrollWheel") + " " + Input.mouseScrollDelta); //当鼠标滚动轮滚动时触发
        //Debug.Log("Mouse mousePosition: " + Input.mousePosition + " " + Camera.main.ScreenToViewportPoint(Input.mousePosition));    //鼠标位置 ,位置归一化

        GetDir();
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

    private void GetDir()
    {
        inputDir = new Vector3(horizontalInput.GetAxis(), 0, verticalInput.GetAxis());

        var cam = Camera.main;

        // 看向摄像机的方向*输入的方向
        //方法1
        //moveDir = cam.transform.TransformDirection(inputDir);
        ////rotateDir = cam.transform.rotation * inputDir;

        //Vector3 _normal = transform.up;
        //Vector3.OrthoNormalize(ref _normal, ref moveDir);

        //方法2 ,只可用于x,y,z平面
        //rotateDir = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized * inputDir.z + cam.transform.right * inputDir.x;
        //if (moveDir.magnitude > 1) moveDir.Normalize();

        //方法3
        rotateDir = Vector3.ProjectOnPlane(cam.transform.forward, transform.up).normalized * inputDir.z +
            Vector3.ProjectOnPlane(cam.transform.right, transform.up).normalized * inputDir.x;

        if (rotateDir.magnitude > 1) rotateDir.Normalize();

        {
            // 看向摄像机
            //Vector3 centerPos = cam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            //rotateDir = cam.ScreenPointToRay(centerPos).direction;

            //rotateDir = cam.transform.position + cam.transform.forward * 100 - transform.position;

            //rotateDir = cam.transform.forward;

            // 看向某个方向
            rotateDir = endMarker.position - transform.position;

            Vector3 _rotNormal = transform.up;
            Vector3.OrthoNormalize(ref _rotNormal, ref rotateDir);

            //射线测试
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //rotateDir = ray.direction;
            //Debug.DrawRay(ray.origin, rotateDir);
        }

        //Debug.DrawLine(transform.position, transform.position + targetDirection);
    }

    private void Rotate(float step)
    {
        // 测试1 //magnitude 0或者1
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, rotateDir, step, 0);
        //transform.rotation = Quaternion.LookRotation(newDirection);

        //测试2
        //float relativeAngle = 30f * Mathf.Deg2Rad;
        //Vector3 targetForward = new Vector3(Mathf.Sin(relativeAngle), 0, Mathf.Cos(relativeAngle));
        //Quaternion targetQua = Quaternion.LookRotation(targetForward) * endMarker.rotation;

        Quaternion targetQua = Quaternion.LookRotation(rotateDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQua, step);

        //测试3
        //RotateToDirection(rotateDir, step);

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
            float latitude = -Mathf.Asin(newDirection.y / newDirection.magnitude) * Mathf.Rad2Deg; // 反转坐标轴

            Vector3 targetEuler = new Vector3(latitude, longitude, 0);

            euler.y = Mathf.LerpAngle(euler.y, targetEuler.y, step);
            //euler.y = Mathf.MoveTowardsAngle(euler.y, targetEuler.y, step);
            //euler.y = Mathf.SmoothDampAngle(euler.y, targetEuler.y, ref dirVelocity, smoothDampTime);
            //targetEuler.y = euler.y + Mathf.DeltaAngle(targetEuler.y, euler.y);
            //euler.y = Mathf.Lerp(euler.y, targetEuler.y, step);

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

        // 方法1
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

        // 方法2
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

        Vector3 transformAxis = transform.up;
        float maxAngle = 45;

        Quaternion upRotation = Quaternion.FromToRotation(transformAxis, Vector3.up) * transform.rotation;
        Quaternion rotationTarget = Quaternion.RotateTowards(upRotation, Quaternion.FromToRotation(transformAxis, normal) * transform.rotation, maxAngle);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, step);

    }

}