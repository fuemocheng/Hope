/*//////////   SCRIPT:   Hide|Unhide Skeleton v1.0   //////////*/
/*//////////   WRITTEN BY:   Branko Zivkovic   //////////*/
/*//////////   EMAIL:   barigazy@hotmail.com   //////////*/
try(destroyDialog ::skeletonRoll)catch()
rollout skeletonRoll "Hide|Unhide"
(
	button btn_hide "Hide" pos:[5,5] width:50 height:18
	button btn_unhide "Unhide" pos:[60,5] width:50 height:18
	groupBox group1 "Skeleton Types:" pos:[5,25] width:105 height:100 
	checkbox cb_bone "Bones" pos:[10,45] width:50 height:18 checked:on
	checkbox cb_bip "Biped" pos:[10,65] width:50 height:18 checked:on
	checkbox cb_cat "CATbone" pos:[10,85] width:65 height:18 checked:on
	checkbox cb_hub "CAThub" pos:[10,105] width:60 height:18 checked:on
	fn collectClasses optArr:#(cb_bone,cb_bip,cb_cat,cb_hub) = 
	( 
		for c in 1 to optArr.count where optArr[c].checked collect
		(
			case c of
			(
				(1): BoneGeometry
				(2): Biped_Object
				(3): CATBone
				(4): HubObject				
			)
		)
	)
	fn hide_unhide_Skeleton state: =
	(
		local skeletonClasses = collectClasses()
		(for b in geometry where findItem skeletonClasses (classof b.baseobject) != 0 collect b).ishidden = state
	)
	on btn_hide pressed do hide_unhide_Skeleton state:on
	on btn_unhide pressed do hide_unhide_Skeleton state:off
)
createDialog skeletonRoll 115 130 10 110 style:#(#style_titlebar, #style_sysmenu, #style_toolwindow)