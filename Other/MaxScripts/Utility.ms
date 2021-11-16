utility TAUtil "TA Utility"
(
	rollout bout "TA Utility"
	(
		button aboutMU "Utility Describe" width:90 height:20
		on aboutMU pressed do
		messagebox "Utility Describe!\nVersion 1.0" title:"Utility Describe"
	)
	
	rollout delRollOut "Delete Unused Node"
	(
		group "删除多余节点"
		(
			button deleteBtn "删除"
			button saveBtn "保存"
		)
		
		on deleteBtn pressed do
		(
			print("删除多余节点")
			--检查多余节点(命名乱码的节点) 目前只遍历一层节点
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
		)
		
		on saveBtn pressed do
		(
			print("保存删除之后的文件")
			max file save
		)
	)

	rollout bakeRollOut "Bake Camera"
	(
		group "烘焙摄像机"
		(
			button bakeCameraBtn "烘焙"
		)
		
		on bakeCameraBtn pressed do
		(
			print("烘焙摄像机")
			
			
		)
	)
	
	on TAUtil open do
	(
		addRollout delRollOut
		addRollout bakeRollOut
	)
	
	on TAUtil close do
	(
		removeRollout delRollOut
		removeRollout bakeRollOut
	)
	
)