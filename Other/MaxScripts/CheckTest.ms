
--Òþ²Ø¹Ç÷ÀºÍ¸¨Öú¶ÔÏó
max display mode
hideByCategory.geometry = false
hideByCategory.shapes = true
hideByCategory.lights = true
hideByCategory.cameras = true
hideByCategory.helpers = true
hideByCategory.spacewarps = true
hideByCategory.Particles = true
hideByCategory.Bones = true

max select all

numTotalFaces = 0
numTotalVerts = 0

nodes = for node in selection where canconvertto node Editable_Mesh collect node 

cnt = 0

for node in nodes do
(
	nodeClass = classof node
	numFaces = if nodeClass == Editable_Poly or nodeClass == PolyMeshObject then node.numfaces else node.mesh.numfaces
	numVerts = if nodeClass == Editable_Poly or nodeClass == PolyMeshObject then node.numverts else node.mesh.numverts
	
	format "name:%  numfaces:%  numverts:%  class:%\n" node.name numFaces numVerts nodeClass
	
	numTotalFaces = numTotalFaces + numFaces
	numTotalVerts = numTotalVerts + numVerts
	
	if node.material != null then
		print node.material.name
	
	cnt = cnt + 1
)

print numTotalFaces
print numTotalVerts
print cnt

max select none

max display mode
hideByCategory.geometry = false
hideByCategory.shapes = false
hideByCategory.lights = false
hideByCategory.cameras = false
hideByCategory.helpers = false
hideByCategory.spacewarps = false
hideByCategory.Particles = false
hideByCategory.Bones = false

