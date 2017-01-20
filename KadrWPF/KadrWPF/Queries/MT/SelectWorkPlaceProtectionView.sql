select
	CODE_PROTECTION,
	NAME_PROTECTION,
	TYPE_PROTECTION_NAME,
	PERIOD_FOR_USE
from
	apstaff.work_place_protection
	join apstaff.individ_protection using (individ_protection_id)
	join apstaff.type_individ_protection using (type_individ_protection_id)
where
	work_place_id=:p_work_place_id
