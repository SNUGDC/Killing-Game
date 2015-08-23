1. prise.png>prize.png 로 변경했습니다
2. 의자를 옮겨야 시체를 매달 수 있게 하면 좋을 것 같습니다
3. img_ 가 붙은 파일은 참고용 이미지입니다

**라이팅에 관하여 (20150823 수정)


li_ 가 붙은 것은 라이팅(add용) 이미지입니다
다음 조건에 따라 적용되는 라이팅이 다릅니다.
항상 화면에는 다음의 3가지 라이팅 이미지가 적용됩니다.
적용되는 3가지 라이팅 이미지가 어떤 것인지는 다음 조건에 따라 바뀝니다.

1. li_lamplight 
 : 무슨 조건이든 항상 적용됩니다(가장 윗 레이어)


2. li_lamp~ 
 : movchair_after 가 켜진 경우  : li_lamp_stand
 : movchair_fallen 가 켜진 경우 : li_lamp_fall
 : 둘 중 아무것도 적용되지 않은 경우 : li_lamp_normal


3. li_wide/mid/closed ~  : 두 가지 조건에 따라 결정됩니다.
  3-1. 커튼이 curtain_open 상태  
    - movchair_after 가 켜진 경우 : li_wide_hang_stand
    - movchair_fallen 가 켜진 경우 : li_wide_hang_fall
    - 둘 중 아무것도 켜지지 않은 경우 : li_wide_normal

  3-2. 커튼이 curtain_untied 상태  
    - movchair_after 가 켜진 경우 : li_mid_hang_stand
    - movchair_fallen 가 켜진 경우 : li_mid_hang_fall
    - 둘 중 아무것도 켜지지 않은 경우 : li_mid_normal

  3-3. 커튼이 curtain_closed 상태  
    - movchair_after 가 켜진 경우 : li_closed_hang_stand
    - movchair_fallen 가 켜진 경우 : li_closed_hang_fall
    - 둘 중 아무것도 켜지지 않은 경우 : li_closed_normal


 항상 수고하십니다. 감사합니다!