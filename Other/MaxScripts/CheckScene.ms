macroScript realCleaner
category:"# Scripts"
toolTip:"Cleaner"
buttonText:"Cleaner"
(
	clearListener()
	
 	try destroyDialog cCleanerRollout catch()
	
	global lTasks_4648134686 =
	#(
		(struct s( tName = "--场景--3dsMax的操作足迹",
			
			tTooltip = "\
			3ds Max 会在场景中会自动保存 <常规操作与动画操作的足迹>.-----------------------------------------\
			然而这些<操作足迹>可以变得相当多,极大地增加你的场景的大小,使储存变得极其缓慢.----------\
			所以这是一个好工具,用来观察这些自动创建<操作足迹>和清洁它们.------------------------------------\
			记住,如果你从其他文件导入当前场景时注意xrefs外部参考文件.--------------------------------------\
			如果使用工具清理场景解决不了问题,你也得打扫xref外部参考文件.
			",
			
			fn autocount =
			(
				try (numNoteTracks TrackViewNodes.Max_MotionClip_Manager) catch ("-")
			),
			
			fn clean =
			(
				try 
				(
					trackViewNodes[#Max_MotionClip_Manager].track = copy trackViewNodes[#Max_MotionClip_Manager].track
					gc()
				)
				catch()
			)
			
		);)(),
		
		(struct s( tName = "Reactor 碰撞",
			
			tTooltip = "\
			3ds Max Reactor碰撞会保存在你的场景.-----------\
			正如 notetracks 会增加场景文件大小,内存使用量,保存时间.----------------------------------------------\
			原动力学中的碰撞清除 在3dsMax2012以后的版本中取消了.\
			",
			
			fn autocount =
			(
				if reactor != undefined
					then reactor.getNumStoredCollisions()
					else "-"
			),
			
			fn clean =
			(
				if reactor != undefined then reactor.clearStoredCollisions()
			)
			
		);)(),
		
		(struct s( tName = "空的图层",
			
			tTooltip = "删除不包含任何对象的空图层。注意,默认层0不能被删除.",
			
			fn autocount =
			(
				local nEmptyLayers = 0
				
				for nLayer = layerManager.count to 2 by -1 do
				(
					local layer = layerManager.getLayer (nLayer - 1)
					
					local lNodes = undefined
					layer.nodes &lNodes
					
					if lNodes.count == 0 do nEmptyLayers += 1
				)
				
				return nEmptyLayers
			),
			
			fn clean =
			(
				-- active layer = layer 0
				(layerManager.getLayer 0).current = true
				
				for nLayer = layerManager.count to 2 by -1 do
				(
					local layer = layerManager.getLayer (nLayer - 1)
					
					local lNodes = undefined
					layer.nodes &lNodes
					
					if lNodes.count == 0 do layerManager.deleteLayerByName layer.name
				)
			)
			
		);)(),
		
		(struct s( tName = "重置材质编辑器",
			
			tTooltip = "重置材质编辑器的材质球,启用背景.",
			
			fn autocount =
			(
				"-"
			),
			
			fn clean =
			(
				for nMat = 2 to meditMaterials.count do
				(
					local tMat = "mat"+(sceneMaterials.count + nMat) as string
					local mat = if matchpattern (renderers.current as string) pattern:"v_ray*"
						then vraymtl name:tMat
						else standardmaterial name:tMat
					
					mat.diffuse = (color 30 30 30)
					setMTLMEditFlags mat #{2..3}
					setMeditMaterial nMat mat
				)
			)
			
		);)(),
		
		(struct s( tName = "丢失的插件",
			
			tTooltip = "搜索当前场景中的插件,标出可能造成场景错误的插件.",
			
			fn autocount =
			(
				""
			),
			
			fn usercount =
			(
				local nCount = 0
				
				local t = stringstream ""
				apropos "*missing*" to:t
				t = t as string
				
				local l = filterstring t "\n "
				local lMissingClasses = #()
				for t in l do
				(
					if matchpattern t pattern:"missing*" then
					(
						local cls = execute t
						if matchpattern (cls as string) pattern:"missing*" then
						(
							local lInst = getclassinstances cls
							nCount += lInst.count
						)
					)
				)
				
				nCount
			),
			
			fn clean =
			(
				local t = stringstream ""
				apropos "*missing*" to:t
				t = t as string
				
				local l = filterstring t "\n "
				local lMissingClasses = #()
				for t in l do
				(
					if matchpattern t pattern:"missing*" then
					(
						local cls = execute t
						if matchpattern (cls as string) pattern:"missing*" then
						(
							local lInst = getclassinstances cls
							print lInst
						)
					)
				)
			)
			
		);)(),
		
		(struct s( tName = "丢失的自定义属性",
			
			tTooltip = "如果你打开一个文件缺少插件或自定义属性的信息时,那么这可能帮助你解决它。从列表中选择节点,修改“定义属性”项。不用通过“自定义属性”来修改,脚本需要你选择需要修改的对象.",
			
			fn autocount =
			(
				"-"
			),
			
			fn clean =
			(
				local pickedTrack = trackView.pickTrackDlg()
				if pickedTrack != undefined then deleteItem pickedTrack.anim.CustAttributes 1
			)
			
		);)(),
		
		(struct s( tName = "场景 状态",
			
			tTooltip = "场景状态可以帮助你处理许多相机使用不同的设置的场景.但是如果您保存的材质在当前场景,那么这可以控制你的场景文件大小和缓慢的加载和保存.",
			
			fn autocount =
			(
				sceneStateMgr.GetCount()   
			),
			
			fn clean =
			(
				for n=sceneStateMgr.GetCount() to 1 by -1 do sceneStateMgr.Delete (sceneStateMgr.GetSceneState n)
			)
			
		);)(),
		
		(struct s( tName = "清理选择集",
			
			tTooltip = "清理所有的选择集.",
			
			fn autocount =
			(
				selectionSets.count
			),
			
			fn clean =
			(
				for n=selectionSets.count to 1 by -1 do deleteItem selectionSets selectionSets[n]
			)
			
		);)(),
		
		(struct s( tName = "时间控制器",
			
			tTooltip = "The Retimer Controller was introduced in 3ds Max 2013 as an alternative procedural EaseCurve controller. It can be used in the EaseCurve track of any controller or as a Global Retirmer Controller affecting the whole scene.",
			
			fn autocount =
			(
				if RetimerMan == undefined then "-" else RetimerMan.numRetimers 
			),
			
			fn clean =
			(
				-- for hamletik by Artyom, suggested by TexeL
				nrt=RetimerMan.numRetimers
				for i = 1 to nrt do
				(
					j=nrt-i+1
					n=RetimerMan.GetNthRetimer j
					if n!=undefined then
					(
						if n.nummarkers==0 then
						(
							RetimerMan.DeleteRetimer j
						)
					)
				)
			)
			
		);)(),
		
		(struct s( tName = "--IDs--对象ID设置为0",
			
			tTooltip = "设置场景中所有对象ID设置为0.",
			
			fn autocount =
			(
				local lObjIds = #()
				
				for obj in objects where obj.gbufferchannel != 0 do appendIfUnique lObjIds obj.gbufferchannel
				
				print "object ids:"
				print ((sort lObjIds) as string)
				
				return lObjIds.count
			),
			
			fn clean =
			(
				for obj in objects do obj.gbufferchannel = 0
			),
			
			fn selectObjects =
			(
				local lObjs = #()
				for obj in objects where obj.gbufferchannel != 0 do append lObjs obj
				
				select lObjs
				
				local bRefreshSel = false
				for obj in selection do
				(
					if isGroupHead obj and not isOpenGroupHead obj then
					(
						bRefreshSel = true
						setGroupOpen obj true
					)
				)
				if bRefreshSel then select lObjs
			)
			
		);)(),
		
		(struct s( tName = "材质ID通道设置为0",
			
			tTooltip = "场景中的所有材质ID通道设置为0.",
			
			fn autocount =
			(
				local lMatIds = #()
				
				for mat in scenematerials do
				(
					try
					(
						if classof mat == multimaterial
							then for subMat in mat.materialList where subMat.effectsChannel != 0 do appendIfUnique lMatIds subMat.effectsChannel 
							else if mat.effectsChannel != 0 then appendIfUnique lMatIds mat.effectsChannel
					)
					catch ()
				)
				
				print "material ids:"
				print ((sort lMatIds) as string)
				
				return lMatIds.count
			),
			
			fn clean =
			(
				for mat in scenematerials do
				(
					try
					(
						if classof mat == multimaterial
							then for subMat in mat.materialList where subMat.effectsChannel != 0 do subMat.effectsChannel = 0
							else if mat.effectsChannel != 0 then mat.effectsChannel = 0
					)
					catch ()
				)
			),
			
			fn selectObjects =
			(
				local lMats = #()
				
				for mat in scenematerials do
				(
					try
					(
						if classof mat == multimaterial
							then for subMat in mat.materialList where subMat.effectsChannel != 0 do appendIfUnique lMats subMat
							else if mat.effectsChannel != 0 then appendIfUnique lMats mat
					)
					catch ()
				)
				
				local lObjs = #()
				for mat in lMats do for obj in (refs.dependents mat) where classof (superclassof obj) == node do appendIfUnique lObjs obj
				
				select lObjs
				
				local bRefreshSel = false
				for obj in selection do
				(
					if isGroupHead obj and not isOpenGroupHead obj then
					(
						bRefreshSel = true
						setGroupOpen obj true
					)
				)
				if bRefreshSel then select lObjs
			),
			
			fn selectMaterials =
			(
				local lMats = #()
				
				for mat in scenematerials do
				(
					try
					(
						if classof mat == multimaterial
							then for subMat in mat.materialList where subMat.effectsChannel != 0 do appendIfUnique lMats subMat
							else if mat.effectsChannel != 0 then appendIfUnique lMats mat
					)
					catch ()
				)
				
				for nMat = 1 to lMats.count where nMat <= meditMaterials.count do setMeditMaterial nMat lMats[nMat]
			)
			
		);)(),
		
		(struct s( tName = "--几何体--壳 修改器. 没开拉直角的",
			
			tTooltip = "可选择没开<将角拉直>的对象,启用<直角>避免倾斜的边缘.",
			
			fn autocount =
			(
				local nCount = 0
				
				for modObj in getclassinstances shell where not modObj.straightenCorners do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				for modObj in getclassinstances shell where not modObj.straightenCorners do modObj.straightenCorners = true
			),
			
			fn selectObjects =
			(
				local lmodInst = getclassinstances shell
				
				local lObjs = #()
				for modObj in lmodInst do for obj in (refs.dependents modObj) where classof (superclassof obj) == node do appendIfUnique lObjs obj
				
				select lObjs
				
				local bRefreshSel = false
				for obj in selection do
				(
					if isGroupHead obj and not isOpenGroupHead obj then
					(
						bRefreshSel = true
						setGroupOpen obj true
					)
				)
				if bRefreshSel then select lObjs
			)
			
		);)(),
		
		(struct s( tName = "Vray Flipped IES",
			
			tTooltip = "IES 比例必须是正确的,或法线向上翻的,会找出来.",
			
			fn autocount =
			(
				local nCount = 0
				
				for iesLight in lights where classof iesLight == VRayIES do
				(
					if iesLight.transform[3].z < 0 and iesLight.center.z > iesLight.target.pos.z then nCount += 1
				)
				
				nCount
			),
			
			fn clean =
			(
				for iesLight in lights where classof iesLight == VRayIES do
				(
					if iesLight.transform[3].z < 0 and iesLight.center.z > iesLight.target.pos.z then
					(
						-- unlink
						local lChilds = for child in iesLight.children collect child
						local parent = iesLight.parent
						for child in lChilds do child.parent = undefined
						iesLight.parent = undefined
						
						iesLight.scale = [1,1,1]
						
						-- relink
						for child in lChilds do child.parent = iesLight
						iesLight.parent = parent
					)
				)
			)
			
		);)(),
		
		(struct s( tName = "链接组合对象",
			
			tTooltip = "有时,当你导入AutoCad模型到max时, 你会发现很多无形的链接组合类型的对象.",
			
			fn autocount =
			(
				(getclassinstances LinkComposite).count
			),
			
			fn clean =
			(
				delete (for o in (getclassinstances LinkComposite astrackviewpick:true) collect o.client)
			)
			
		);)(),
		
		(struct s( tName = "缺失的几何对象",
			
			tTooltip = "缺少插件来显示你场景中的对象.",
			
			fn autocount =
			(
				local nCount = 0
				
				for obj in objects where matchpattern ((classof obj) as string) pattern:"*miss*" do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				for obj in objects where matchpattern ((classof obj) as string) pattern:"*miss*" do delete obj
			),
			
			fn selectObjects =
			(
				local lObjs = #()
				for obj in objects where matchpattern ((classof obj) as string) pattern:"*miss*" do append lObjs obj
				
				select lObjs
				local bRefreshSel = false
				for obj in selection do
				(
					if isGroupHead obj and not isOpenGroupHead obj then
					(
						bRefreshSel = true
						setGroupOpen obj true
					)
				)
				if bRefreshSel then select lObjs
			)
			
		);)(),
		
		(struct s( tName = "--材质--丢失的程序贴图",
			
			tTooltip = "丢失的程序贴图,表明在你的场景中是使用了程序贴图插件的,而你的3dmax上没有.",
			
			fn autocount =
			(
				(getclassinstances missing_texturemap).count
			),
			
			fn clean =
			(
				local lTvps = #()
				for tvp in (getclassinstances missing_texturemap asTrackViewPick:true) do appendIfUnique lTvps tvp.client
				for tvp in lTvps do 
				(
					local lPropNames = getPropNames tvp
					for propName in lPropNames do
					(
						local prop = getproperty tvp propName
						if classof prop == missing_texturemap then try setProperty tvp propName undefined catch()
					)
				)
			),
			
			fn selectObjects =
			(
				local lObjs = #()
				for mat in (getclassinstances missing_texturemap) do
				(
					for obj in (refs.dependents mat) where classof (superclassof obj) == node do appendIfUnique lObjs obj
				)
				
				select lObjs
				local bRefreshSel = false
				for obj in selection do
				(
					if isGroupHead obj and not isOpenGroupHead obj then
					(
						bRefreshSel = true
						setGroupOpen obj true
					)
				)
				if bRefreshSel then select lObjs
			),
			
			fn selectMaterials =
			(
				local lMats = #()
				
				for mat in (getclassinstances missing_texturemap) do appendIfUnique lMats mat
				
				for nMat = 1 to lMats.count where nMat <= meditMaterials.count do setMeditMaterial nMat lMats[nMat]
			)
			
		);)(),
		
		(struct s( tName = "Bercon Noise",
			
			tTooltip = "清理思维粒子TP 变化贴图按照颜色, Vray-材质 和 多维子材质.",
			
			fn autocount =
			(
				if berconnoise != undefined
					then (getclassinstances berconnoise).count
					else "-"
			),
			
			fn clean =
			(
				if berconnoise != undefined then 
				(
					local lMaps = makeUniqueArray (getclassinstances berconnoise)
					
					for map in lMaps do
					(
						local lDeps = refs.dependents map
						local parent = lDeps[1]
						local replaceMap = noise \
							color1:map.color1 \
							color2:map.color2 \
							map1:map.map1 \
							map2:map.map2 \
							map1Enabled:map.map1Enabled \
							map2Enabled:map.map2Enabled \
							size:map.size \
							phase:map.phase \
							levels:map.levels \
							thresholdLow:map.thresholdLow \
							thresholdHigh:map.thresholdHigh \
							coords:map.coords \
							output:map.output
						
						if map.fractalType <= 2 then replaceMap.type = map.fractalType
						
						case classof parent of
						(
							ColorCorrect: parent.src_tex = replaceMap
							VrayMtl: parent.texmap_diffuse = replaceMap
							Standardmaterial: parent.diffuseMap = replaceMap
						)
					)
				)
			),
			
			fn selectObjects =
			(
				if berconnoise != undefined then 
				(
					local lObjs = #()
					for mat in (getclassinstances berconnoise) do
					(
						for obj in (refs.dependents mat) where classof (superclassof obj) == node do appendIfUnique lObjs obj
					)
					
					select lObjs
					local bRefreshSel = false
					for obj in selection do
					(
						if isGroupHead obj and not isOpenGroupHead obj then
						(
							bRefreshSel = true
							setGroupOpen obj true
						)
					)
					if bRefreshSel then select lObjs
				)
			),
			
			fn selectMaterials =
			(
				if berconnoise != undefined then 
				(
					local lMats = #()
					
					for mat in (getclassinstances berconnoise) do appendIfUnique lMats mat
					
					for nMat = 1 to lMats.count where nMat <= meditMaterials.count do setMeditMaterial nMat lMats[nMat]
				)
			)
			
		);)(),
		
		(struct s( tName = "TP思维粒子 变化贴图",
			
			tTooltip = "清理思维粒子TP 变化贴图按照颜色, Vray-材质 和 多维子材质.",
			
			fn autocount =
			(
				if tp_variation_texmap != undefined
					then (getclassinstances tp_variation_texmap).count
					else "-"
			),
			
			fn clean =
			(
				if tp_variation_texmap != undefined then 
				(
					local lMaps = makeUniqueArray (getclassinstances tp_variation_texmap)
					
					-- 
					for map in lMaps do
					(
						local lDeps = refs.dependents map
						local parent = lDeps[1]
						
						case classof parent of
						(
							ColorCorrect:
							(
								parent.src_color = parent.src_tex.color
								parent.src_tex = undefined
							)
							VrayMtl:
							(
								parent.diffuse = parent.texmap_diffuse.color
								parent.texmap_diffuse = undefined
							)
							Standardmaterial:
							(
								parent.diffuse = parent.diffuseMap.color
								parent.diffuseMap = undefined
							)
						)
					)
				)
			),
			
			fn selectObjects =
			(
				if tp_variation_texmap != undefined then 
				(
					local lObjs = #()
					for mat in (getclassinstances tp_variation_texmap) do
					(
						for obj in (refs.dependents mat) where classof (superclassof obj) == node do appendIfUnique lObjs obj
					)
					
					select lObjs
					local bRefreshSel = false
					for obj in selection do
					(
						if isGroupHead obj and not isOpenGroupHead obj then
						(
							bRefreshSel = true
							setGroupOpen obj true
						)
					)
					if bRefreshSel then select lObjs
				)
			),
			
			fn selectMaterials =
			(
				if tp_variation_texmap != undefined then 
				(
					local lMats = #()
					
					for mat in (getclassinstances tp_variation_texmap) do appendIfUnique lMats mat
					
					for nMat = 1 to lMats.count where nMat <= meditMaterials.count do setMeditMaterial nMat lMats[nMat]
				)
			)
			
		);)(),
		
		(struct s( tName = "Vray材质 灯光材质的GI",
			
			tTooltip = "VR灯光材质必须通过VR包裹材质生成 GI, 否则<外部引用>可能出错.",
			
			fn autocount =
			(
				local nCount = 0
				
				local lLightMtlTvps = getclassinstances VRayLightMtl astrackviewpick:true
				for lightMtlTvp in lLightMtlTvps do
				(
					local lightMtl = lightMtlTvp.anim
					local parent = lightMtlTvp.client
					local wrapperMtl = vraymtlwrapper generateGI:false basemtl:lightMtl
					
					if (classof (superclassof parent)) == node then
					(
						nCount += 1
					)
					
					else if classof parent == multimaterial then
					(
						for nMat = 1 to parent.MaterialList.count do
						(
							if parent.MaterialList[nMat] == lightMtl then nCount += 1
						)
					)
					
					else if classof parent == vraymtlwrapper and parent.generateGI == true then
					(
						nCount += 1
					)
				)
				
				nCount
			),
			
			fn clean =
			(
				-- lights need to be in wrappers
				local lLightMtlTvps = getclassinstances VRayLightMtl astrackviewpick:true
				for lightMtlTvp in lLightMtlTvps do
				(
					local lightMtl = lightMtlTvp.anim
					local parent = lightMtlTvp.client
					local wrapperMtl = vraymtlwrapper generateGI:false basemtl:lightMtl
					
					if (classof (superclassof parent)) == node then
					(
						parent.mat = wrapperMtl
					)
					
					else if classof parent == multimaterial then
					(
						for nMat = 1 to parent.MaterialList.count do
						(
							if parent.MaterialList[nMat] == lightMtl then parent.MaterialList[nMat] = wrapperMtl
						)
					)
					
					else if classof parent == vraymtlwrapper then
					(
						parent.generateGI = false
					)
				)
			)
			
		);)(),
		
		(struct s( tName = "Vray材质 ClampTex 禁用",
			
			tTooltip = "常常灯光计算的准备动作会杀死效果图.",
			
			fn autocount =
			(
				local nCount = 0
				
				for mtl in getclassinstances vraymtl where mtl.option_clampTextures != true do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				for mtl in getclassinstances vraymtl where mtl.option_clampTextures != true do mtl.option_clampTextures = true
			)
			
		);)(),
		
		(struct s( tName = "贴图纹理 过滤",
			
			tTooltip = "过滤锥体的所有位图,如果没有一个位图属于一片叶子或模型+模型的书.",
			
			fn autocount =
			(
				local nCount = 0
				
				for curr_map in (getclassinstances bitmaptexture) do
				(
					if
						curr_map.filename != undefined and
						curr_map.filtering != 2 and
						(
							matchpattern (getfilenamefile curr_map.filename) pattern:"*leaf*" or
							matchpattern (curr_map.filename) pattern:"*model+model*maps_high*" or
							matchpattern (getfilenamefile curr_map.filename) pattern:"*flower*"
						)
					then nCount += 1
					else if(curr_map.filtering == 1) then
						nCount += 1
				)
				
				nCount
			),
			
			fn clean =
			(
				for curr_map in (getclassinstances bitmaptexture) do
				(
					-- auto-detect plants
					if
						curr_map.filename != undefined and
						(
							matchpattern (getfilenamefile curr_map.filename) pattern:"*leaf*" or
							matchpattern (curr_map.filename) pattern:"*model+model*maps_high*" or
							matchpattern (getfilenamefile curr_map.filename) pattern:"*flower*"
						)
					then curr_map.filtering = 2
					else curr_map.filtering = 0
				)
			)
			
		);)(),
	
		(struct s( tName = "Vay材质 折射/反射 细分 > 8",
			
			tTooltip = "过高的VR材质细分，会影响你的渲染速度.",
			
			fn autocount =
			(
				local nCount = 0
				
				local nMaxSubdivs = 8
				
				for mat in (getclassinstances VrayMtl)
					where mat.reflection_subdivs > nMaxSubdivs or mat.refraction_subdivs > nMaxSubdivs
					do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				local nMaxSubdivs = 8
				
				for mat in (getclassinstances VrayMtl) do
				(
					if mat.reflection_subdivs > nMaxSubdivs then mat.reflection_subdivs = nMaxSubdivs
					if mat.refraction_subdivs > nMaxSubdivs then mat.refraction_subdivs = nMaxSubdivs
				)
			)
			
		);)(),
			
		(struct s( tName = "Vay材质 折射/反射 细分 > 12",
			
			tTooltip = "过高的VR材质细分，会影响你的渲染速度.",
			
			fn autocount =
			(
				local nCount = 0
				
				local nMaxSubdivs = 12
				
				for mat in (getclassinstances VrayMtl)
					where mat.reflection_subdivs > nMaxSubdivs or mat.refraction_subdivs > nMaxSubdivs
					do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				local nMaxSubdivs = 12
				
				for mat in (getclassinstances VrayMtl) do
				(
					if mat.reflection_subdivs > nMaxSubdivs then mat.reflection_subdivs = nMaxSubdivs
					if mat.refraction_subdivs > nMaxSubdivs then mat.refraction_subdivs = nMaxSubdivs
				)
			)
			
		);)(),
		
		(struct s( tName = "Vay材质 折射/反射 细分 > 16",
			
			tTooltip = "过高的VR材质细分，会影响你的渲染速度.",
			
			fn autocount =
			(
				local nCount = 0
				
				local nMaxSubdivs = 16
				
				for mat in (getclassinstances VrayMtl)
					where mat.reflection_subdivs > nMaxSubdivs or mat.refraction_subdivs > nMaxSubdivs
					do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				local nMaxSubdivs = 16
				
				for mat in (getclassinstances VrayMtl) do
				(
					if mat.reflection_subdivs > nMaxSubdivs then mat.reflection_subdivs = nMaxSubdivs
					if mat.refraction_subdivs > nMaxSubdivs then mat.refraction_subdivs = nMaxSubdivs
				)
			)
			
		);)(),
			

		(struct s( tName = "Vay材质 折射/反射 细分 > 24",
			
			tTooltip = "过高的VR材质细分，会影响你的渲染速度.",
			
			fn autocount =
			(
				local nCount = 0
				
				local nMaxSubdivs = 24
				
				for mat in (getclassinstances VrayMtl)
					where mat.reflection_subdivs > nMaxSubdivs or mat.refraction_subdivs > nMaxSubdivs
					do nCount += 1
				
				nCount
			),
			
			fn clean =
			(
				local nMaxSubdivs = 24
				
				for mat in (getclassinstances VrayMtl) do
				(
					if mat.reflection_subdivs > nMaxSubdivs then mat.reflection_subdivs = nMaxSubdivs
					if mat.refraction_subdivs > nMaxSubdivs then mat.refraction_subdivs = nMaxSubdivs
				)
			)
			
		);)(),
			

		(struct s( tName = "Vay材质 折射/反射 细分 > 32",
			
			tTooltip = "过高的VR材质细分，会影响你的渲染速度.",
			
			fn autocount =
			(
				local nCount = 0
				
				local nMaxSubdivs = 32
				
				for mat in (getclassinstances VrayMtl)
					where mat.reflection_subdivs > nMaxSubdivs or mat.refraction_subdivs > nMaxSubdivs
					do nCount += 1
				
				nCount
			),

			fn clean =
			(
				local nMaxSubdivs = 32
				
				for mat in (getclassinstances VrayMtl) do
				(
					if mat.reflection_subdivs > nMaxSubdivs then mat.reflection_subdivs = nMaxSubdivs
					if mat.refraction_subdivs > nMaxSubdivs then mat.refraction_subdivs = nMaxSubdivs
				)
			)
			
		);)()
	)
	

	function getTooltip task =
	(
		local t = task.tTooltip
		
		t = substituteString t "\\" ""
		t = substituteString t "\t" ""
		t = substituteString t "\n" ""
	)
	
	local tCode = ""
	tCode += "global cCleanerRollout = rollout cCleanerRollout \"Cleaner场景清理器 v8\" ("
	tCode += "label cLabCount \"计数:\" width:160 height:24 pos:[200,7];"
	tCode += "label cLabSelect \"从场景中选择:\" width:160 height:24 pos:[265,7];"
		
		-- vertical control position
		local nPosY = 5
		
		-- add tasks
		for nTask = 1 to lTasks_4648134686.count do
		(
			local task = lTasks_4648134686[nTask]
			local tTask = nTask as string
			
			if not (vray == undefined and matchpattern task.tName pattern:"*vray*") then
			(
				-- groups
				if matchpattern task.tName pattern:"--*--*" then
				(
					local tGroup = (filterstring task.tName "--")[1]
					task.tName = (filterstring task.tName "--")[2]
					
					tCode += "label cLab_group"+ nPosY as string +" \""+ tGroup +":\" width:100 height:24 pos:[10,"+ (nPosY+2) as string +"];"
					nPosY += 24
				)
				
				-- clean
				tCode += "button cBut_clean"+ tTask +" \""+ task.tName +"\" tooltip:\""+ getTooltip task +"\" width:160 height:24 pos:[5,"+ nPosY as string +"];"
				tCode += "on cBut_clean"+ tTask +" pressed do ("
				tCode += "lTasks_4648134686["+ tTask +"].clean();"
				tCode += "cCleanerRollout.cLab"+ tTask +".text = lTasks_4648134686["+ tTask +"].autocount() as string;"
				tCode += ");"
				
				if try (task.usercount; true) catch false then
				(
					-- usercount
					tCode += "button cBut_count"+ tTask +" \"计数\" tooltip:\"This tasks takes longer to count, so you have to start the count manually.\" width:50 height:24 pos:[189,"+ nPosY as string +"];"
					tCode += "on cBut_count"+ tTask +" pressed do ("
					tCode += "cCleanerRollout.cBut_count"+ tTask +".text = lTasks_4648134686["+ tTask +"].usercount() as string;"
					tCode += ");"
				)
				else
				(
					-- autocount
					tCode += "label cLab"+ tTask +" \"计数..\" pos:[190,"+ (nPosY+5) as string +"];"
				)
				
				-- objects
				if try (task.selectObjects; true) catch false then
				(
					tCode += "button cBut_objects"+ tTask +" \"对象\" tooltip:\"select objects (opens groups)\" width:50 height:24 pos:[260,"+ nPosY as string +"];"
					tCode += "on cBut_objects"+ tTask +" pressed do lTasks_4648134686["+ tTask +"].selectObjects();"
				)
				
				-- materials
				if try (task.selectMaterials; true) catch false then
				(
					tCode += "button cBut_materials"+ tTask +" \"材质\" tooltip:\"select materials (in materialeditor)\" width:50 height:24 pos:[315,"+ nPosY as string +"];"
					tCode += "on cBut_materials"+ tTask +" pressed do lTasks_4648134686["+ tTask +"].selectMaterials();"
				)
				
				nPosY += 28
			)
		)
		
		-- on rollout open
		tCode += "on cCleanerRollout open do ("
		for nTask = 1 to lTasks_4648134686.count do
		(
			local task = lTasks_4648134686[nTask]
			
			if not (vray == undefined and matchpattern task.tName pattern:"*vray*") then
			(
				local tTask = nTask as string
				tCode += "try cLab"+ tTask +".text = lTasks_4648134686["+ tTask +"].autocount() as string; catch()"
			)
			
			if hasProperty task "nMaxSubdivs" then print "!"
		)
		tCode += ");"
		
	tCode += ")"
	local cCleanerRollout = execute tCode
	createDialog cCleanerRollout 370 (nPosY+2)
)