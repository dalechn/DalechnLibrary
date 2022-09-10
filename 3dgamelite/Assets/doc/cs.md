
## 优化方案:

//少用 GetComponent AddComponent
//少用内置的引用 transform,gameObject
//for 循环用到的.Count 方法 最好单独创建一个变量 var n = list.Count;
//避免在方法内 new 引用类型
//GC 类没有引用的时候自动触发,
//解除引用: Destroy(gameobject) , list.clean();

// 资源文件: material,prefab
// shader类型:glsl,hlsl
// 模型文件(model): .fbx, .dae (Collada), .3ds, .dxf, .obj ,glTF(不支持)
// 字体文件(font):ttf(true type)
// 音频文件(audio):mp3、ogg、wav、aif、mod、it、s3m、xm
// 常见图片格式(texture):jpeg/jpg(Joint Photographic Experts Group-iso),bmp(bitmap-Microsoft),png,svg,raw,tga

## 数学相关

Matrix4x4 Mathf Vector3 Quaternion Transform / GameObject Camera

## 物理引擎相关

## 基础碰撞检测图形
// unity: Ray Plane Bounds BoundingSphere

// line, ray, segment - plane, circle, triangle, rectangle,
// polygon, box(Oriented bounding box(OOB)) / aab(Axis - aligned bounding box(AABB)), sphere

// aab - aab
// box - box, capsule
// plane - plane, sphere, triangle, aab / box
// sphere - sphere, aab / box
// triangle - triangle

// 凸包 : Fixed directions hulls, k - DOP, convex hull 没有任何一个内角是优角
// 凹包 : Concave hull 内角至少有一个优角
// 优角(reflex angle)亦称凹角，180° < θ < 360°。
// 劣角(inferior angle)亦称凸角，0° < θ < 180°

## 物理引擎:

havok(intel,2015 被微软收购)
bullet(sony)
physx(AGEIA,2008 被 nvidia 收购)

## bullet 物理引擎

// Convex Primitives:
// btBoxShape btSphereShape btCapsuleShape btCylinderShape btConeShape btMultiSphereShape
// Compound Shapes:
// btCompoundShape

// Convex Hull Shapes:
// btConvexHullShape

// Concave Triangle Meshes:
// btBvhTriangleMeshShape

// Forward Dynamic: Apply Gravity Predict Transforms
// BroadPhase: Compute AABBs Detect Pairs(区域分割 四叉树 / 八叉树)
// NarrowPhase: Compute Contacts(碰撞检测 GJK / EPA 算法)
// Forward Dynamic: Solve constrains Integrate Position

---------------------------------------------------------------------------

## computer science
    数据结构与算法(data structure and algorithm),编译原理(principle of compiling),汇编语言(Assembly Language)
    计算机组成原理(principles of computer composition)
    操作系统(operating system),计算机网络(CNT,computer networking technology)

