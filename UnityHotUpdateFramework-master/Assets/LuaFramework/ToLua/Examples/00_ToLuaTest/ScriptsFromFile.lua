
--TestTable---------------------------------------------------------------
varTable = {1,2,3,4,5}
varTable.default = 1
varTable.map = {}
varTable.map.name = 'map'

meta = {name = 'meta'}
setmetatable(varTable, meta)

--TestEnum----------------------------------------------------------------
space = nil

function TestEnum(cam,e)        
    print('Enum is:'..tostring(e))        

    cam.clearFlags = e;

    print(space:ToInt())        
    print(space:Equals(0) )  
    print(space == e)
    print(UnityEngine.CameraClearFlags.IntToEnum(0))        

end

--TestFunc----------------------------------------------------------------
function TestFunc(num)
    print('get func by variable' .. num)
    
    return num + 1
end

--TestCallback-------------------------------------------------------------
function DoClick1(go)                
    print('add callback in c# '..go.name)                    
end

function DoClick2()                
    print('add callback in lua')                              
end       

local t = {name = 'byself'}

function t:TestSelffunc()
    print('callback with self: '..self.name)
end       

function AddClick(listener)    

    -- 测试delegate-----------------------------------------------
    listener.onClick:Destroy()
    listener.onClick = listener.onClick + DoClick1                                                    
    listener.onClick = listener.onClick + DoClick2                      
    listener.onClick = listener.onClick + TestEventListener.OnClick(t.TestSelffunc, t)
    
    -- listener.onClick = listener.onClick - TestEventListener.OnClick(t.TestSelffunc, t)
    listener.onClick = listener.onClick - DoClick1   

    --测试event--------------------------------------------
    listener.onClickEvent = listener.onClickEvent + DoClick1
    listener.onClickEvent = listener.onClickEvent + DoClick2

    listener.onClickEvent = listener.onClickEvent - DoClick1   

    --测试重载问题??--------------------------------------------------
    listener:SetOnFinished(TestEventListener.OnClick(DoClick1))
    listener:SetOnFinished(TestEventListener.VoidDelegate(DoClick2))

end

--TestData
local utf8 = utf8

function TestString()    
    -- utf-8 test   
    local l1 = utf8.len('你好')
    local l2 = utf8.len('こんにちは')
    print('chinese string len is: '..l1..' japanese sting len: '..l2)     

    local s = '遍历字符串'                                        

    for i in utf8.byte_indices(s) do            
        local next = utf8.next(s, i)                   
        print(s:sub(i, next and next -1))
    end   

    local s1 = '天下风云出我辈'        
    print('风云 count is: '..utf8.count(s1, '风云'))
    s1 = s1:gsub('风云', '風雲')

    local function replace(s, i, j, repl_char)            
        if s:sub(i, j) == '辈' then
            return repl_char            
        end
    end

    print(utf8.replace(s1, replace, '輩'))

    -- string test
    local str = System.String.New('男儿当自强')
    local index = str:IndexOfAny('儿自')
    print('and index is: '..index)

    local buffer = str:ToCharArray()
    print('str type is: '..type(str)..' buffer[0] is ' .. buffer[0])

    local luastr = tolua.tolstring(buffer)
    print('lua string is: '..luastr..' type is: '..type(luastr))

    luastr = tolua.tolstring(str)
    print('lua string is: '..luastr)         
end

function TestInt64(x)                
    x = 789 + x
    assert(tostring(x) == '9223372036854775807')		                                       
    local low, high = int64.tonum2(x)                
    print('x value is: '..tostring(x)..' low is: '.. low .. ' high is: '..high.. ' type is: '.. tolua.typename(x))           
    local y = int64.new(1,2)                
    local z = int64.new(1,2)
    
    if y == z then
        print('int64 equals is ok, value: '..int64.tostring(y))
    end

    x = int64.new(123)                   

    if int64.equals(x, 123) then
        print('int64 equals to number ok')
    else
        print('int64 equals to number failed')
    end

    x = int64.new('78962871035984074')
    print('int64 is: '..tostring(x))

    local str = tostring(int64.new(3605690779, 30459971))                
    local n2 = int64.new(str)
    local l, h = int64.tonum2(n2)                        
    print(str..':'..tostring(n2)..' low:'..l..' high:'..h)                  

    print('----------------------------uint64-----------------------------')
    x = uint64.new('18446744073709551615')                                
    print('uint64 max is: '..tostring(x))
    l, h = uint64.tonum2(x)      
    str = tostring(uint64.new(l, h))
    print(str..':'..tostring(x)..' low:'..l..' high:'..h)     

    return y
end

--------------------------------------------------------------------------------

local Color = UnityEngine.Color
local GameObject = UnityEngine.GameObject
local ParticleSystem = UnityEngine.ParticleSystem 

function OnComplete()
    print('OnComplete CallBack')
end                       

local go = GameObject('go')
go.name = '123'

local node = go.transform
node.position = Vector3.one                  
print('gameObject is: '..tostring(go))                 

-- go:AddComponent(typeof(ParticleSystem))  --未知:这里好像要报错?
--go.transform:DOPath({Vector3.zero, Vector3.one * 10}, 1, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D, 10, nil)
--go.transform:DORotate(Vector3(0,0,360), 2, DG.Tweening.RotateMode.FastBeyond360):OnComplete(OnComplete)     

-- GameObject.Destroy(go, 2)                  
--print('delay destroy gameobject is: '..go.name)    

local box = UnityEngine.BoxCollider
function TestPick(ray)                                                                  
    local _layer = 2 ^ LayerMask.NameToLayer('Default')                
    local time = os.clock()                                                  
    local flag, hit = UnityEngine.Physics.Raycast(ray, nil, 5000, _layer)                                              
    --local flag, hit = UnityEngine.Physics.Raycast(ray, RaycastHit.out, 5000, _layer)                                
                    
    if flag then
        print('pick from lua, point: '..tostring(hit.point))                                        
    end
end
