/*
	---------------------------------------------------
	BakeTM v.1.0 [2009-09-09]
	---------------------------------------------------
	Author: 	Anubis [http://project3d.narod.ru]
	Started: 	v.1.0 [2009-09-09]
	Updated: 	v.1.0 [2009-09-09]
	Compatibility: 3ds Max 9 and higher
	* (currently tested on 3ds Max 2009)
*/
if ro_bakeTM != undefined do
	if ro_bakeTM.isDisplayed do
		DestroyDialog ro_bakeTM
rollout ro_bakeTM "Bake TM"
(
	local sObj, tObj, rObj
	fn BakeTransform sourceObj targetObj strFrame endFrame subStep = (
		animate on (
			for t = strFrame to endFrame by subStep do
			(
				at time t
					targetObj.transform = sourceObj.transform
			)
		)
	)
	group "Bake Transform"
	(
		pickbutton pb_source "Pick Source Object" width:140 autoDisplay:true align:#right
		pickbutton pb_target "Pick Target Object" width:140 autoDisplay:true align:#right
		spinner sp_strFrame "Start Frame: " range:[-10000,10000,animationRange.start] type:#integer fieldWidth:50
		spinner sp_endFrame "End Frame: " range:[-10000,10000,animationRange.end] type:#integer fieldWidth:50
		spinner sp_subStep "Step sampler: " range:[0.01,1000,1] fieldWidth:50 -- float
		button bt_bakeTM "Bake Transform" width:140 enabled:false
	)
	group "Reduce Keys"
	(
		pickbutton pb_obj "Pick Object" width:140 autoDisplay:true align:#right
		spinner sp_intFrame "Start Frame: " range:[-10000,10000,animationRange.start] type:#integer fieldWidth:50
		spinner sp_outFrame "End Frame: " range:[-10000,10000,animationRange.end] type:#integer fieldWidth:50
		spinner sp_sStep "Step sampler: " range:[0.01,1000,1] fieldWidth:50 -- float
		spinner sp_thres "Threshold: " range:[0.01,1000,0.5] fieldWidth:50 -- float
		button bt_redKeys "Reduce Transform Keys" width:140 enabled:false
	)
	on sp_strFrame changed val do
		if val >= sp_endFrame.value do sp_strFrame.value = sp_endFrame.value - 1
	on sp_endFrame changed val do
		if val <= sp_strFrame.value do sp_endFrame.value = sp_strFrame.value + 1
	on sp_intFrame changed val do
		if val >= sp_outFrame.value do sp_intFrame.value = sp_outFrame.value - 1
	on sp_outFrame changed val do
		if val <= sp_intFrame.value do sp_outFrame.value = sp_intFrame.value + 1
	on pb_source picked obj do
	(
		if isValidNode obj do
		(
			sObj = obj
			if isValidNode tObj do
			(
				if tObj != sObj then
					bt_bakeTM.enabled = true
				else bt_bakeTM.enabled = false
			)
		)
	)
	on pb_source rightclick do
	(
		sObj = pb_source.object = undefined
		bt_bakeTM.enabled = false
	)
	on pb_target picked obj do
	(
		if isValidNode obj do
		(
			tObj = obj
			if isValidNode sObj do
			(
				if sObj != tObj then
					bt_bakeTM.enabled = true
				else bt_bakeTM.enabled = false
			)
		)
	)
	on pb_target rightclick do
	(
		tObj = pb_target.object = undefined
		bt_bakeTM.enabled = false
	)
	on pb_obj picked obj do
	(
		if isValidNode obj do
		(
			rObj = obj
			bt_redKeys.enabled = true
		)
	)
	on pb_obj rightclick do
	(
		rObj = pb_obj.object = undefined
		bt_redKeys.enabled = false
	)
	on bt_bakeTM pressed do
	(
		BakeTransform sObj tObj sp_strFrame.value sp_endFrame.value sp_subStep.value
	)
	on bt_redKeys pressed do
	(
		max create mode -- needed to fix max2009 bug!
		--setWaitCursor()
		reduceKeys rObj.transform.controller sp_thres.value sp_sStep.value (interval sp_intFrame.value sp_outFrame.value)
		--setArrowCursor()
		DestroyDialog ro_bakeTM -- needed to fix max2009 bug!
	)
)
CreateDialog ro_bakeTM width:166 \
style:#(#style_titlebar,#style_sysmenu,#style_minimizebox)