## 操作系统:
      设备(device)管理器
      资源管理器(explorer)(IO)->                                    硬盘(hard disk)(hdd,ssd)
                        Input                                      外设(peripheral):键盘(keyboard),鼠标(mouse)
      进程(process)/服务(service)管理器                             内存(memory)(ram+rom+cache),ram:sram,dram,flash
                                                                   cpu(寄存器(register)+(L1,L2,L3缓存)+控制器(controller)+运算器(ALU(Arithmetic Logic Unit))
                                                                   gpu(寄存器+(L1,L2缓存)+显存+控制器+运算器)->显示器(display)
                                                                   音响(audio):
                                                                   网络(蓝牙(Bluetooth),WiFi)
## 平台开发:
          win c/c++,c# - mfc,qt,.net framework
          linux/macos c/c++ 
          Android java - 原生框架
          ios obj-c/swift - 原生框架
          
            编译器:
            gcc,clang-llvm
            cl.exe, link.exe, rc.exe(资源编译器?)

            记事本:Input->内存->记事本/render(以某种编码读取内存/写入硬盘数据)->(编译器)->硬盘->记事本(以某种编码读取硬盘数据)
            ASCII (ISO-8859-1)									             都占一个字节
            GB2312(包含7000+汉字)、GBK(21000+)、GB18030(27000+)               这几个是英文汉字都占2个字节，并向下兼容。
            UTF-8(Unicode的一种存储、传输方式,不是一一对应)					    英文1个字节,汉字占3个字节

## web开发: 
            ## 数据库
            mysql/oracle/sqlserver/mongodb/redis 

            ## 服务器:
            APACHE/TOMCAT           -java
            PHPStudy(mysql+apache)  -php 
            IIS                     -asp.net
            nodeJS+express          -js/ts

            ## 浏览器(browser): Edge/IE, Chrome, Firefox, Safari
            web前端 html/css/js/ts -vue/react/angular
            web后端 java/c#/php/go


## 游戏开发: 
          GameManager,UIManager,PlayerInput,CharactorController/EnemyController(Animator,Rigidbody), EnemyBehavior(Navmesh,NavmeshAgent) ,CameraController,HotFix 
          游戏引擎:Render(opengl/dx), Physics, Animator / Animation, Mesh(Meshfilter), Meshrender(shader),ParticleSystem, AI, Event, UI, Network

          unity c#, lua
          unreal c/c++
          cocos c/c++, js, lua

## 大数据开发: 
          python


网络模型:
    OSI模型:                                                                                tcp/ip模型:
    应用层(application) (DNS,SMTP/POP3(邮件发送/接收),http/https,socket,websocket)              应用层   
    表示层(presentation)                SSL/TSL(加密通信?)                                                       
    会话层(session)                     FTP                                                                
    传输层(transport)                   TCP,UDP                                                 传输层                      
    网络层(network)                     IP(v4,v6)                                               网络层   
    数据链路层(data link)               MAC                                                                              
    物理层(physics)                     Ethernet,WIFI,                                          物理层

1--------------------------cpu和内存,硬盘交互:
## 语言相关

1.数据类型:值类型(value type),引用类型(ref type)
2.数据传递方式: 值传递(pass by value),引用传递(pass by reference)
文件名/其他:FileTest,file_test
包名：com.deamerstudio.xxxtest

3.定义(define):
类名/结构体,接口名,枚举：CSharpTest/dSharpTest/d_SharpTest, IInterfaceTest,EEnumTest(ENUM_VALUE)
函数名(function): DataTest,dataTest

4.声明(declare):
变量(成员变量(一般不需要初始化),局部变量(一般需要初始化),静态变量/常量)
变量名(variable)：memberTest (属性:MemberTest)(字段:memberTest,m_MemberTest)
常量名(constant)：CONST_TEST,k_Const_test
静态名(static): s_Instance

形参(函数定义):parameter，实参(函数调用):argument/variable
回调函数(callback function)
函数指针(function pointer),箭头函数(arrow function)/lambda,delegate好像都是一个意思???

编译型语言(compiled language):c/c++
解释型语言(interpreted languages):c#,java
脚本语言(script language):js/ts,lua,python

# 解释型语言
// .net standard/opengl都是一项规范具体包含:
# 1.虚拟机/运行时(c++写的.exe)
// CLI虚拟机(Common Language Infrastructure(运行时规范))
// CTS(Common Type System(通用类型系统))                 //定义数据类型
// CLS(Common Language Specification(公共语言规范))      //定义语言间的操作性

// CLR(Commen Language Runtime) //CLI的一个实现
// .net framework clr:    c#/vb, 服务器,PC .NET Framework Class Library(FLC), asp.net, ado.net, Windows Forms
// .net core clr:         c#/vb, 跨平台,服务器,PC .NET Framework Class Library(FLC),asp.net core
// .net mono clr:         c#  跨平台, FLC实现受限,c#max version:c#4.0

# 2.编译器
// 1.JIT(Just-in-time compiler(动态编译IL(Intermediage Language(中间语言))))---(字节码(byte code)->机器码(machine code)/原生码(Native Code))
// 2.AOT(Ahead-of-time compiler(运行前编译IL))---(把编写的c#文件编译为机器码)

// vs:支持vc++, c# 
      .dll(linux叫.so)(vc++,c#) 
      .lib(linux叫.a)(vc++)
      clr.dll(vc用于和c#通信) 

// development: window sdk, dx sdk, c/c++/c#基础库(集成在vs中)
// runtime:dx, vc++                               (集成在windows中)


    进程:不共享内存区                                       
    线程:共享内存区域

最高内存地址                                                          

    内核空间(用户代码无法读写):                        
    命令行参数和环境变量

    栈区(stack area):向下增长
      .exe  CLR/JVM(JIT+GC+API)+(用户代码(部分字节码+部分原生码))

    堆区(heap area):向上增长


    全局区/数据区(data area):
    静态区static extern
    常量区(const)


    代码区(code area):
        定义(define):
        类/结构体/接口/枚举
        函数

    最低内存地址

2--------------------------cpu和内存交互, gpu(和显存/内存交互) 猜测:dx 在此阶段发送 drawcall?

    0.------------------Input Assembler(IA) 输入装配阶段 --√
            Unity自定义流水数据,写入常量缓冲区: 灯光颜色,灯光位置,环境光颜色,自定义颜色,相机位置, MVP矩阵
            Unity固定流水数据,写入顶点,索引缓冲区: 顶点,法线,切线,纹理坐标,

3--------------------------cpu和内存交互, gpu(和显存/内存交互): // 未知点: 图元(primitives)?

    1------------------ Vertex Shader(VS) 顶点着色阶段/几何阶段(geometry processing) 
    已知可以:计算裁剪空间坐标(MVP 矩阵),计算模型世界坐标法线,传递 uv 坐标
            外壳着色器(hull shader)
            曲面细分控制着色器(tessllation controller shader)
            域着色器(domain shader)
            几何着色器(geomtry shader)

    2------------------Rasterizer(RS) 光栅化阶段
    已知:采样(sampling),反走样 (anti-aliasing)(抗锯齿(anti-jaggies)处理)
           三角形设置/遍历, 把顶点数据转换为片元(fragments) 

                (Early-Z技术(将深度测试放在pixel shader之前))
    3------------------Pixel Shader(PS) 像素着色器阶段(pixel processing) 
    已知可以: 通过 灯光颜色,灯光位置,环境光颜色,自定义颜色,相机位置计算灯光颜色, 通过法线, 视线计算颜色
            => Pixel OwnerShip Test(像素所有者测试)=> Scissor Test (裁剪测试)=>Alpha Test(clip函数) => Stencil Test(模板测试) => Depth Test(深度测试) 
            =>Blending =>Dithering(抖动) =>Logic Op
            =>GBuffer=>front buffer=>frame buffer		//不知道是不是这个阶段进行的

    4------------------Output Merger(OM) 输出合并阶段  //不确定干啥的?

## mvp 矩阵

//(模型/物体/本地坐标系)local/model/space ->(model matrix(unity_ObjectToWorld 模型变换)), transform 的 trs 矩阵 == >
//未知:css 还未确定?
// (世界坐标系)world space(dx 左手, opengl 右手, unity 左手,css 左手) ->(view matrix(unity_MatrixV 视点变换)), camera 的 transform 的 trs 矩阵 == >

// (眼坐标系)view space->(perspective / orthographic projection matrix(glstate_matrix_projection 投影变换投影变换)), camera 的 project 矩阵 == >

// (归一化坐标系 normalized device coordinates)ndc space -> (齐次裁剪坐标系)homogeneous clip space ->(viewport transform 视口变换) == >

// (屏幕坐标系)screen space(dx 左上角原点(0,1), opengl 左下角原点(-1,1), unity 左下角原点(0,1),css(左上角原点(0,1)))
//
// uv 坐标: opengl( opengl 左下角原点(0,1),dx 左上角原点(0,1),unity 左下角原点(0,1),)
// 惯性坐标系:不旋转的本地坐标系
// aspect ratio : 宽高比
//

//Point:需要构造(x,y,z,1)(只要 w 分量!=0 就可以表示点)
//Vector:需要构造(x,y,z,0)
//旋转矩阵(矩阵的乘法)(右手坐标系): // 逆矩阵: 旋转矩阵是一个正交矩阵,所以求转置矩阵就行
// 绕 x 轴矩阵(yz 平面): //pitch,gradient  
//x′=x | 1,0, 0, 0 | |x|
//y′=ycosθ?6?1zsinθ | 0,cosθ,-sinθ, 0 | \*|y|
//z′=ysinθ+zcosθ | 0,sinθ, cosθ, 0 | |z|
// | 0,0, 0, 1 | |1|

// 绕 z 轴矩阵(xy 平面): //roll,banking  
//x′=ysinθ+zcosθ | cosθ,-sinθ, 0,0 | |x|
//y′=ycosθ?6?1zsinθ | sinθ, cosθ,0,0 | \* |y| //未知: 旋转矩阵还需要更多推导?
//z′=z | 0,0, 1, 0| |z|
// | 0,0, 0, 1 | |1|

// 绕 y 轴矩阵(xz 平面): // yaw,turn, heading  
//x′=zsinθ+xcosθ | cosθ,0, sinθ, 0 | |x|
//y′=y | 0, 1,0 ,0 | \*|y|
//z′=zcosθ?6?1xsinθ | -sinθ, 0,cosθ, 0 | |z|
// | 0,0, 0, 1 | |1|

//最终的顶点*旋转矩阵: E(α,π/2,β) = Rz(β) * Ry(π/2) * Rx(α) = Ryz(α) *Rzx(π/2)_ Rxy(β) = Ry(π/2) _ Rx(α-β)

//// quaternion 顶点*轴角: 3d 空间中任意一个 v 沿着单位向量 u 旋转 θ 角度之后的 v'为: v' = cos(θ)*v + (1-cos(θ))_(u.v).u + sin(θ)_(uxv)
///
/// 赋值给 transform 的 rotation(Quat=Quat),
/// 对已有 Vec3 变换(Quat*Vec3),
/// 运算 Quat*Quat
///

//平移矩阵(矩阵的加法): // 逆矩阵(矩阵的减法):
//x' | 1,0,0,dx| |x| |x+dx| | 0,0,0,-dx| |x| |x-dx|  
//y' | 0,1,0,dy|_|y| = |y+dy| | 0,0,0,-dy|_|y| = |y-dy|
//z' | 0,0,1,dz| |z| |z+dz| | 0,0,0,-dz| |z| |z-dz|
// | 0,0,0,1 | |1| |1| | 0,0,0,1 | |1| |1|

//缩放矩阵(矩阵的乘法) //逆矩阵:
//x' | sx,0,0,0| |x| | 1/sx,0,0,0| |x|  
//y' | 0,sy,0,0|_|y| | 0,1/sy,0,0|_|y|
//z' | 0,0,sz,0| |z| | 0,0,1/sz,0| |z|  
// | 0,0,0,1 | |1| | 0,0,0, 1 | |1|

//正交投影矩阵:

//透视投影矩阵:
