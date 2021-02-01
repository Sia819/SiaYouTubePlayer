; <COMPILER: v1.1.26.01>
SetBatchLines, -1		;한줄 읽는데 대기시간은 0
global wb				; 글로벌 변수 wb
#SingleInstance off		; 스크립트가 실행중일때 중복실행가능
DetectHiddenWindows, on ; 이제 스크립트는 보이지않는 창을 감지합니다

Gui, -DPIScale			; DPI 스케일링 비활성화
Gui, start: -DPIScale
Gui, 2: -DPIScale
Gui, 3: -DPIScale

Gui, start: add, Listbox, x0 y0 w300 h300 vLog, start program...	; 로그
Gui, Start: Show, W300 h300, Start process
Gui, Start:DEfault



GuiControl,,log, Open IE...		; 로그추가
wb := ComObjCreate("InternetExplorer.Application")
Guicontrol,,log, Open IE Successful

Guicontrol,,log, IE toolbar Setting...
wb.toolbar := false
Guicontrol,,log, IE toolbar Setting Successful

Guicontrol,,log, IE Statusbar Setting...
wb.StatusBar := false
Guicontrol,,log, IE Statusbar Setting Successful

Guicontrol,,log, IE AddressBar Setting...
wb.AddressBar := false
Guicontrol,,log, IE AddressBar Setting Successful

Guicontrol,,log, All ready for starting program
wb.MenuBar := false
sleep,100
gui, Start: hide
Gui, 1:Default
;gui, add, Edit, x0 y0 w350 h18 vUrl, 주소 입력
gui, add, Edit, x0 y0 w350 h18 vUrl, https://www.youtube.com/watch?v=-tKVN2mAKRI&list=RD-tKVN2mAKRI
gui, add, button, x350 y0 w50 h19 gBtn, 확인
gui, add, Slider, x0 y25 w200 h20 Range1-3 vSize, vSize
gui, add, Checkbox, x220 y25 w900 h20 gNoCap vNoCap, 테두리 삭제 (Ctrl + Shift + A)
gui, Show, w400 h50 ,AlwaysOnYT Settings
return


Btn:
gui, submit, nohide
IfInString,Url,https
{
	gui, 2: Default
	gui, 1: submit, nohide
	gui, 2:+alwaysontop
	gui, 2:add, text, x0 y0 w500 h30 vLoadText, 동영상을 불러오는 중..
	;gui, 2:Add, GroupBox, x0 y0 w400 h300, Geographic Criteria
	gui, 2:  +Resize 
	if (Size = 1)
	{
		gui, 2:show, w496 h239,AlwaysOnYT
		;WinMove, % "Ahk_id " wb.hWnd ,, -79,-126, 600, 500
		WinMove, % "Ahk_id " wb.hWnd ,, -79, 10, 600, 500
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
	Set_Parent_by_id(wb.hWnd)
	ShowYT(Url)
}
else
MSgbox, 올바른 주소를 입력해 주세요
return


^+A::
gui, submit, nohide
if (Nocap = 0)
	GuiControl,,Nocap,1
else if (Nocap = 1)
	guicontrol,,Nocap,0
else
	Msgbox, Error
return

NoCap:
gui, submit, nohide
if (Nocap = 1)
{
	gui, 2:-caption
	WinGetPos,vx,vy,vw,vh,AlwaysOnYT
	vw -= 6
	Winmove, AlwaysOnYT,,%vx%,%vy%,%vw%,% vh - 28
}
else if (Nocap = 0)
{
	gui, 2:+caption
	WinGetPos,vx,vy,vw,vh,AlwaysOnYT
	vw += 6
	Winmove, AlwaysOnYT,,%vx%,%vy%,%vw%,% vh + 28
}
else
Msgbox, Error
return


GuiClose:
Gui, 2:Destroy
gui, 3: +AlwaysOnTop -Caption +Border
gui, 3:color, FFFFFF
Gui, 3:add, text, x0 y120 h290 w300 gHidden vHid center,노력한다고 모두 성공 할 수 없다.`n`n하지만 성공한 사람은 모두 노력한 사람들이다.`n`n( 3 )
;gui, 3:show, w300 h300, Quit message
gui, 3:Default
wb.quit()
;sleep, 1000
;guicontrol,,hid,노력한다고 모두 성공 할 수 없다.`n`n하지만 성공한 사람은 모두 노력한 사람들이다.`n`n( 2 )
;sleep, 1000
;guicontrol,,hid,노력한다고 모두 성공 할 수 없다.`n`n하지만 성공한 사람은 모두 노력한 사람들이다.`n`n( 1 )
;sleep, 1000
ExitApp


2GuiClose:
guicontrol,,loadText,동영상 종료 중
wb.quit()
Msgbox,262144,Close Video,Wait for closing video... ,3
reload
return


Hidden:
a++
if (a = 10)
Msgbox,262144,Hello,프로그램 이용에 감사드립니다 :)
return


Set_Parent_by_id(Window_ID)
{
	Gui, +LastFound
	Return DllCall("SetParent", "uint", Window_ID, "uint", WinExist())
}
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
