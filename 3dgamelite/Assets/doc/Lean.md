leanfingerupdate                                                   LeanTouch.OnFingerUpdate
leanfingerflick				LeanTouch.OnFingerDown+LeanTouch.OnFingerUp+Update
leanfingerswipe				Update
leanfingertap                                                         LeanTouch.OnFingerTap
leanfingertapexpired 同时有双击单击的情况            LeanTouch.OnFingerTap+ LeanTouch.OnFingerExpired
leanfingerup				LeanTouch.OnFingerUp
leanfingerdown                                                      	LeanTouch.OnFingerDown
leanfingerheld				LeanTouch.OnFingerDown+LeanTouch.OnFingerUpdate
leanfingerold				LeanTouch.OnFingerOld

leanfirstdown			 LeanTouch.OnFingerDown +LeanTouch.OnFingerUp
leanlastup				LeanTouch.OnFingerDown +LeanTouch.OnFingerUp

leanmultiupdate                                                 LeanTouch.OnFingerDown +Update
leanmultiswipe				Update
leanmultitap				Update
leanmultiup				Update
leanmultidown			 LeanTouch.OnFingerDown +LeanTouch.OnFingerUp
leanmultiheld
leanmultidirection

leanmultipull
leanmultipinch
leanmultitwist

leanfingerdowncanvas +leanmanualswipe,leanmanualflick
leanmultiupdatecanvas

---------------------------------------

//这3个是单独使用
leantwistcamera---roll                     或者leanpitchyaw +leanpitchyawautorotate
leanpinchcamera ---更改fov 或者leanmaintaindistance---更改distance
leandragcamera ---move +leanconstraintoorthograpthic,leanconstraintocollider,leanconstraintocolliders,leanconstraintobox

leanmanualtranslate,leanmanualtranslaterigidbody,leanchase,leanchaserigidbody
leanmanualrotate,leanmanualtorque,leanroll
leanmanualrescale +leanconstrainscale

leangesturetoggle
leandelayedvalue
leanspawnwithfinger

leanselectbyfinger 
->leanselectablebyfinger+leanselectablerenderercolor+leanselectablepressurescale
->leanselectcount
->leanselectabledragtorque

leanpick
->leanpickable

------------------------------------

leandragselect
leanselectionbox
leandragtrail
leandragline
leanpulsescale

leandragtranslate,leandragtranslaterigidbody,leandragtranslatealong
leantwistrotate,leantwistrotateaxis
leanpinchscale

------------------------------------------
pinch- scale,zoom,distance
twist- rotate
pan-translate,drag,move


------------------------------------------

LeanGesture.GetStartScreenCenter
LeanGesture.GetScreenCenter
LeanGesture.GetLastScreenCenter

LeanGesture.GetStartScreenDistance
LeanGesture.GetScreenDistance
LeanGesture.GetLastScreenDistance

LeanGesture.GetStartScaledDistance
LeanGesture.GetScaledDistance
LeanGesture.GetLastScaledDistance

LeanGesture.GetScreenDelta
LeanGesture.GetScaledDelta
LeanGesture.GetWorldDelta

LeanFinger
LeanFingerFilter
LeanGesture

LeanTouch
LeanTouchSumulator

CW.Common
Lean.Common
Lean.Touch

-----------------------------------------
CrossPlatformInputManager
MobileControlRig 

joystick
touchpad
axistouchbutton,buttonhandler
tiltinput
