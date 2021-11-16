/*
	---------------------------------------------------
	CheckTool v.1.0 [2021-02-18]
	---------------------------------------------------
	Author: 	xgc
	Compatibility: 3ds Max 2014 and higher
	* (currently tested on 3ds Max 2014)
*/

macroScript CheckTools
ButtonText:"检查工具"
Category:"检查工具"
Tooltip:"检查工具"
(
	--几何体三角面、移动、旋转、缩放、材质命名
	--变量定义
	local listPartName = #()
	local listPosition = #()
	local listRotation = #()
	local listScale = #()
	local listNumTriangle = #()
	local listMaterialsName = #()
	local listFaceNode = #()

	--三角面总数
	local numTotalTriangle = 0
	local geometryCount = 0
	local faceNodeCount = 0

	--隐藏除了几何体和辅助对象
	max display mode
	hideByCategory.geometry = false
	hideByCategory.shapes = true
	hideByCategory.lights = true
	hideByCategory.cameras = true
	hideByCategory.helpers = false
	hideByCategory.spacewarps = true
	hideByCategory.Particles = true
	hideByCategory.Bones = true

	-- -- 记录几何体三角面、移动、旋转、缩放、材质命名
	-- max select all
	-- nodes = for node in selection where canconvertto node Editable_Mesh collect node 
	-- -- 去除 BoneGeometry、Biped_Object 骨骼对象
	-- -- canconvertto node Editable_Mesh 去除了辅助对象
	nodes = for node in geometry where ((not isKindof node.baseobject BoneGeometry) and (not isKindof node.baseobject Biped_Object) and (canconvertto node Editable_Mesh)) collect node 
	for node in nodes do
	(
		--Name
		local showName = node.name
		if node.ishidden then showName = showName + " (被隐藏) "
		print showName

		--nodeClass = classof node
		--numTriangle = if nodeClass == Editable_Poly or nodeClass == PolyMeshObject then node.mesh.numfaces else node.mesh.numfaces
		--numFaces = if nodeClass == Editable_Poly or nodeClass == PolyMeshObject then node.numfaces else node.mesh.numfaces
		--numVerts = if nodeClass == Editable_Poly or nodeClass == PolyMeshObject then node.numverts else node.mesh.numverts

		--移动旋转缩放
		local strPosition = ([node.position.x as integer, node.position.y as integer, node.position.z as integer]) as string
		local strRotation = ([node.rotation.x_rotation,node.rotation.y_rotation,node.rotation.z_rotation]) as string
		local strScale = node.scale as string

		--三角面
		local numTriangle = node.mesh.numfaces

		--材质命名
		local materialName = ""
		if node.material != null then
		(
			materialName = node.material.name
		)
		
		--添加进列表
		append listPartName showName
		append listPosition strPosition
		append listRotation strRotation
		append listScale strScale
		append listNumTriangle numTriangle
		append listMaterialsName materialName

		if (matchPattern node.name pattern:"*face*") then
		(
			append listFaceNode node
			faceNodeCount = faceNodeCount + 1
		)

		--三角面总计
		numTotalTriangle = numTotalTriangle + numTriangle

		geometryCount = geometryCount + 1
	)

	-- Particle_View 单独列出来		-- helpers 辅助对象
	max select all
	othernodes = for node in selection where (isKindof node.baseobject Particle_View) collect node 
	for node in othernodes do
	(
		local showName = node.name + " (Particle_View)"
		print showName

		--添加进列表
		append listPartName showName
		append listPosition ""
		append listRotation ""
		append listScale ""
		append listNumTriangle "0"
		append listMaterialsName ""

		geometryCount = geometryCount + 1
	)

	--模型信息----------------------------------------------------------------------------------------------
	--代码生成Rollout
	local tCode = ""
	tCode += "global tModelInfoRollout = rollout tModelInfoRollout \"模型信息\" width:750 height:600("
	tCode += "label tPartName \"名称\" width:200 height:24 pos:[10,10];"
	tCode += "label tPosition \"位置\" width:80 height:24 pos:[220,10];"
	tCode += "label tRotation \"旋转\" width:80 height:24 pos:[310,10];"
	tCode += "label tScale \"缩放\" width:80 height:24 pos:[400,10];"
	tCode += "label tNumTriangle \"三角面\" width:50 height:24 pos:[490,10];"
	tCode += "label tMatrialName \"材质名称\" width:220 height:24 pos:[550,10];"
	tCode += "\n"
	
	--定义nPosY
	nPosY = 10
	local bMatDiff = false
	for i = 1 to geometryCount do
	(
		nPosY = nPosY + 28
		sPosY = nPosY as string
		sIndex = i as string
		tCode += "label tPartName_" + sIndex + " \"" + listPartName[i] + "\" width:200 height:24 pos:[10," + sPosY + "];"
		tCode += "label tPosition_" + sIndex + " \"" + listPosition[i] + "\" width:200 height:24 pos:[220," + sPosY + "];"
		tCode += "label tRotation_" + sIndex + " \"" + listRotation[i] + "\" width:200 height:24 pos:[310," + sPosY + "];"
		tCode += "label tScale_" + sIndex + " \"" + listScale[i] + "\" width:200 height:24 pos:[400," + sPosY + "];"
		tCode += "label tNumTriangle_" + sIndex + " \"" + listNumTriangle[i] as string + "\" width:50 height:24 pos:[490," + sPosY + "];"
		
		local tFindResult = findString listMaterialsName[i] "_mat"
		tPartName = substituteString listMaterialsName[i] "_mat" ""
		if tFindResult != undefined and listPartName[i] == tPartName then
		(
			tCode += "label tMatrialName_" + sIndex + " \" " + listMaterialsName[i] + "\" width:220 height:24 pos:[550," + sPosY + "];"
		)
		else
		(
			tCode += "label tMatrialName_" + sIndex + " \" " + listMaterialsName[i] + "     —— 命名错误\" style_sunkenedge:true width:210 height:19 pos:[550," + sPosY + "];"
			bMatDiff = true 
		)

		tCode += "\n"
	)
	nPosY = nPosY + 28
	tCode += "label tPartName_Total" + " \"总计数\"" + " width:200 height:24 pos:[10," + nPosY as string + "];"
	tCode += "label tPosition_Default" + " \"默认 [0,0,0]\"" + " width:200 height:24 pos:[220," + nPosY as string + "];"
	tCode += "label tRotation_Default" + " \"默认 [0,0,0]\"" + " width:200 height:24 pos:[310," + nPosY as string + "];"
	tCode += "label tScale_Default" + " \"默认 [1,1,1]\"" + " width:200 height:24 pos:[400," + nPosY as string + "];"
	tCode += "label tNumTriangle_Total" + " \"" + numTotalTriangle as string + "\" width:50 height:24 pos:[490," + nPosY as string + "];"
	if bMatDiff then
	(
		tCode += "label tMatrialName_Total" + " \" 材质命名 错误\" style_sunkenedge:true width:210 height:19 pos:[550," + nPosY as string + "];"
	)
	else
	(
		tCode += "label tMatrialName_Total" + " \" 材质命名 正确\" width:220 height:19 pos:[550," + nPosY as string + "];"
	)
	
	if faceNodeCount > 0 then
	(
		nPosY = nPosY + 40
		tCode += "label tFaceLable \"Face角度检查:\" width:200 height:24 pos:[10," + nPosY as string + "];"
		tCode += "\n"
	)
	--face单独列出判断旋转
	for i = 1 to faceNodeCount do
	(
		nPosY = nPosY + 28
		sPosY = nPosY as string
		sIndex = i as string
		local tNode =  listFaceNode[i]
		local tRotation = [tNode.rotation.x_rotation, tNode.rotation.y_rotation, tNode.rotation.z_rotation]
		tCode += "label tFacePartName_" + sIndex + " \"" + tNode.name + "\" width:200 height:24 pos:[10," + sPosY + "];"
		tCode += "label tFacePosition_" + sIndex + " \"\" width:200 height:24 pos:[220," + sPosY + "];"
		tCode += "label tFaceRotation_" + sIndex + " \"" + (tRotation as string) + "\" width:200 height:24 pos:[310," + sPosY + "];"
		tCode += "label tFaceScale_" + sIndex + " \"\" width:200 height:24 pos:[400," + sPosY + "];"
		tCode += "label tFaceNumTriangle_" + sIndex + " \"\" width:50 height:24 pos:[490," + sPosY + "];"

		if tRotation.x==90 and tRotation.y==0 and tRotation.z==0 then
		(
			tCode += "label tFaceRot_" + sIndex + " \" face 角度正确\" width:220 height:19 pos:[550," + sPosY + "];"
		)
		else
		(
			tCode += "label tFaceRot_" + sIndex + " \" face 旋转角度应为[90,0,0]\" style_sunkenedge:true width:210 height:19 pos:[550," + sPosY + "];"
		)

		tCode += "\n"
	)

	nPosY = nPosY + 28

	tCode += ")"	--end
	local tModelInfoRollout = execute tCode
	---------------------------------------------------------------------------------------------------------


	--孤立点信息---------------------------------------------------------------------------------------------
	--代码生成Rollout2
	local tCode2 = ""
	local nPosY2 = 10
	tCode2 += "global tIsolatedVerticesRollout = rollout tIsolatedVerticesRollout \"孤立点\" width:750 height:600("
	tCode2 += "label tPartName \"名称\" width:200 height:24 pos:[10,10];"
	tCode2 += "label tIsoVerticesCnt \"孤立顶点数\" width:220 height:24 pos:[550,10];"
	tCode2 += "\n"
	local vecIdx = 0
	for obj in nodes do(
		select obj
		convertTomesh obj			--转换为可编辑网格		
		theResults = #()			--we need an array to store the results in
		resultType = IsolatedVertices.Check currentTime obj &theResults
		--> #vertex
		--vertCnt = vertCnt + theResults.count
		--print (obj.name + " " + (theResults.count as string) + " " + (theResults as string) )
		if theResults.count > 0 then
		(
			local sIndex = vecIdx as string
			vecIdx = vecIdx + 1
			nPosY2 = nPosY2 + 25
			tCode2 += "label tPartName_" + sIndex + " \"" + obj.name + "\" width:200 height:24 pos:[10," + nPosY2 as string + "];"
			tCode2 += "label tIsoVerticesCnt_" + sIndex + " \" " + theResults.count as string + "\" style_sunkenedge:true width:100 height:19 pos:[550," + nPosY2 as string + "];"
			tCode2 += "\n"
		)
		convertTopoly obj			--转换为可编辑多边形
		clearSelection()
	)
	tCode2 += ")"
	nPosY2 = nPosY2 + 25
	local tIsolatedVerticesRollout = execute tCode2
	---------------------------------------------------------------------------------------------------------


	--五边形信息---------------------------------------------------------------------------------------------
	--代码生成Rollout3
	local tCode3 = ""
	local nPosY3 = 10
	tCode3 += "global tFaceDegRollout = rollout tFaceDegRollout \"五边形\" width:750 height:600("
	tCode3 += "label tPartName \"名称\" width:200 height:24 pos:[10,10];"
	tCode3 += "label tFaceDegCnt \"五边及以上多边形个数\" width:220 height:24 pos:[550,10];"
	tCode3 += "\n"

	local faceedgIdx = 0
	for obj in nodes do(
		local faceCnt = 0
		convertTopoly obj			--转换为可编辑多边形
		local num_faces = polyop.getNumFaces obj
		for i = 1 to num_faces do
		(
			local num_face_verts = polyop.getFaceDeg obj i
			if num_face_verts > 4 do faceCnt = faceCnt + 1
		)

		if faceCnt > 0 then
		(
			local sIndex = faceedgIdx as string
			faceedgIdx = faceedgIdx + 1
			nPosY3 = nPosY3 + 25
			tCode3 += "label tPartName_" + sIndex + " \"" + obj.name + "\" width:200 height:24 pos:[10," + nPosY3 as string + "];"
			tCode3 += "label tFaceDegCnt_" + sIndex + " \" " + faceCnt as string + "\" style_sunkenedge:true width:100 height:19 pos:[550," + nPosY3 as string + "];"
			tCode3 += "\n"
		)
	)
	tCode3 += ")"
	nPosY3 = nPosY3 + 25
	local tFaceDegRollout = execute tCode3
	---------------------------------------------------------------------------------------------------------


	--Main
	--创建窗口
	rollout CheckToolsDialog "模型检查工具" width:800 height:(1000)
	(
		label Title_Check "模型检查工具" pos:[10,10] width:50 height:18
		
		-- 1.检查面数、命名、材质命名信息
		label Title_Info "1.几何体三角面、移动、旋转、缩放、材质命名：" pos:[10,38] width:300 height:18
		subRollout subRL "subRL" pos:[10,60]
		local tPosY = 0
		label label_MeshTips "Tips:模型面数范围" pos:[10,tPosY = nPosY + 105] width:400 height:18
		label label_MeshCnt_Main "主角模型：20000-25000" pos:[30,tPosY = tPosY + 20] width:400 height:18
		label label_MeshCnt_NPC "NPC  模型：10000-13000" pos:[270,tPosY = tPosY] width:400 height:18
		label label_MeshCnt_Monster "小怪模型：7000  -12000" pos:[30,tPosY = tPosY + 20] width:4000 height:18
		label label_MeshCnt_Boss "Boss 模型：13000-24000   (看复杂程度而定)" pos:[270,tPosY = tPosY] width:400 height:18
		
		-- 2.单位检查
		label Title_Unit "2.单位" pos:[10, tPosY = tPosY + 35] width:200 height:18
		label label_UnitContent "公制-厘米" pos:[20, tPosY = tPosY + 20] width:200 height:18
		label label_UnitContent_Error "Tip：正常" style_sunkenedge:true pos:[560, tPosY = tPosY] width:200 height:18
		
		-- 3.法线重置
		label Title_Normal "3.法线" pos:[10, tPosY = tPosY + 35] width:200 height:18
		button btn_UnifyNormal "统一法线" width:90 align:#left orient:#vertical pos:[20, tPosY = tPosY + 20]
		
		-- 4.孤立点
		label Title_IsolatedVertices "4.孤立点" pos:[10, tPosY = tPosY + 35] width:200 height:18
		--label lable_IsolatedVer_Info "不存在孤立点" pos:[20, tPosY = tPosY + 20] width:200 height:18
		--label lable_IsolatedVer_Error "Tip：正常" pos:[500, tPosY = tPosY] width:200 height:18
		subRollout subRL2 "subRL2" pos:[10, tPosY = tPosY + 20]

		-- 5.五边及以上多边形
		label Title_Poly "5.五边及以上多边形" pos:[10, tPosY = tPosY + 35 + nPosY2 + 10] width:200 height:18
		--label label_Poly_Info "不存在五边及以上多边形" pos:[20, tPosY = tPosY + 20] width:200 height:18
		--label label_Poly_Error "Tip：正常" pos:[500, tPosY = tPosY] width:200 height:18
		subRollout subRL3 "subRL3" pos:[10, tPosY = tPosY + 20]

		-- 6.乱码节点及病毒
		label Title_Virus "6.乱码节点及病毒" pos:[10, tPosY = tPosY + 35 + nPosY3 + 10] width:200 height:18
		label label_Virus_Info "不存在病毒" pos:[20, tPosY = tPosY + 20] width:200 height:18
		label label_Virus_Error "Tip：正常" style_sunkenedge:true pos:[560, tPosY = tPosY] width:200 height:18
		button button_ClearVirus "删除乱码节点" pos:[20, tPosY = tPosY + 20] width:120 align:#left orient:#vertical
		
		-- 2.检查单位
		fn CheckUnit = 
		(
			local showUnitText = "" 
			local errorTip = ""
			local unitDisplayType = units.DisplayType as string
			local unitMetricType = units.MetricType as string
			if unitDisplayType != "metric" or unitMetricType != "centimeters" then
			(
				showUnitText = "单位不是 \"公制-厘米\"，请重新设置"
				errorTip = "Tip：单位不正确"
			)
			else
			(
				showUnitText = "单位为：公制-厘米"
				errorTip = "Tips：单位正确"
			)
			label_UnitContent.text = showUnitText
			label_UnitContent_Error.text = errorTip
		)

		-- 4.检查是否有孤立点
		-- fn CheckIsolatedVertices = 
		-- (
		-- 	local vertCnt = 0
		-- 	for obj in nodes do(
		-- 		select obj
		-- 		convertTomesh obj			--转换为可编辑网格
				
		-- 		theResults = #()		--we need an array to store the results in
		-- 		resultType = IsolatedVertices.Check currentTime obj &theResults
		-- 		--> #vertex
		-- 		vertCnt = vertCnt + theResults.count
		-- 		--print (obj.name + " " + (theResults.count as string) + " " + (theResults as string) )
				
		-- 		convertTopoly obj			--转换为可编辑多边形
		-- 		clearSelection()
		-- 	)
		-- 	lable_IsolatedVer_Info.text = "孤立点个数为：" + (vertCnt as string)
		-- 	if vertCnt > 0 then lable_IsolatedVer_Error.text = "Tip：存在孤立顶点"
		-- )
		
		-- 5.检查是否有五边及以上的多边形
		-- fn CheckPolyByNumericEdge = 
		-- (
		-- 	local count = 0
		-- 	for obj in nodes do(
		-- 		convertTopoly obj			--转换为可编辑多边形
		-- 		local num_faces = polyop.getNumFaces obj
		-- 		for i = 1 to num_faces do
		-- 		(
		-- 			local num_face_verts = polyop.getFaceDeg obj i
		-- 			if num_face_verts > 4 do count = count + 1
		-- 		)
				
		-- 	)
		-- 	label_Poly_Info.text = "五边及以上多边形：" + (count as string)
		-- 	if count > 0 then label_Poly_Error.text = "Tip：存在五边及以上多边形"
		-- )
		
		-- 6.乱码节点及病毒
		fn CheckVirus =
		(
			--检查多余节点(命名乱码的节点) 目前只遍历一层节点
			for child in rootNode.children do 
			(		
				--正则表达式
				pattern = "[^a-zA-Z0-9_\-\.\@\(\) ]"	--除了这些字符外是否有其他字符	乱码字符
				rgx = dotNetObject "System.Text.RegularExpressions.Regex" pattern
				bisfound = rgx.IsMatch child.name
				--排除点辅助对象
				--if child.name == "　" or child.name == "" then bisfound = false
				if(bisfound) then
				(
					label_Virus_Info.text = "存在病毒，请清除！"
					label_Virus_Error.text = "Tip：存在病毒"
				)
			)
		)

		--初始化
		on CheckToolsDialog open do
		(
			print "CheckToolsDialog open"
			
			--2.单位
			CheckUnit()

			--4.孤立点
			--CheckIsolatedVertices()
			
			--5.五边及以上多边形
			--CheckPolyByNumericEdge()

			--6.乱码节点及病毒
			CheckVirus()
			
		)
		
		-- 3.统一法线
		on btn_UnifyNormal pressed do
		(
			-- print "onclick btn_UnifyNormal"
			for obj in nodes do(
				select obj
				convertTomesh obj			--转换为可编辑网格
				max modify mode				--选择修改器
				subobjectlevel = 4			--选择多边形
				--max select all				--选择所有对象
				meshOps.unifynormal obj		--Unifies the normals of the selected faces,valid in Face, Polygon, and Element Sub-Object levels.
				--统一所选面的法线，在“面”，“多边形”和“元素子对象”层中有效。
				convertTopoly obj			--转换为可编辑多边形
				clearSelection()
			)
			messagebox ("处理完成!!!\n\n") title:"Tips"
		)
		
		-- 6.清除乱码节点
		on button_ClearVirus pressed do
		(
			-- print("删除多余节点")
			-- 检查多余节点(命名乱码的节点) 目前只遍历一层节点
			for child in rootNode.children do 
			(		
				--正则表达式
				pattern = "[^a-zA-Z0-9_\-\.\@\(\) ]"	--除了这些字符外是否有其他字符	乱码字符
				rgx = dotNetObject "System.Text.RegularExpressions.Regex" pattern
				bisfound = rgx.IsMatch child.name
				--排除点辅助对象
				--if child.name == "　" or child.name == "" then bisfound = false
				if(bisfound) then
				(
					print("Delete " + child.name)
					delete child
				)
			)
			messagebox ("删除完成!!!\n\n") title:"Tips"
		)
	)
	

	--创建窗口
	Createdialog CheckToolsDialog lockHeight:false lockWidth:false
	addsubrollout CheckToolsDialog.subRL tModelInfoRollout rolledup:false
	CheckToolsDialog.subRL.width += (12)
	CheckToolsDialog.subRL.height += (nPosY + 20)
	
	addsubrollout CheckToolsDialog.subRL2 tIsolatedVerticesRollout rolledup:false
	CheckToolsDialog.subRL2.width += (12)
	CheckToolsDialog.subRL2.height += (nPosY2 + 20)

	addsubrollout CheckToolsDialog.subRL3 tFaceDegRollout rolledup:false
	CheckToolsDialog.subRL3.width += (12)
	CheckToolsDialog.subRL3.height += (nPosY3 + 20)

	max display mode
	hideByCategory.geometry = false
	hideByCategory.shapes = false
	hideByCategory.lights = false
	hideByCategory.cameras = false
	hideByCategory.helpers = false
	hideByCategory.spacewarps = false
	hideByCategory.Particles = false
	hideByCategory.Bones = false
	max select none
)