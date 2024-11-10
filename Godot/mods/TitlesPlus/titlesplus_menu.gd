extends Node

onready var titles_plus = get_node_or_null("/root/TitlesPlus")
onready var title_api = get_node_or_null("/root/TitleAPI")

onready var anims = $Anims

var isOpen = false

func _ready():
	_sound()

func _sound():
	$Open.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	$Panel/Exit.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	$Panel/Settings.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	$Panel/Submit.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	
	$Panel/Submit.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])
	$Panel/Exit.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])
	$Panel/Settings.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])
	$Open.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])

func _on_Exit_pressed():
	#$".".visible = false
	anims.play("CloseTitles")
	isOpen = false
	$Open.disabled = false
	print("Exited")

func _on_Submit_pressed():
	if titles_plus != null:
		var custom_title = $Panel/CustomTitle.text
		
		title_api.register_title(Network.STEAM_ID, custom_title)
		titles_plus.sendTitle(custom_title)
		
		PlayerData._send_notification("Title set to: " + custom_title)
		print("Submited!")
	else:
		print("Oh")

func _on_Open_pressed():
	if not isOpen:
		$Open.disabled = true
		isOpen = true
		anims.play("OpenTitles")

func _on_Settings_mouse_entered():
	$Panel/Settings/HoverOn.visible = true

func _on_Settings_mouse_exited():
	$Panel/Settings/HoverOn.visible = false