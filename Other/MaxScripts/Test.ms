-- clearSelection()
-- for node in geometry do
-- (
-- 	--Name
-- 	local showName = node.name
-- 	if node.ishidden then showName = showName + " (被隐藏) "

-- 	-- nodeClass = classof node
-- 	-- print nodeClass
-- 	if isBoneOnly node then
-- 	(
-- 		print showName
-- 	)

-- 	-- if (canconvertto node Editable_Mesh) then
-- 	-- (
-- 	-- 	print showName
-- 	-- )
-- )

-- bonesAll = for b in geometry where isKindof b.baseobject BoneGeometry collect b
-- for node in bonesAll do
-- (
-- 	local showName = node.name
-- 	if node.ishidden then showName = showName + " (被隐藏) "
-- 	print showName
-- )


-- count = 0
-- for obj in geometry do(
-- 	select obj
-- 	convertTomesh obj			--转换为可编辑网格

-- 	max modify mode				--switch to modify panel
-- 	subObjectLevel = 4 			--switch to Face level

-- 	--<bool>PolyToolsSelect.NumericFace <integer>sides <integer>type <bool>withinSel 
-- 	--选中所有面数大于4的面
-- 	PolyToolsSelect.NumericFace 4 1 false

-- 	sf = for i in obj.selectedFaces collect i.index
-- 	--sf = getFaceSelection obj
-- 	print obj.name
-- 	print sf as string
-- 	print sf.count
-- 	count = count + sf.count
-- 	convertTopoly obj			--转换为可编辑多边形
-- 	clearSelection()
-- )
-- print count

-- count = 0
-- for obj in geometry do(
-- 	convertTopoly obj			--转换为可编辑多边形
-- 	local num_faces = polyop.getNumFaces obj
-- 	for f = 1 to num_faces do
-- 	(
-- 		local num_face_verts = polyop.getFaceDeg obj f
-- 		if num_face_verts > 4 do count = count + 1
-- 	)

-- )
-- print count

max select all
for obj in selection do (
	print obj.name
	nodeClass = classof obj
	print nodeClass
	-- print obj.position
	-- print obj.rotation
	-- print obj.mesh.numfaces

	print (isKindof obj.baseobject Particle_View)
) 



















