for obj in geometry do(
	select obj
	convertTomesh obj			--转换为可编辑网格
	max modify mode				--选择修改器
	subobjectlevel = 4			--选择多边形
	max select all				--选择所有对象
	meshOps.unifynormal obj		--Unifies the normals of the selected faces 
								--valid in Face, Polygon, and Element Sub-Object levels.
								--统一所选面的法线
								--在“面”，“多边形”和“元素子对象”层中有效。
	convertTopoly obj			--转换为可编辑多边形
	clearSelection()	
	print obj.name
)