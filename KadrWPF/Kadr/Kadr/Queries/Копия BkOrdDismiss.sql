select * from (
select rownum as num, last_name||chr(10)||first_name||' '||middle_name, per_num, year_birth, educ,
    subd, posit, razr_sal, dat_dsms, rsn_dsms, base_dsms, dat_ord, num_ord_dsms/*, dat_form, note*/
from (
    select distinct em.emp_last_name as last_name, em.emp_first_name as first_name, em.emp_middle_name as middle_name,
        em.per_num as per_num, to_char(em.emp_birth_date,'YYYY') as year_birth,
        (select te.TE_name from {0}.type_edu te where te.type_edu_id=(select max(type_edu_id) from {0}.edu where per_num = em.per_num)) as educ,
        sub.code_subdiv as subd, pos.pos_name as posit,
        (case
        when a_d.classific is null then 'Ò.êîıô '|| a_d.salary
        else 'Ğàçğÿä '||a_d.classific 
        end) as razr_sal,
        tr.date_transfer as dat_dsms, rd.reason_name as rsn_dsms, bd.base_doc_name as base_dsms, tr.tr_date_order as dat_ord,
        (case (chan_sign) when 1 then tr.tr_num_order||'/ê' else tr.tr_num_order||'/ó' end) as num_ord_dsms,
        trunc(tr.df_book_dismiss) as dat_form,
        (case (chan_sign) when 1 then 'Êàíöåëÿğèÿ' else null end) as note
    from {0}.emp em, {0}.transfer tr, {0}.account_data a_d, {0}.subdiv sub, {0}.position pos, {0}.reason_dismiss rd, {0}.base_doc bd 
    where tr.transfer_id = a_d.transfer_id(+) and 
        em.per_num = tr.per_num and tr.subdiv_id = sub.subdiv_id and tr.pos_id = pos.pos_id and rd.reason_id = tr.reason_id
        and BD.BASE_DOC_ID = TR.BASE_DOC_ID and 
        /*((df_book_dismiss between :p_date_begin and :p_date_end and TR.DATE_TRANSFER between :p_date_begin and :p_date_end) 
            or (TR.DATE_TRANSFER between :p_date_begin and :p_date_end and TR.DF_BOOK_DISMISS > :p_date_end)) */
        TR.TR_DATE_ORDER between :p_date_begin and :p_date_end
    order by /*dat_dsms*/dat_form, to_number(regexp_substr(num_ord_dsms,'\d*')))) 
where num > :p_num