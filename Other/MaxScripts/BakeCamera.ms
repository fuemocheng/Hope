/*
	---------------------------------------------------
	BackCamera v.1.0 [2021-03-01]
	---------------------------------------------------
	Author: 	xgc
	Compatibility: 3ds Max 2014 and higher
	* (currently tested on 3ds Max 2014)
*/

macroScript BakeCamera
ButtonText:"烘焙摄像机"
Category:"烘焙摄像机"
Tooltip:"烘焙摄像机"
(
	rollout BakeCameraDialog "烘焙摄像机" width:200 height:300
	(
		local sObj, tObj

		label Title_Check "选择目标摄像机" pos:[10,15] width:150 height:18
		pickbutton pb_source "选择Targetcamera" pos:[10,38] width:140 autoDisplay:true align:#left
		
        label Title_FrameRange "复制帧数" pos:[10,75] width:150 height:18
        spinner sp_strFrame "Start Frame:   " range:[-10000,10000,animationRange.start] type:#integer fieldWidth:50 align:#left
		spinner sp_endFrame "End Frame:     " range:[-10000,10000,animationRange.end] type:#integer fieldWidth:50 align:#left
		spinner sp_subStep "Step sampler: " range:[0.01,1000,1] fieldWidth:50 align:#left
		
        button bt_bakeCamera "创建 FreeCamera" pos:[10,175] width:140 enabled:false align:#left

		fn BakeTransform sourceObj targetObj strFrame endFrame subStep = 
        (
            -- 打开自动关键点
			max tool animmode
			set animate on
            -- 遍历每一帧并Copy
			for t = strFrame to endFrame by subStep do
			(
                -- at time t
				sliderTime = t          
				targetObj.transform = sourceObj.transform	
				targetObj.fov = sourceObj.fov
                -- subAnim value accessor
				targetObj.target_distance = sourceObj.targetDistance   
            )
            --关闭自动关键点
			max tool animmode
			set animate off
		)

		on pb_source picked obj do
		(
			if isValidNode obj do
			(
				local tmp = ClassOf obj as string
				if tmp != "Targetcamera" then
				(
					sObj = pb_source.object = undefined
					bt_bakeCamera.enabled = false
					messagebox ("选择的不是 Targetcamera !!!\n\n") title:"Tips"
				)
				else
				(
					sObj = obj
					bt_bakeCamera.enabled = true
				)
			)
		)

		on pb_source rightclick do
		(
			sObj = pb_source.object = undefined
			bt_bakeCamera.enabled = false
		)
		
		on bt_bakeCamera pressed do
		(
			tObj = freecamera()
			print("Create Camera : " + tObj.name)
			tObj.fovType = sObj.fovType
			tObj.fov = sObj.fov
			tObj.orthoProjection = sObj.orthoProjection     --正投影
			tObj.target_distance = sObj.targetDistance
			BakeTransform sObj tObj sp_strFrame.value sp_endFrame.value sp_subStep.value
            messagebox ("创建 FreeCamera 完成 !!!\n\n") title:"Tips"
        )

		--初始化
		on CheckToolsDialog open do
		(
			--print "CheckToolsDialog open"
		)
	)

	--创建窗口
	Createdialog BakeCameraDialog lockHeight:false lockWidth:false
)
