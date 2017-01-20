select D.DOMAIN_ACCOUNT_ID, D.NAME_AD, D.CODE_AD
from {0}.DOMAIN_ACCOUNT D
join {0}.TYPE_AD TD on D.TYPE_AD_ID = TD.TYPE_AD_ID
where TD.SYSNAME_TYPE_AD = '{1}'
order by CODE_AD
/*1-наименование признака*/