ProtoEnum = {
    cmd_proto = { },
    game_proto = { }
}

ProtoEnum.cmd_proto.Cmd = {
	NONE = 0;
	PING = 1;

    --Version
	CLIENTVER_NTF = 10; 			-- 客户端版本号

	--Cmd
	GM_COMMAND = 20; 				-- 游戏内部指令

	--Login
	LOGIN = 50; 					-- 登录游戏
	CREATE_ROLE = 51; 				-- 创建角色
	SET_ROLENAME = 52; 				-- 设置角色名字
	ROLEINFO_NTF = 53; 				-- 角色信息
	LOGIN_END_NTF = 54; 			-- 登录结束

	--Scene
	SCENE_LOAD = 60;				-- 加载场景
	SCENE_ROLE = 61; 				-- 场景玩家
	SCENE_ROLE_NTF = 62;			-- 广播玩家数据
	SCENE_NPC_NTF = 63;				-- 广播NPC

	--Mail
	MAIL_LIST_NTF = 80; 			-- 邮件列表
	MAIL_OPEN = 81;
	MAIL_ATCH = 82;
	MAIL_DEL = 83;

}

ProtoEnum.cmd_proto.ErrorCode = {
	SUCCESS = 0;

	SERVICE_CLOSED      = 1;    	-- 服务关闭
	SERVICE_UNAVAILABLE = 2;    	-- 服务繁忙
	SERVICE_DBMAX       = 3;    	-- 禁止新用户进入
	VERSION_INVALID     = 4;    	-- 版本号不对
	PARAM_INVALID       = 5;    	-- 参数不合法
	OPREATE_FAST        = 6;    	-- 操作太快了
}