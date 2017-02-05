select * from (
    select rownum as num, tab1.* from 
    (
        select last_name, first_name, middle_name, per_num, otd_before, pos_before, b_tar_koef, razr, kateg,
            charac, comb, date_tr, otd_after, pos_after, a_t_k, a_razr, a_kateg, a_charac, a_comb, num_order,
            tr_date_order, date_form
        from (
            select distinct em.emp_last_name as last_name, em.emp_first_name as first_name, em.emp_middle_name as middle_name,
                em.per_num as per_num, 
                (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = TR.SUBDIV_ID) as otd_before, 
                (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = TR.POS_ID) as pos_before,
                a_d.salary as b_tar_koef, a_d.classific as razr, tr.degree_id as kateg, 
                (SELECT TTT.TYPE_TERM_TRANSFER_NAME FROM {0}.TYPE_TERM_TRANSFER TTT
                WHERE TTT.TYPE_TERM_TRANSFER_ID = NVL(tr.CHAR_TRANSFER_ID, tr.char_work_id)) as charac,
                a_d.COMB_ADDITION as comb, tr.date_transfer as date_tr, 
                (SELECT S.CODE_SUBDIV FROM {0}.SUBDIV S WHERE S.SUBDIV_ID = TR1.SUBDIV_ID) as otd_after,
                (SELECT P.POS_NAME FROM {0}.POSITION P WHERE P.POS_ID = TR1.POS_ID) as pos_after, 
                a_d1.salary as a_t_k, a_d1.classific as a_razr, tr1.degree_id as a_kateg,
                (SELECT TTT.TYPE_TERM_TRANSFER_NAME FROM {0}.TYPE_TERM_TRANSFER TTT
                WHERE TTT.TYPE_TERM_TRANSFER_ID = NVL(tr1.CHAR_TRANSFER_ID, tr1.char_work_id)) as a_charac,
                a_d1.COMB_ADDITION as a_comb,
                (case (tr1.chan_sign)/**/
                    when 0 then tr1.tr_num_order||'/Ï'
                    when 1 then tr1.tr_num_order||'/Ê'
                    else tr1.tr_num_order
                end) as num_order,
                tr1.tr_date_order as tr_date_order, tr1.df_book_order as date_form
            from {0}.transfer tr 
            join {0}.emp em on em.per_num = tr.per_num
            join {0}.transfer tr1 on tr.transfer_id = tr1.from_position 
            left join {0}.account_data a_d on tr.transfer_id = a_d.transfer_id
            left join {0}.account_data a_d1 on tr1.transfer_id = a_d1.transfer_id 
            where tr1.type_transfer_id = 2 and to_char(tr1.DATE_TRANSFER,'YYYY') = :p_YEAR
            order by to_number(regexp_substr(num_order,'\d*')),  date_form
        )
  ) tab1 where substr(otd_after,1,1)!='5' and regexp_substr(num_order,'^([^à-ÿÀ-ß])') is not null
) 
where num>:p_NUMBER_ORDER