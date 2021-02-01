gui, add, Edit, x0 y0 w350 h18 vUrl, https://www.youtube.com/watch?v=-tKVN2mAKRI&list=RD-tKVN2mAKRI
gui, add, button, x350 y0 w50 h19 gBtn, 확인
gui, add, button, x350 y0 w50 h19, 확인
gui, add, Slider, x0 y25 w200 h20 Range1-3 vSize, vSize
;gui, add, Checkbox, x220 y25 w900 h20 gNoCap vNoCap, 테두리 삭제 (Ctrl + Shift + A)
gui, add, Checkbox, x220 y25 w900 h20 vNoCap, 테두리 삭제 (Ctrl + Shift + A)
Gui  +Resize 
gui, Show, w400 h50 AutoSize Center,AlwaysOnYT Settings

global wb
wb := ComObjCreate("InternetExplorer.Application")
wb.toolbar := false
wb.StatusBar := false
wb.AddressBar := false
wb.MenuBar := false

return

Btn:
gui, submit, nohide
IfInString,Url,https
{
	gui, 2: Default
	gui, 1: submit, nohide
	gui, 2:+alwaysontop
	gui, 2:add, text, x0 y0 w500 h30 vLoadText, 동영상을 불러오는 중..
	gui, 2:  +Resize 
	;gui, 2:Add, GroupBox, x0 y0 w400 h300, Geographic Criteria
	if (Size = 1)
	{
		gui, 2:show, w496 h239,AlwaysOnYT
		;WinMove, % "Ahk_id " wb.hWnd ,, -79,-126, 600, 500
		;WinMove, % "Ahk_id " wb.hWnd ,, -79, 10, 600, 500
		WinActivate, AlwaysOnYT
	}
	else if (Size = 2)
	{
		gui, 2:show, w639 h360,AlwaysOnYT
		WinMove, % "Ahk_id " wb.hWnd ,, -10,-127, 1100, 500
		WinActivate, AlwaysOnYT
	}
	else if (Size = 3)
	{
		gui, 2:show, w854 h479,AlwaysOnYT
		WinMove, % "Ahk_id " wb.hWnd ,, -102,-127, 1500, 1000
		WinActivate, AlwaysOnYT
	}
	else
	{
		Msgbox, Error: C0de A
	}
	;Set_Parent_by_id(wb.hWnd)
	ShowYT(Url)
}
else
MSgbox, 올바른 주소를 입력해 주세요
return

ShowYT(Url)
{
	wb.Navigate(url)
	While wb.readyState != 4 || wb.document.readyState != "complete" || wb.busy
	Sleep, 10
	wb.Visible := true
	wb.Silent := true
	WinActivate, AlwaysOnYT
}

F5::
ExitApp