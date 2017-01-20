select * from (
select rownum as num, last_name||chr(10)||first_name||' '||middle_name, per_num, year_birth, educ,
    subd, posit, razr_sal, dat_dsms, rsn_dsms, base_dsms, dat_ord, num_ord_dsms/*, dat_form, note*/
from (
    select distinct em.emp_last_name as last_name, em.emp_first_name as first_name, em.emp_middle_name as middle_name,
        em.per_num as per_num, to_char(em.emp_birth_date,'YYYY') as year_birth,
        (select distinct FIRST_VALUE(te.TE_name) OVER(ORDER BY TE.TYPE_EDU_PRIOR) 
        from {0}.TYPE_EDU TE join {0}.EDU ED on te.type_edu_id=ED.type_edu_id where ED.per_num = em.per_num) as educ,
        sub.code_subdiv as subd, pos.pos_name as posit,
        (case when NVL(a_d.classific,0)!=0 then a_d.classific end) as razr_sal,
        tr.date_transfer as dat_dsms, rd.reason_name as rsn_dsms, bd.base_doc_name as base_dsms, tr.tr_date_order as dat_ord,
        (case (chan_sign) when 1 then tr.tr_num_order||'/ê' else tr.tr_num_order||'/ó' end) as num_ord_dsms,
        trunc(NVL(tr.df_book_dismiss, TR.DF_BOOK_ORDER)) as dat_form,
        (case (chan_sign) when 1 then 'Êàíöåëÿğèÿ' else null end) as note
    from {0}.emp em
    join {0}.transfer tr on em.per_num = tr.per_num
    join {0}.account_data a_d on a_d.transfer_id = DECODE(TR.TYPE_TRANSFER_ID,3,TR.FROM_POSITION,TR.TRANSFER_ID)
    join {0}.subdiv sub on tr.subdiv_id = sub.subdiv_id
    join {0}.position pos on tr.pos_id = pos.pos_id
    join {0}.reason_dismiss rd on rd.reason_id = tr.reason_id
    join {0}.base_doc bd on BD.BASE_DOC_ID = TR.BASE_DOC_ID 
    where 
        /*((df_book_dismiss between :p_date_begin and :p_date_end and TR.DATE_TRANSFER between :p_date_begin and :p_date_end) 
            or (TR.DATE_TRANSFER between :p_date_begin and :p_date_end and TR.DF_BOOK_DISMISS > :p_date_end)) */
        /*TR.df_book_dismiss*/TR.TR_DATE_ORDER between :p_date_begin and :p_date_end
    order by /*dat_dsms*/dat_form, to_number(regexp_substr(num_ord_dsms,'\d*')))) 
where num > :p_num