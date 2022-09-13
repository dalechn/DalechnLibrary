require 'TestExport'                                        
local out = require 'tolua.out'
local GameObject = UnityEngine.GameObject                                

function OverloadTest(to)            
    assert(to:Test(1) == 4)            
    local flag, num = to:Test(out.int)
    assert(flag == 3 and num == 1024, 'Test(out)')
    assert(to:Test('hello') == 6, 'Test(string)')
    assert(to:Test(System.Object.New()) == 8)            
    assert(to:Test(true) == 15)
    assert(to:Test(123, 456) == 5)            
    assert(to:Test('123', '456') == 1)
    assert(to:Test(System.Object.New(), '456') == 1)
    assert(to:Test('123', 456) == 9)
    assert(to:Test('123', System.Object.New()) == 9)
    assert(to:Test(1,2,3) == 12)            
    assert(to:Test('hello') == 6)
    assert(TestExport.Test('hello', 'world') == 7)
    assert(to:TestGeneric(GameObject().transform) == 11)
    assert(to:TestCheckParamNumber(1,2,3) == 6)
    assert(to:TestCheckParamString('1', '2', '3') == '123')
    assert(to:Test(TestExport.Space.World) == 10)       

    print(to.this:get(123))
    to.this:set(1, 456)          

    local v = to:TestNullable(Vector3.New(1,2,3)) 
    print(v.z)
end

-- ReflectionTest
require 'tolua.reflection'          
tolua.loadassembly('Assembly-CSharp')        
local BindingFlags = require 'System.Reflection.BindingFlags'

function DoClick()
    print('do click')        
end 

function ReflectionTest()  
    local t = typeof('TestExport')        
    local func = tolua.getmethod(t, 'TestReflection')           
    func:Call()        
    func:Destroy()
    func = nil
    
    local objs = {Vector3.one, Vector3.zero}
    local array = tolua.toarray(objs, typeof(Vector3))
    local obj = tolua.createinstance(t, array)
    --local constructor = tolua.getconstructor(t, typeof(Vector3):MakeArrayType())
    --local obj = constructor:Call(array)        
    --constructor:Destroy()

    func = tolua.getmethod(t, 'Test', typeof('System.Int32'):MakeByRefType())        
    local r, o = func:Call(obj, 123)
    print(r..':'..o)
    func:Destroy()

    local property = tolua.getproperty(t, 'Number')
    local num = property:Get(obj, null)
    print('object Number: '..num)
    property:Set(obj, 456, null)
    num = property:Get(obj, null)
    property:Destroy()
    print('object Number: '..num)

    local field = tolua.getfield(t, 'field')
    num = field:Get(obj)
    print('object field: '.. num)
    field:Set(obj, 2048)
    num = field:Get(obj)
    field:Destroy()
    print('object field: '.. num)       
    
    field = tolua.getfield(t, 'OnClick')
    local onClick = field:Get(obj)        
    onClick = onClick + DoClick        
    field:Set(obj, onClick)        
    local click = field:Get(obj)
    click:DynamicInvoke()
    field:Destroy()
    click:Destroy()
end  