declare
begin
    open :c1 for select * from apstaff.gr_work where gr_work_id=:p_gr_work_id;              
end;