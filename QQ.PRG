* REQUEST HB_GT_WVW

function Main()
   cls

*  WVW_nOpenWindow('outra janela', 10,20,15,40)

   Wvw_SetMouseMove(, .t. )

   Do While .T.
      @ 02,10 Say MRow()
      @ 03,10 Say MCol()

      nTecla := Inkey(0.01)
      If nTecla = 0
         Loop
      EndIf
      
      @ 04,10 Say nTecla


      If Lastkey() = 27
         exit
      endif
   EndDo

*  WVW_lCloseWindow()
   Wait
Return
