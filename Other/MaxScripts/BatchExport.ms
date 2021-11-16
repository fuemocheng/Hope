macroScript BatchExport
ButtonText:"批处理工具"
category:"批处理工具"     
tooltip:"批处理工具"
 
(     
	rollout BatchExportDialog "批处理工具" width:600 height:(700)
	(
		subRollout subRL "subRL" pos:[0,0] 
	)
 
	global batchExportAni
	global batchExportSkin
	
	--Animation处理
	rollout batchExportAni "导出Animation"     
	(  
		--------------------------------------------------- 定义变量
		
		--文件名列表
		local fileList = #()
		

		--------------------------------------------------- UI 元素
		
		edittext edittext_rootname "根节点名称:" fieldWidth:150 labelOnTop:false readOnly:false  align:#left orient:#vertical
		
		label labEmptyLine1 "" align:#left orient:#vertical
		
		button button_import "导入 .max 文件" width:90 align:#left orient:#vertical
		
		label labEmptyLine2 "" align:#left orient:#vertical

		listbox listboxFileName "导入文件目录:" items:(for o in #() collect o.name)
		
		button button_delSel "删除选中" width:90 align:#left orient:#vertical
		button button_delAll "删除全部" width:90 align:#left orient:#vertical
		
		label labEmptyLine3 "" align:#left orient:#vertical
		
		button button_selFolder "选择导出文件夹" width:90 align:#left orient:#vertical
		edittext edittext_exportFolderPath "导出目录:" fieldWidth:470 labelOnTop:false readOnly:true
		
		label labEmptyLine4 "" align:#left orient:#vertical
		
		button button_export "导出" width:120 height:40  align:#right orient:#vertical
		
		
		--------------------------------------------------- 自定义函数
		
		fn replaceSub str1 str2 str3=(
			while ((i=findstring str1 str2) != undefined) do
			(
				str1 = replace str1 i str2.count str3
			)
			return str1
		)
		
		-- 打开文件导入文件夹
		fn getnames Title FilterName =(
			dialog = DotNetObject"System.Windows.Forms.OpenFileDialog"
			dialog.title = Title
			dialog.filter = FilterName
			dialog.Multiselect = true
			OpenFileResult = dialog.ShowDialog()
			if OpenFileResult.Equals OpenFileResult.OK then
			(
				dialog.FileNames
			)
			else
				Undefined
		)
		
		fn init=
		(
			--Default
			edittext_rootname.text = "Root"
			
			--重置
			fileList = #()
		)
		
		
		--------------------------------------------------- Event
	 
		on batchExportAni open do
		(
			init()
		)
		
		on button_import pressed do
		(
			print("press button_import")
			
			file_names= getnames "选择导入文件" "MAX (*.max)|*.max"
			if file_names != undefined do
			(
				for f in file_names do
				(
					--print(f)
					appendIfUnique fileList f
				)
				
				--清空 listboxFileName
				listboxFileName.items = #()
				
				for f in fileList do
				(
					listboxFileName.items = append listboxFileName.items (f)
				)
			)
		)
		
		on button_delSel pressed do
		(
			print("press button_delSel")
			if listboxFileName.items.count > 0 and listboxFileName.selection > 0 do
			(
				local selIdx = listboxFileName.selection
				listboxFileName.items = deleteItem listboxFileName.items selIdx 
				fileList = deleteItem fileList selIdx 
			)
		)
		
		on button_delAll pressed do
		(
			print("press button_delAll")
			listboxFileName.items = #()
			fileList = #()
		)
		
		on button_selFolder pressed do
		(
			print("press button_selectFolder")
			dialog = DotNetObject"System.Windows.Forms.FolderBrowserDialog"
			OpenFileResult = dialog.ShowDialog()
			if OpenFileResult.Equals OpenFileResult.OK then
			(
				edittext_exportFolderPath.text = dialog.SelectedPath
			)
		)
		
		on button_export pressed do
		(
			print("press button_export")
			
			if(listboxFileName.items.count == 0) then
			(
				messagebox ("未选择导出文件!!!\n\n") title:"警告"
			)
			else if(edittext_exportFolderPath.text == "") then
			(
				messagebox ("未选择导出路径!!!\n\n") title:"警告"
			)
			else
			(
				-- 依次导出，单次处理每个文件
				for f in fileList do
				(	
					--resetMaxFile #noprompt
					print("Loadmaxfile:" + f)
					
					--import maxfile
					local filepath = f
					--local val = undefined
					loadmaxfile filepath missingExtFilesList:&val missingExtFilesAction:#logmsg quiet:true					
					--val	--print Error	
					
					--骨骼检查
					--1.根节点命名是否正确，不正确则提醒修改并退出(暂定不退出，直接批处理修改)
					if(rootNode.children.count > 0) then
					(
						childRoot = rootNode.children[1]
						if(childRoot !=null and childRoot.name != edittext_rootname.text) then
						(
							print("Change Root Name ! The original root name is " + childRoot.name + " . The current root name is " + edittext_rootname.text)
							childRoot.name = edittext_rootname.text
						)
					)
					
					
					--2.检查多余节点(命名乱码的节点) 目前只遍历一层节点
					for child in rootNode.children do 
					(
						--print(child.name)
						
						--正则表达式
						pattern = "[^a-zA-Z0-9_\-\.\@\(\) ]"	--除了这些字符外是否有其他字符	乱码字符
						rgx = dotNetObject "System.Text.RegularExpressions.Regex" pattern
						bisfound = rgx.IsMatch child.name
						if(bisfound) then
						(
							print("Delete " + child.name)
							delete child
						)
					)
					
					--全选
					max select all
					
					--设置一些导出参数
					max display mode
					hideByCategory.geometry = true
					pluginManager.loadClass FBXEXPORTER
					FBXExporterSetParam "Export|IncludeGrp|Geometry|GeomNormalPerPoly" true
					FBXExporterSetParam "Export|IncludeGrp|Geometry|SmoothMesh" true
					FBXExporterSetParam "Export|IncludeGrp|Geometry|MaxBoneAsBone" true
					FBXExporterSetParam "Export|IncludeGrp|Geometry|PreserveEdgeOrientation" true
					
					FBXExporterSetParam "Export|IncludeGrp|Animation" true
					FBXExporterSetParam "Export|IncludeGrp|Animation|BakeComplexAnimation" true
					FBXExporterSetParam "Export|IncludeGrp|Animation|BakeComplexAnimation|BakeFrameStart" 0
					FBXExporterSetParam "Export|IncludeGrp|Animation|BakeComplexAnimation|BakeFrameEnd" 90
					FBXExporterSetParam "Export|IncludeGrp|Animation|BakeComplexAnimation|BakeFrameStep" 1
					FBXExporterSetParam "Export|IncludeGrp|Animation|BakeComplexAnimation|ResampleAnimationCurves" false
				
					exportName = filenameFromPath filepath
					exportName = substring exportName 1 (exportName.count - 4) 	---去掉 ".max"
					exportPath = edittext_exportFolderPath.text + "/" +  exportName;
					print("ExportPath:" + exportPath)
					
					exportfile exportPath  #noPrompt selectedOnly:true using:FBXEXP 	--print exporterPlugin.classes to see all formates
					
					
					--保存更改
					--max file save
				)
				
				messagebox ("完成!!!") title:"提示"
			)
		)
	)
	
	
	--Skin处理
	rollout batchExportSkin "导出蒙皮"     
	(  
		--------------------------------------------------- 定义变量
		
		--文件名列表
		local fileList = #()
		

		--------------------------------------------------- UI 元素
		
		edittext edittext_rootname "根节点名称:" fieldWidth:150 labelOnTop:false readOnly:false  align:#left orient:#vertical
		
		label labEmptyLine1 "" align:#left orient:#vertical
		
		button button_import "导入 .max 文件" width:90 align:#left orient:#vertical
		
		label labEmptyLine2 "" align:#left orient:#vertical

		listbox listboxFileName "导入文件目录:" items:(for o in #() collect o.name)
		
		button button_delSel "删除选中" width:90 align:#left orient:#vertical
		button button_delAll "删除全部" width:90 align:#left orient:#vertical
		
		label labEmptyLine3 "" align:#left orient:#vertical
		
		button button_selFolder "选择导出文件夹" width:90 align:#left orient:#vertical
		edittext edittext_exportFolderPath "导出目录:" fieldWidth:470 labelOnTop:false readOnly:true
		
		label labEmptyLine4 "" align:#left orient:#vertical
		
		button button_export "导出" width:120 height:40  align:#right orient:#vertical
		
		
		--------------------------------------------------- 自定义函数
		
		fn replaceSub str1 str2 str3=(
			while ((i=findstring str1 str2) != undefined) do
			(
				str1 = replace str1 i str2.count str3
			)
			return str1
		)
		
		-- 打开文件导入文件夹
		fn getnames Title FilterName =(
			dialog = DotNetObject"System.Windows.Forms.OpenFileDialog"
			dialog.title = Title
			dialog.filter = FilterName
			dialog.Multiselect = true
			OpenFileResult = dialog.ShowDialog()
			if OpenFileResult.Equals OpenFileResult.OK then
			(
				dialog.FileNames
			)
			else
				Undefined
		)
		
		fn init=
		(
			--Default
			edittext_rootname.text = "Root"
			
			--重置
			fileList = #()
		)
		
		
		--------------------------------------------------- Event
	 
		on batchExportSkin open do
		(
			init()
		)
		
		on button_import pressed do
		(
			print("press button_import2")
			
			file_names= getnames "选择导入文件" "MAX (*.max)|*.max"
			if file_names != undefined do
			(
				for f in file_names do
				(
					--print(f)
					appendIfUnique fileList f
				)
				
				--清空 listboxFileName
				listboxFileName.items = #()
				
				for f in fileList do
				(
					listboxFileName.items = append listboxFileName.items (f)
				)
			)
		)
		
		on button_delSel pressed do
		(
			print("press button_delSel2")
			if listboxFileName.items.count > 0 and listboxFileName.selection > 0 do
			(
				local selIdx = listboxFileName.selection
				listboxFileName.items = deleteItem listboxFileName.items selIdx 
				fileList = deleteItem fileList selIdx 
			)
		)
		
		on button_delAll pressed do
		(
			print("press button_delAll2")
			listboxFileName.items = #()
			fileList = #()
		)
		
		on button_selFolder pressed do
		(
			print("press button_selectFolder2")
			dialog = DotNetObject"System.Windows.Forms.FolderBrowserDialog"
			OpenFileResult = dialog.ShowDialog()
			if OpenFileResult.Equals OpenFileResult.OK then
			(
				edittext_exportFolderPath.text = dialog.SelectedPath
			)
		)
		
		on button_export pressed do
		(
			print("press button_export2")
			
			if(listboxFileName.items.count == 0) then
			(
				messagebox ("未选择导出文件!!!\n\n") title:"警告"
			)
			else if(edittext_exportFolderPath.text == "") then
			(
				messagebox ("未选择导出路径!!!\n\n") title:"警告"
			)
			else
			(
				-- 依次导出，单次处理每个文件
				for f in fileList do
				(	
					--resetMaxFile #noprompt
					print("Loadmaxfile:" + f)
				
					--import maxfile
					local filepath = f
					local val = undefined
					loadmaxfile filepath missingExtFilesList:&val missingExtFilesAction:#logmsg quiet:true		
					--val	--print Error
					
					--骨骼检查
					--1.根节点命名是否正确，不正确则提醒修改并退出(暂定不退出，直接批处理修改)
					if(rootNode.children.count > 0) then
					(
						childRoot = rootNode.children[1]
						if(childRoot !=null and childRoot.name != edittext_rootname.text) then
						(
							print("Change Root Name ! The original root name is " + childRoot.name + " . The current root name is " + edittext_rootname.text)
							childRoot.name = edittext_rootname.text
						)
					)
					
					
					--2.检查多余节点(命名乱码的节点) 目前只遍历一层节点
					for child in rootNode.children do 
					(
						--print(child.name)
						
						--正则表达式
						pattern = "[^a-zA-Z0-9_\-\.\@\(\) ]"	--除了这些字符外是否有其他字符	乱码字符
						rgx = dotNetObject "System.Text.RegularExpressions.Regex" pattern
						bisfound = rgx.IsMatch child.name
						if(bisfound) then
						(
							print("Delete " + child.name)
							delete child
						)
					)
					
					--全选
					max select all
				
					--设置一些导出参数
					max display mode
					hideByCategory.geometry = false
					pluginManager.loadClass FBXEXPORTER
					FBXExporterSetParam "Export|IncludeGrp|Geometry|GeomNormalPerPoly" true
					FBXExporterSetParam "Export|IncludeGrp|Geometry|SmoothMesh" true
					FBXExporterSetParam "Export|IncludeGrp|Geometry|MaxBoneAsBone" true
					FBXExporterSetParam "Export|IncludeGrp|Geometry|PreserveEdgeOrientation" true
					FBXExporterSetParam "Export|IncludeGrp|Animation" false

					exportName = filenameFromPath filepath
					exportName = substring exportName 1 (exportName.count - 4) 	---去掉 ".max"
					exportPath = edittext_exportFolderPath.text + "/" +  exportName;
					print("ExportPath:" + exportPath)
					
					exportfile exportPath  #noPrompt selectedOnly:true using:FBXEXP 	--print exporterPlugin.classes to see all formates
					
					--保存更改
					--max file save
				)
				
				messagebox ("完成!!!") title:"提示"
			)
		)
	)
  
	Createdialog BatchExportDialog bgcolor:black fgcolor:red lockHeight:false lockWidth:false
	addsubrollout BatchExportDialog.subRL batchExportAni rolledup:false
	addsubrollout BatchExportDialog.subRL batchExportSkin rolledup:false
	BatchExportDialog.subRL.width += (600)
	BatchExportDialog.subRL.height += (700)
)