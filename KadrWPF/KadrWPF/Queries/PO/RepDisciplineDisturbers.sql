select EXTRACT(MONTH FROM VI.DETENTION_DATE) DET_MONTH, VR.VIOLATION_ID, SD.SIGN_DETENTION_NAME
from {0}.VIOLATION VI
join {0}.VIOLATOR VR on VI.VIOLATION_ID = VR.VIOLATION_ID
join {0}.SIGN_DETENTION SD on VI.SIGN_DETENTION_ID = SD.SIGN_DETENTION_ID
join {0}.REASON_DETENTION RD on (VI.REASON_DETENTION_ID = RD.REASON_DETENTION_ID)
where VI.DETENTION_DATE between :p_BEGIN_PERIOD and :p_END_PERIOD and RD.SIGN_ALC_INTOX = 1