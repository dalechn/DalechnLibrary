--[[Notice:This lua config file is auto generate by Xls2Lua Tools，don't modify it manually! --]]
local fieldIdx = {}
fieldIdx.id = 1
fieldIdx.name = 2
fieldIdx.type = 3
fieldIdx.location = 4
fieldIdx.owned = 5
local data = {
{[[Kart01]],[[motuo]],0,[[Textures/KartUI/Kart]],true},
{[[Kart02]],[[kart]],0,[[Textures/KartUI/Kart]],true},
{[[Kart03]],[[hover]],0,[[Textures/KartUI/Kart]],true},
{[[Kart04]],[[anhei]],0,[[Textures/KartUI/Kart]],true},}
local mt = {}
mt.__index = function(a,b)
	if fieldIdx[b] then
		return a[fieldIdx[b]]
	end
	return nil
end
mt.__newindex = function(t,k,v)
	error('do not edit config')
end
mt.__metatable = false
for _,v in ipairs(data) do
	setmetatable(v,mt)
end
return data