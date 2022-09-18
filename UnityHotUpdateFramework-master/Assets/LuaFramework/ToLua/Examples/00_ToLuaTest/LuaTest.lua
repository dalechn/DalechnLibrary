-- require "BasicTest"     -- 猜测:原生解释器可以全盘搜索,tolua是通过 luaState.AddSearchPath(path)设置的?

-- metatable test
tableBase = {
    baseField = 100
}
function tableBase:New()
    local o = o or {}

    -- 如果说__index字段是在访问表中不存在的值（get）是执行的操作的话
    -- 那么__nexindex字段则是在对表中不存在的值进行赋值（set）时候执行的操作,
    -- 就是只要存在__nexindex字段，那么就不会对本表新建值。

    -- 继承写法1
    -- setmetatable(o, self)
    -- self.__index = self
    -- -- tableBase.__index = tableBase

    -- 继承写法2
    setmetatable(
        o,
        {
            __index = self

            -- __newindex测试
            -- __newindex = function(table,key,value)
            --     print(key ..'字段是不存在的，不要试图给它赋值！')
            -- end
        }
    )

    self.super = self

    return o
end

function tableBase.__eq(f1, f2)
    return f1.a == f2.a
end

function tableBase.__add(f1, f2)
    local sum = {}
    sum.a = f1.a + f2.a
    sum.b = f1.b + f2.b
    return sum
end

function tableBase:Init()
    -- 需要重载的操作符
    -- 算数类型
    -- __add(a, b)                     for a + b
    -- __sub(a, b)                     for a - b
    -- __mul(a, b)                     for a * b
    -- __div(a, b)                     for a / b
    -- __mod(a, b)                     for a % b
    -- __pow(a, b)                     for a ^ b
    -- __unm(a)                        for -a
    -- __concat(a, b)                  for a .. b
    -- __len(a)                        for #a

    -- 关系类型
    -- __eq(a, b)                      for a == b
    -- __lt(a, b)                      for a < b
    -- __le(a, b)                      for a <= b

    -- table访问的元方法
    -- __index(a, b)  <fn or a table>  for a.b
    -- __newindex(a,b)
    -- __newindex(a, b, c)             for a.b = c
    -- __call(a, ...)                  for a(...)

    -- 库定义的元方法
    -- __metatable					    保护元表,不可读写
    -- __tostring

    print("------------__add,__eq test-------------------")
    local f1 = {
        a = 1,
        b = 2
    }
    local f2 = {
        a = 1,
        b = 3,
        c = 4
    }
    setmetatable(f1, tableBase)
    setmetatable(f2, tableBase)

    -- 测试__Add
    print((f1 + f2).a)
    -- 测试__eq
    print(f1 == f2)
end

tableTest = tableBase:New()
-- tableTest.tablefield = 10

--lua运算符优先级:
--后缀运算符:[]             --猜的
--指数操作符:^              --右结合
--一元运算符:not #
--基础运算符:* / % + -
--连接运算符: ..            --右结合
--关系运算符: < > <= >= ~= ==
--逻辑运算符: and or
--赋值运算符: =             --猜的

function tableTest:DateTest()
    --[[
        数据类型:
    nil, boolean, number, string,function,table,
    thread,userdata(c/c++中使用) 未知:暂时没使用过?
       ]]
    -- 使用local创建一个局部变量,与全局变量不同,局部变量只在被声明的那个代码块内有效
    -- 代码块:指的是一个控制结构内,一个函数体,或者一个chunk(变量被声明的那个文件或者文本)
    x = -10

    local int1 = 0xff ^ 5 -- number类型
    local bool1 = true -- boolean类型
    local bool2 = false
    local str = "STR1" -- string类型
    local str2 = "str2 sub test"
    local str3 = [[ []\n不会转义里边的字符]]
    print(string.lower(str) .. string.sub(str2, 1, 7)) -- ..表示+, index从1开始(0也行)

    local resAnd = not (bool1 and bool2 or true)

    print(resAnd ~= str)
    print(str3)
end

function tableTest:TableTest()
    subTable = {
        name = "fa",
        age = 45,
        sex = "man",
        ["str"] = "str1",
        [6] = "5",
        -- nil,
        1,
        3,
        3,
        4,
        5
    }
    subTable["People"] = "People"
    subTable[-1] = 09
    subTable.skill = "nothing"

    print(type(subTable))
    print(subTable[-1])
    print(subTable[1]) -- index从1开始
    print(subTable["People"])
    print(subTable.skill)

    -- 把表中的数字连成字符串
    print(table.concat(subTable))
    table.insert(subTable, 2, "ma") -- tableTest[2] = "ma"
    table.remove(subTable, 1)

    tableTest:TableLength()
end

function tableTest:TableLength()
    --猜测: 方法1和2都只能取到table里的数字型key的个数?
    -- 方法1. #
    local tab = {1, 2, nil}
    print(#tab) -- 结果为2

    local tab2 = {}
    tab2[1] = nil
    tab2[2] = 2
    tab2[3] = 3
    print(#tab2) -- 结果为0

    -- 如果table的第一个元素key为非数字，那么#tb获取到的长度也是0。
    local tab1 = {}
    tab1["1"] = 1
    tab1["2"] = 2
    tab1["3"] = 3
    print(#tab1) -- 结果为0

    -- 方法2. table.getn(subTable)
    -- for循环第三个数1每次变化以 1 为步长递增 ,不写默认为1
    for index = 1, table.getn(subTable), 1 do
        -- print(subTable[index])
    end

    -- 方法3. i++
    -- 虚变量(dummy variable):_ 用来占位,用来丢弃不需要的数值,比如不需要的返回值,或者这样:
    local i = 0
    for _, _ in pairs(subTable) do
        i = i + 1
    end
    -- print(i)

    ----------------------------
    local grid = {{11, 12, 13}, {21, 22, 23}, {31, 32, 33}}

    for y, row in ipairs(grid) do
        for x, value in ipairs(row) do
            -- print(x, y, grid[y][x])
        end
    end
    -- ipairs是没法保证完整遍历的(比如中间出现nil元素)，优势是可以有序遍历
    -- pairs是由于next的机制可以保证完整的遍历，但是却没办法保证顺序。

    -- 1、pairs和ipairs均优先输出没有key的value；
    --2、pairs会输出所有的数据，不带key的值按顺序输出，带key的值无序输出；
    --遇到nil不会停止输出
    --3、ipairs会跳过字符串的key，按顺序输出数字型key的值；
    --遇到nil会停止输出

    -- while true do
    --      --todo
    -- end

    -- if  true then
    --     -- todo
    -- elseif true then
    --     -- todo
    -- else
    --     -- todo
    -- end
end

-- tableTest.Init = function()
function tableTest:Init()
    self:DateTest()
    -- self.TableTest()

    self.super.Init()
end

-- tableTest.Init() --这样写使用self会报错
tableTest:Init() --等于传入self tableTest.Init(self),还需要原函数也用:

function funcTest()
    print("funcTest")
end
-- funcTest()
