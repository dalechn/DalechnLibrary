-- 只有创建表才能[]访问
tableBase = {}
-- tableBase.__index = tableBase
function tableBase:New(age, name, sex)

    local o = o or {}
    -- 继承写法1
    setmetatable(o, {
        __index = self
    })
    -- 继承写法2
    -- setmetatable(o, self)
    -- self.__index = self

    self.age = age
    self.name = name
    self.sex = sex
    self.super = self

    return o
end

function tableBase:Init()
    print("Base Init")
end

-- 未知:     因为可以直接获得父类,子类的引用所以没有多态?
tableTest = tableBase:New(26, "I", "man")

function tableTest:DateTest()

    --[[
注释(comment)
变量类型(variable type)
变量默认值均为nil
]] --
    -- nil, boolean, number, string,function,table,
    -- thread,userdata(c/c++中使用) 未知:暂时没使用过?

    -- 使用local创建一个局部变量,与全局变量不同,局部变量只在被声明的那个代码块内有效
    -- 代码块:指的是一个控制结构内,一个函数体,或者一个chunk(变量被声明的那个文件或者文本)
    x = 10

    local int1 = 0xff -- number类型
    local bool1 = true -- boolean类型
    local bool2 = false
    local str = "STR1" -- string类型
    local str2 = 'str2 sub test'
    print(string.lower(str) .. string.sub(str2, 1, 7)) -- ..表示+, index从1开始(0也行)

    local resAnd = not (bool1 and bool2 or true)

    print(resAnd)

end

function tableTest:TableTest()

    subTable = {
        name = "fa",
        age = 45,
        sex = "man",
        -- nil,
        1,
        3,
        3,
        4,
        5
    }
    subTable["People"] = 'People'
    subTable[-1] = 09
    subTable.skill = "nothing"

    print(type(subTable))
    print(subTable[-1])
    print(subTable[1]) -- index从1开始
    print(subTable["People"])
    print(subTable.skill2)

    -- 把表中的数字连成字符串
    print(table.concat(subTable));
    table.insert(subTable, 2, "ma") -- tableTest[2] = "ma"
    table.remove(subTable, 1)

    tableTest:TableLength()

end

function tableTest:TableLength()

    print("方法1. #----------------------------------------------------")

    local tab = {1, 2, nil}
    print(#tab)
    -- 结果为2

    local tab2 = {}
    tab2[1] = nil
    tab2[2] = 2
    tab2[3] = 3
    print(#tab2)
    -- 结果为0

    -- 如果table的第一个元素key为非数字，那么#tb获取到的长度也是0。
    local tab1 = {}
    tab1["1"] = 1
    tab1["2"] = 2
    tab1["3"] = 3
    print(#tab1)
    -- 结果为0

    print("方法2. table.getn(subTable)----------------------------------------------------")
    -- 第三个数1每次变化以 1 为步长递增 ,不写默认为1
    for index = 1, table.getn(subTable), 1 do
        print(subTable[index])
    end
    
    print("方法3. i++----------------------------------------------------")
    local i = 0
    for k, v in pairs(subTable) do
        i = i + 1
    end
    print(i)

    print("----------------------------------------------------")

    local grid = {{11, 12, 13}, {21, 22, 23}, {31, 32, 33}}

    for y, row in ipairs(grid) do
        for x, value in ipairs(row) do
            print(x, y, grid[y][x])
        end
    end
    -- ipairs是没法保证完整遍历的(比如中间出现nil元素)，优势是可以有序遍历
    -- pairs是由于next的机制可以保证完整的遍历，但是却没办法保证顺序。

    -- while true do
    -- todo
    -- end
    -- if not 1~=1 then
    --     -- todo
    -- elseif true then
    --     -- todo
    -- else
    --     -- todo
    -- end

end

-- tableTest.Init = function()
function tableTest:Init()
    self.super.Init()
    print(tableTest.name)

    -- tableTest.DateTest()
    tableTest.TableTest()
    -- tableTest.LoopTest()
end

function tableTest:Extend(u)         
    local t = t or {}                        
    tolua.setpeer(u, t)     
    t.__index = t

    local get = tolua.initget(t)
    local set = tolua.initset(t)   

    local _position = u.position      
    local _base = u.base            

    --重写同名属性获取        
    get.position = function(self)                              
        return _position                
    end            

    --重写同名属性设置
    set.position = function(self, v)                 	                                            
        if _position ~= v then         
            _position = v                    
            _base.position = v                                                                      	            
        end
    end

    --重写同名函数
    function t:Translate(...)            
        print('child Translate')
        _base:Translate(...)                   
    end    
                   
    return u
end

-- tableTest.Init() --这样写使用self会报错
tableTest:Init() -- 等于传入self tableTest.Init(self)

function funcTest()
    print("funcTest")
end
-- funcTest()
