extends Node

const TITLE_CHANNEL = 452329
const PREFIX = "[TitlesPlus] "

onready var title_api = get_node("/root/TitleAPI")

func sendTitle(custom_title):
	update_title(custom_title)

func update_title_packet(sender, packet):
	var title = packet.get("title", null)
	if title == null:
		print(PREFIX + "oh no invalid packet !>>! >! !>! ?!? !? ?!")
		return

	title_api.register_title(sender, title)

func _inject(hud):
	print(PREFIX + "I have injected to the hud  - 1")
	var main: Node = hud.get_child(0)
	if main == null: return

	var menu = load("res://mods/TitlesPlus/titlesplus_menu.tscn").instance()

	if main:
		print(PREFIX + "Main does exist")
		if menu:
			print(PREFIX + "Menu does exist")
			main.add_child(menu)
		else:
			print(PREFIX + "Menu doesn't exist")
	else:
		print(PREFIX + "Menu doesn't exist")	

	print(PREFIX + "I have injected to the hud  - 2")

func update_title(title):
	print(PREFIX + "I have sent a packet")
	Network._send_P2P_Packet({"type": "update_title", "title": title}, "peers", 2, TITLE_CHANNEL)
