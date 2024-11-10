extends Node

const DEBUG = false
const TITLE_CHANNEL = 452329
const PREFIX = "[TitlesPlus] "

onready var title_api = get_node("/root/TitleAPI")

func sendTitle(custom_title):
	update_title(custom_title)

func update_title_packet(sender, packet):
	var title = packet.get("title", null)
	if title == null:
		print(PREFIX + "Invalid packet received")
		return
	
	if str(title).length() > 32:
		print(PREFIX + "Illegal packet > 32 characters received")
		return

	title_api.register_title(sender, title)

func _inject(hud):
	_print_debug("I have injected to the hud  - 1")
	var main: Node = hud.get_child(0)
	if main == null: return

	var menu = load("res://mods/TitlesPlus/titlesplus_menu.tscn").instance()
	
	if main:
		_print_debug("Main does exist")
		if menu:
			_print_debug("Menu does exist")
			main.add_child(menu)
		else:
			_print_debug("Menu doesn't exist")
	else:
		_print_debug("Menu doesn't exist")

	_print_debug("I have injected to the hud  - 2")

func update_title(title):
	_print_debug("I have sent a packet")
	Network._send_P2P_Packet({"type": "update_title", "title": title}, "peers", 2, Network.CHANNELS.ACTOR_UPDATE)

func _print_debug(message):
	if not DEBUG:
		return
	print(PREFIX + message)
