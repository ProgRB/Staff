select  ad.account_data_id,AD.CHANGE_DATE ,AD.SALARY, AD.TAX_CODE, ad.classific, ad.harmful_addition, ad.harmful_addition_ADD, 
	ROUND(ad.comb_addition,2) comb_addition, tg.code_tariff_grid    
from {0}.account_data ad 
join {0}.TRANSFER T on AD.TRANSFER_ID=DECODE(T.TYPE_TRANSFER_ID, 3, T.FROM_POSITION, T.TRANSFER_ID)  
left join {0}.tariff_grid tg ON (ad.tariff_grid_id = tg.tariff_grid_id)
where T.TRANSFER_ID = :p_transfer_id 
order by AD.CHANGE_DATE desc