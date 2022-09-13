function CoExample()
    WaitForSeconds(1)
    print('WaitForSeconds end time: ' .. UnityEngine.Time.time)

    WaitForFixedUpdate()
    print('WaitForFixedUpdate end frameCount: ' .. UnityEngine.Time.frameCount)

    WaitForEndOfFrame()
    print('WaitForEndOfFrame end frameCount: ' .. UnityEngine.Time.frameCount)

    Yield(null)
    print('yield null end frameCount: ' .. UnityEngine.Time.frameCount)

    Yield(0)
    print('yield(0) end frameCime: ' .. UnityEngine.Time.frameCount)

    local www = UnityEngine.WWW('http://www.baidu.com')
    Yield(www)
    print('yield(www) end time: ' .. UnityEngine.Time.time)

    local s = tolua.tolstring(www.bytes)
    print(s:sub(1, 128))
    print('coroutine over')
end

function fib(n)
    local a, b = 0, 1
    while n > 0 do
        a, b = b, a + b
        n = n - 1
    end

    return a
end

function CoFunc()
    print('Coroutine started')
    for i = 0, 10, 1 do
        print(fib(i))
        coroutine.wait(0.1)
    end
    print("current frameCount: " .. Time.frameCount)
    coroutine.step()
    print("yield frameCount: " .. Time.frameCount)

    local www = UnityEngine.WWW("http://www.baidu.com")
    coroutine.www(www)
    local s = tolua.tolstring(www.bytes)
    print(s:sub(1, 128))
    print('Coroutine ended')
end

function CoFunc3(len)
    print('Coroutine started')
    local i = 0
    for i = 0, len, 1 do
        local flag = coroutine.yield(fib(i))
        if not flag then
            break
        end
    end
    print('Coroutine ended')
end

local coDelay = nil
local coDelay2 = nil
local coDelay3 = nil

local json = require 'cjson'

function CoroutineTest(str)

    -- cjson test
    local data = json.decode(str)
    print(data.glossary.title)
    s = json.encode(data)
    print(s)

    -- coDelay = coroutine.start(CoFunc)
    -- coDelay2= StartCoroutine(CoExample)  

    coDelay3 = coroutine.create(CoFunc3)
    return coDelay3
end

function StopCoroutineTest()

    coroutine.stop(coDelay3)

    coroutine.stop(coDelay)

    StopCoroutine(coDelay2)
    coDelay2 = nil

end
