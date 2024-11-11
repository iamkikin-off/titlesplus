extends Node

onready var titles_plus = get_node_or_null("/root/TitlesPlus")
onready var title_api = get_node_or_null("/root/TitleAPI")

onready var anims = $Anims

onready var PlayerData = get_node_or_null("/root/PlayerData")

var isOpen = false

func _sound():
	$Open.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	$MainMenu/Exit.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	$MainMenu/Presets.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	$MainMenu/Submit.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	
	$MainMenu/Submit.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])
	$MainMenu/Exit.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])
	$MainMenu/Presets.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])
	$Open.connect("pressed", GlobalAudio, "_play_sound", ["button_up"])

func _ready():
	_sound()

func _on_Exit_pressed():
	#$".".visible = false
	anims.play("CloseTitles")
	isOpen = false
	$Open.disabled = false

func _on_Submit_pressed():
	if titles_plus != null:
		var custom_title = $MainMenu/CustomTitle.text
		
		if str(custom_title).length() > 32:
			PlayerData._send_notification("Title can't exceed 32 chars!", 1)
		else:
			title_api.register_title(Network.STEAM_ID, custom_title)
			titles_plus.sendTitle(custom_title)
		
			PlayerData._send_notification("Title set to: " + custom_title)
			print("Submited!")


func _on_Open_pressed():
	if not isOpen:
		$Open.disabled = true
		isOpen = true
		anims.play("OpenTitles")

# This thing is very stupid, imma fix it later
func _on_LAMEDEV_pressed():
	var title = "[color=#e69d00][LAMEDEV][/color]"
	
	title_api.register_title(Network.STEAM_ID, title)
	titles_plus.sendTitle(title)

func _on_CONTRIBUTOR_pressed():
	var title = "[color=#008583][CONTRIBUTOR][/color]"
	
	title_api.register_title(Network.STEAM_ID, title)
	titles_plus.sendTitle(title)

func _on_TRANS_pressed():
	var title = "[color=#5BCEFA][T[color=#F5A9B8]R[/color][color=#ffffff]A[/color][color=#F5A9B8]N[/color]S][/color]"
	
	title_api.register_title(Network.STEAM_ID, title)
	titles_plus.sendTitle(title)


func _on_Preferences_pressed():
	$MainMenu/Presets.disabled = true
	$Anims.play("OpenPreferences")

func _on_ExitPreferences_pressed():
	$MainMenu/Presets.disabled = false
	$Anims.play("ClosePreferences")
