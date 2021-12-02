--------------------------------------------------------------------------------
--      Copyright (c) 2021 - 2022 , MattXu
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

print('TestSort')

table_demo = {
    [1] = {
        creat_time = 11,
        quality = 1,
        sum = 4,
    },
    [2] = {
        creat_time = 12,
        quality = 1,
        sum = 2,
    },
    [3] = {
        creat_time = 13,
        quality = 1,
        sum = 1,
    },
    [4] = {
        creat_time = 14,
        quality = 1,
        sum = 5,
    },
    [5] = {
        creat_time = 15,
        quality = 1,
        sum = 7,
    },
    [6] = {
        creat_time = 16,
        quality = 1,
        sum = 7,
    },
}

function sortFunc(a,b)
	--降序
	--return a.sum > b.sum
	--升序
	return a.sum < b.sum

	--if a.sum < b.sum then
	--	return true
	--end
	--return false
end

table.sort(table_demo, sortFunc)

for i=1,#table_demo do
	print("---------", i, table_demo[i].sum)
end