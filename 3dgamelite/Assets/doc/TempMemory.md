
微分中值定理:
罗尔定理,柯西定理,达布定理,拉格朗日定理,泰勒公式,洛必达法则
傅里叶级数变换

__uuidof noexcept 

localhost,127.0.0.1
80 8086
tcp/ip ,get,set ,socket, tcp ,udp,ftp ,dns(domain name)
http,https


绑定到app .use 应用级别中间件
              next           全局中间件
            funName  第二个参数    局部中间件  
              err 第一个参数         错误级别中间件,所有路由之后

绑定到router.use          路由级别中间件

内置中间件
 static 解析html,css,js
 json 解析json表单 (raw)
 urlencoded     
 querystring

post请求header,body???

declare interface

直接suicide
炒币暴富-学东西-健身-孤独终老
炒币暴富-躺平-精尽人亡
送外卖-躺平-精尽人亡-被沙口恶心
送外卖-学东西-健身-孤独终老-被沙口恶心
敲ui-被优化-死
去大公司-高强度脑力-戒色-健身-有机会
去大公司-高强度脑力-戒色-健身-学双语-出国有机会
网站项目-成功-会员捞金-卖掉-找女朋友-健身-有机会
网站项目-成功-米哈游-找女朋友-健身-有机会
网站项目-失败-米哈游-当狗-健身-戒色-有机会

random Physics string 英语 攀爬 雾效 水波(透明材质) 镜面 引擎比较 math/matrix  ui测试 js yield 数组 c++ map
web:
html css js dom bom jquery(包含ajax),bootstrap

multer

如何实现继承基类成员重写???
unity泛型能否序列化??

set map
链表
dummy node

分析现有行业公司 大学
游戏发展史

设计模式???



        // W 为背包总重量, N 为物品数量
        // weights 数组存储 N 个物品的重量
        // values 数组存储 N 个物品的价值
        public int knapsack(int W, int N, int[] weights, int[] values)
        {
            //dp[i][0]和dp[0][j]没有价值已经初始化0
            int[][] dp = new int[N + 1][];
            //   从dp[1][1]开始遍历填表
            for (int i = 1; i <= N; ++i)
            {
                //  第i件物品的重量和价值
                int w = weights[i - 1], v = values[i - 1];
                for (int j = 1; j <= W; ++j)
                {
                    if (j < w)
                    {
                        //  超过当前状态能装下的重量j
                        dp[i][j] = dp[i - 1][j];
                    }
                    else
                    {
                        dp[i][j] = Mathf.Max(dp[i - 1][j], dp[i - 1][j - weights[i]] + values[i]);
                    }
                }
            }
            return dp[N][W];
        }

eslint:       不能有多行空行
                每一行后面不能有空格
                文件末尾需要有空行
                禁止多余的逗号
                注释后需要空格
                不允许存在 从未使用过的变量/常量
                形参前加空格

                查询参数???

