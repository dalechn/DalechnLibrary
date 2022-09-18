require "Config/ConfigLoader/ConfigMgr/ConfigMgr"

ScrollViewTest = {}
local configName = "Kart"

function ScrollViewTest.GetConfig(index)
  local configMgr = ConfigMgr:Instance()
    local lang = configMgr:GetConfig(configName)
    -- print(lang.items[1].id .. " " .. lang.items[1].text)

    -- local myText = configMgr:GetItem(configName, 10000).text
    -- print(myText)

    return lang.items[index]
end

function ScrollViewTest.GetConfigLength(scrollView)
  local configMgr = ConfigMgr:Instance()
    local lang = configMgr:GetConfig(configName)

    local i = 0
    for _, _ in pairs(lang.items) do
        i = i + 1
    end

    scrollView.itemPrefab:GetHashCode()

    return  i
end
-- local len = ScrollViewTest.GetConfigLength()
-- print(len)