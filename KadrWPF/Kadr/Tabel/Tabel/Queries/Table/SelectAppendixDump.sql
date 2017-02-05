select * from 
(
	select  
		:p_npp as npp, :p_kdvpodr as kdvpodr, :p_code_subdiv as podr, 
		case when :p_code_degree != '04' 
			then :p_code_degree 
			else 
				case when :p_code_pos1 != :p_code_pos2 and :p_code_pos1 != :p_code_pos3 
					then '04' 
					else
						case when :p_code_pos1 = '2' 
							then '41' 
							else 
								case when :p_code_pos1 = '3' 
									then '42' 
									else '43'
								end
						end  
				end 
		end as kat, 
		s101, s102, s106, 0 as s111, s124, s231, s112, s211, 
		s210, s222, s121, s125, s237, s536, s529, 0 as ADAD, s531D, s226, s210P, s215, s531U, s533, /*0 as SLXR*/s544, s124D, 0 as SUBV, s124All, 
		s102D, s222D, sAll, 
		/* ��������������� ����������� = ���������� ���� � ������ / ���������� ���� � ������ */
		case when V.sAll > 0 then round(V.sAll /:p_days, 2) else null end as srsp,
		/* �� ������ ����� */
		/*case when V.sAll > 0 
			then {0}.count_emp_degree(:p_date_begin, :p_date_end, :p_subdiv_id, :p_code_degree, :p_code_posC, :p_code_f_o1, :p_code_f_o2, '�', '�') 
			else null end as EmpCount,*/
		(select decode(C_E,0,null,C_E) from (select {0}.count_emp_degree(:p_date_begin, :p_date_end, :p_subdiv_id, :p_code_degree, :p_code_posC, :p_code_f_o1, :p_code_f_o2, '�', '�') C_E from dual))
			as EmpCount,        
		/* �� ������ � ��� ����� ������ */
		/*case when V.sAll > 0 
			then {0}.count_emp_degree(:p_date_begin, :p_date_end, :p_subdiv_id, :p_code_degree, :p_code_posC, :p_code_f_o1, :p_code_f_o2, '�', '�') 
			else null end as EmpCountW, */
		(select decode(C_E,0,null,C_E) from (select {0}.count_emp_degree(:p_date_begin, :p_date_end, :p_subdiv_id, :p_code_degree, :p_code_posC, :p_code_f_o1, :p_code_f_o2, '�', '�') C_E from dual))
			as EmpCountW,   
		case when V.sAll > 0 then round((nvl(V.s101,0) + nvl(V.s102,0) + nvl(V.s222,0)) / V.s102D, 2) else null end as prdn
	from 
	(
	select 
		sum(CASE when PAY_TYPE_ID = '101' or (CODE_DEGREE in ('01', '02') and PAY_TYPE_ID in ('510')) THEN round(TIME,0) ELSE null END) as s101,
		sum(CASE when PAY_TYPE_ID = '106' THEN round(TIME,0) ELSE null END) as s106,
		sum(CASE when PAY_TYPE_ID = '102' or (CODE_DEGREE not in ('01', '02') and PAY_TYPE_ID in ('510')) THEN trunc(TIME,0) ELSE null END) as s102,
		sum(CASE when PAY_TYPE_ID = '124' THEN round(TIME,0) ELSE null END) as s124,
		sum(CASE when PAY_TYPE_ID in ('112','114') THEN round(TIME,0) ELSE null END) as s112,
		sum(CASE when PAY_TYPE_ID in ('131','231') THEN round(TIME,0) ELSE null END) as s231,
		sum(CASE when PAY_TYPE_ID = '113' THEN round(TIME,0) ELSE null END) as s113,
		sum(CASE when PAY_TYPE_ID in ('222','622') THEN trunc(TIME,0) ELSE null END) as s222,
		sum(CASE when PAY_TYPE_ID = '121' THEN round(TIME,0) ELSE null END) as s121,
		sum(CASE when PAY_TYPE_ID = '125' THEN round(TIME,0) ELSE null END) as s125,
		sum(CASE when PAY_TYPE_ID in ('531', '543', '544') THEN trunc(TIME,0) ELSE null END) as s531,
		sum(CASE when PAY_TYPE_ID = '211' THEN round(TIME,0) ELSE null END) as s211,
		sum(CASE when PAY_TYPE_ID in ('210','214','542') THEN round(TIME,0) ELSE null END) as s210,
		sum(CASE when PAY_TYPE_ID = '111' THEN round(TIME,0) ELSE null END) as s111,
		sum(CASE when PAY_TYPE_ID = '237' THEN round(TIME,0) ELSE null END) as s237,
		sum(CASE when PAY_TYPE_ID = '536' THEN round(TIME,0) ELSE null END) as s536,
		sum(CASE when PAY_TYPE_ID in ('529','542D') THEN round(TIME,0) ELSE null END) as s529,
		sum(CASE when PAY_TYPE_ID = '226' THEN round(TIME,0) ELSE null END) as s226,
		sum(CASE when PAY_TYPE_ID in ('531D', '543D') THEN round(TIME,0) ELSE null END) as s531D,
		sum(CASE when PAY_TYPE_ID = '215' THEN round(TIME,0) ELSE null END) as s215,
		sum(CASE when PAY_TYPE_ID = '531U' THEN round(TIME,0) ELSE null END) as s531U,
		sum(CASE when PAY_TYPE_ID = '210P' THEN round(TIME,0) ELSE null END) as s210P,
		sum(CASE when PAY_TYPE_ID in ('533','539') THEN round(TIME,0) ELSE null END) as s533,
		sum(CASE when PAY_TYPE_ID = '544D' THEN round(TIME,0) ELSE null END) as s544,
		/* �������, �������� � ����������� ��� */
		sum(CASE when PAY_TYPE_ID = '124D' THEN round(TIME,0) ELSE null END) as s124D,
		/* ����� ������ */
		sum(CASE when PAY_TYPE_ID = '124All' THEN round(TIME,0) ELSE null END) as s124All,
		/* ��� ������ ����� */
		sum(CASE when (:p_subdiv_id = 11 and CODE_DEGREE != '04' and PAY_TYPE_ID = '540_') or
			(:p_subdiv_id = 11 and CODE_DEGREE = '04' and PAY_TYPE_ID in ('540_','222D')) or 
			(:p_subdiv_id != 11 and PAY_TYPE_ID in ('540_','222D')) THEN round(TIME,0) ELSE null END) as s102D,
		/* ��� ������ � ��� ����� ������������ */
		sum(CASE when PAY_TYPE_ID = '222D' THEN round(TIME,0) ELSE null END) as s222D,
		/* ����� ���� � ������ */
		sum(CASE when (:p_subdiv_id = 11 and CODE_DEGREE != '04' and PAY_TYPE_ID in ('540_','124All')) or 
			(:p_subdiv_id = 11 and CODE_DEGREE = '04' and PAY_TYPE_ID in ('540_','222D','124All')) or 
			(:p_subdiv_id != 11 and PAY_TYPE_ID in ('540_','222D', '124All')) THEN round(TIME,0) ELSE null END) as sAll
	from 
	(
		select PAY_TYPE_ID,TIME, CODE_POS, CODE_DEGREE from {0}.TEMP_SALARY TS 
		where TS.TEMP_SALARY_ID = :p_temp_salary_id and TS.CODE_DEGREE = :p_code_degree and TS.SIGN_APPENDIX = 1 
			and TS.SIGN_COMB = 0 and TS.CODE_F_O in (:p_code_f_o1, :p_code_f_o2)
			and TS.CODE_POS in (:p_code_pos1, :p_code_pos2, :p_code_pos3)
	)) V
) V1
where V1.sAll > 0 or V1.EmpCount > 0
