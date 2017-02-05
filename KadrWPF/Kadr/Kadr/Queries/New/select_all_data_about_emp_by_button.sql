select SUBDIV.SUBDIV_NAME as "Подразделение", emp.per_num as "Табельный номер",emp.emp_last_name as "Фамилия",emp.emp_first_name as "Имя",emp.emp_middle_name as "Отчество",
    transfer.date_transfer as "Дата перевода",  account_data.salary as "Оклад",
    position.pos_name as "Должность",account_data.classific as "Разряд",
      transfer.tr_date_order as "Дата окончания договора"
from {0}.emp left  join {0}.transfer on (emp.per_num=transfer.per_num)
    left  join {0}.account_data on (transfer.transfer_id=account_data.transfer_id)
    left outer join   {0}.position on (TRANSFER.POS_ID=POSITION.POS_ID)
    left outer join {0}.subdiv on (SUBDIV.SUBDIV_ID =TRANSFER.SUBDIV_ID)
where  position.pos_id=to_number('{1}')
order by transfer.sign_cur_work, emp.emp_last_name,emp.emp_first_name,position.pos_actual_sign
