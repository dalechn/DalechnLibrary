
--TestArray----------------------------------------------------------------
function TestArray(array)
  local len = array.Length
  
  for i = 0, len - 1 do
      print('Array: '..tostring(array[i]))
  end

  local iter = array:GetEnumerator()

  while iter:MoveNext() do
      print('iter: '..iter.Current)
  end

  local t = array:ToTable()                
  
  for i = 1, #t do
      print('table: '.. tostring(t[i]))
  end

  local pos = array:BinarySearch(3)
  print('array BinarySearch: pos: '..pos..' value: '..array[pos])

  pos = array:IndexOf(4)
  print('array indexof bbb pos is: '..pos)
  
  return 1, '123', true
end     


--TestList----------------------------------------------------------------
function Exist2(v)
    return v == 2
end

function IsEven(v)
    return v % 2 == 0
end

function NotExist(v)
    return false
end

function Compare(a, b)
    if a > b then 
        return 1
    elseif a == b then
        return 0
    else
        return -1
    end
end

function TestList(list, list1)        
    list:Add(123)
    print('Add result: list[0] is '..list[0])
    list:AddRange(list1)
    print(string.format('AddRange result: list[1] is %d, list[2] is %d', list[1], list[2]))

    local const = list:AsReadOnly()
    print('AsReadOnley:'..const[0])    

    index = const:IndexOf(123)
    
    if index == 0 then
        print('const IndexOf is ok')
    end

    local pos = list:BinarySearch(1)
    print('BinarySearch 1 result is: '..pos)

    if list:Contains(123) then
        print('list Contain 123')
    else
        error('list Contains result fail')
    end

    if list:Exists(Exist2) then
        print('list exists 2')
    else
        error('list exists result fail')
    end                    
    
    if list:Find(Exist2) then
        print('list Find is ok')
    else
        print('list Find error')
    end

    local fa = list:FindAll(IsEven)

    if fa.Count == 2 then
        print('FindAll is ok')
    end

    --注意推导后的委托声明必须注册, 这里是System.Predicate<int>
    local index = list:FindIndex(System.Predicate_int(Exist2))

    if index == 2 then
        print('FindIndex is ok')
    end

    index = list:FindLastIndex(System.Predicate_int(Exist2))

    if index == 2 then
        print('FindLastIndex is ok')
    end                
    
    index = list:IndexOf(123)
    
    if index == 0 then
        print('IndexOf is ok')
    end

    index = list:LastIndexOf(123)
    
    if index == 0 then
        print('LastIndexOf is ok')
    end

    list:Remove(123)

    if list[0] ~= 123 then
        print('Remove is ok')
    end

    list:Insert(0, 123)

    if list[0] == 123 then
        print('Insert is ok')
    end

    list:RemoveAt(0)

    if list[0] ~= 123 then
        print('RemoveAt is ok')
    end

    list:Insert(0, 123)
    list:ForEach(function(v) print('foreach: '..v) end)
    local count = list.Count      

    list:Sort(System.Comparison_int(Compare))
    print('--------------sort list over----------------------')
                    
    for i = 0, count - 1 do
        print('for:'..list[i])
    end

    list:Clear()
    print('list Clear not count is '..list.Count)
end

--TestDict----------------------------------------------------------------
function TestDict(map)                        
    local iter = map:GetEnumerator() 
    
    while iter:MoveNext() do
        local v = iter.Current.Value
        print('id: '..v.id ..' name: '..v.name..' sex: '..v.sex)                                
    end

    local flag, account = map:TryGetValue(1, nil)

    if flag then
        print('TryGetValue result ok: '..account.name)
    end

    local keys = map.Keys
    iter = keys:GetEnumerator()
    print('------------print dictionary keys---------------')
    while iter:MoveNext() do
        print(iter.Current)
    end
    print('----------------------over----------------------')

    local values = map.Values
    iter = values:GetEnumerator()
    print('------------print dictionary values---------------')
    while iter:MoveNext() do
        print(iter.Current.name)
    end
    print('----------------------over----------------------')                

    print('kick '..map[2].name)
    map:Remove(2)
    iter = map:GetEnumerator() 

    while iter:MoveNext() do
        local v = iter.Current.Value
        print('id: '..v.id ..' name: '..v.name..' sex: '..v.sex)                                
    end
end                        
