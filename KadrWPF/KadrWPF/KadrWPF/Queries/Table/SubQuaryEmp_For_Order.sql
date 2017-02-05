max(decode(V.DW, :p_date{0}, to_char(V.HOURS)||decode(V.SIGN_HOLIDAY,0,'',' отгул')||decode(V.SIGN_ACTUAL_TIME,1,' факт'))) w_date{0}
to_char(sum(regexp_replace(w_date{0}, '[[:alpha:]]', '')),'FM99990.09')||nvl2(w_date{0},decode(REGEXP_INSTR(w_date{0},'отгул'),0,'',' (О)')||decode(REGEXP_INSTR(w_date{0},'факт'),0,'',' -Ф'),null) w_date{0}
w_date{0}
decode(w_date{0}, 0, 'Отгул', to_char(w_date{0})) w_date{0}