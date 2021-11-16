(
global FaceAreaChecker
struct FaceAreaCheckerStruct
(
FaceAreaThresholdMin = 1,
FaceAreaThresholdMax = 10,
faceAreaDialog = undefined,
 
fn geomCheck theTime theNode theResults =
(
  local theCount = case classof theNode of
  (
    Editable_Poly: getNumFaces theNode
    Editable_Mesh: theNode.numfaces
  )
  local theAreaMethod = case classof theNode of
  (
    Editable_Poly: polyOp.getFaceArea
    Editable_Mesh: meshOp.getFaceArea
  )
  
  for f = 1 to theCount do
  (
    local theArea = theAreaMethod theNode f
    if theArea >= FaceAreaChecker.FaceAreaThresholdMin and theArea <= FaceAreaChecker.FaceAreaThresholdMax do append theResults f
  ) 
3
),
 
fn supportedCheck theNode =
(
  classof theNode == Editable_Mesh or classof theNode == Editable_Poly
),
 
fn configDlg =
(
  try(destroyDialog FaceAreaChecker.faceAreaDialog)catch()
  rollout faceAreaDialog "Face Area Checker"
  (
    spinner spn_FaceAreaThresholdMin "Min. Threshold:" range:[0,1000000,FaceAreaChecker.FaceAreaThresholdMin] offset:[7,-3] type:#worldUnits
    spinner spn_FaceAreaThresholdMax "Max. Threshold:" range:[0,1000000,FaceAreaChecker.FaceAreaThresholdMax] offset:[7,-3] type:#worldUnits
   
    fn updateDisplay =
    (
      XViewChecker.runCheck CurrentTime
      max views redraw
    )
    on spn_FaceAreaThresholdMin changed val do
    (
      FaceAreaChecker.FaceAreaThresholdMin = val
      updateDisplay()
    ) 
    on spn_FaceAreaThresholdMax changed val do
    (
      FaceAreaChecker.FaceAreaThresholdMax = val
      updateDisplay()
    )   
  )--end rollout
  
  createDialog faceAreaDialog
  setDialogPos faceAreaDialog mouse.screenpos
  FaceAreaChecker.faceAreaDialog = faceAreaDialog
)
)--end struct
 
try(destroyDialog FaceAreaChecker.faceAreaDialog)catch()
FaceAreaChecker = FaceAreaCheckerStruct()
 
XViewChecker.unRegisterChecker "Face Area Checker"
XViewChecker.registerChecker FaceAreaChecker.geomCheck FaceAreaChecker.supportedCheck #Faces "Face Area Checker" FaceAreaChecker.configDlg undefined undefined
)--end script