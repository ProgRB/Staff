select 
pw_firm, 
(to_char(pw_date_start,'MM.YYYY')||' - '||to_char(pw_date_end,'MM.YYYY')) ||' - '|| pw_name_pos as prev 
from {1}.prev_work where per_num = to_number('{0}') order by pw_date_start