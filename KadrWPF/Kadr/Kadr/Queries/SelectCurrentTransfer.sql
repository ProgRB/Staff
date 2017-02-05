where ((tab1.{2} = 1 or (tab1.{2}  = 1 and (tab1.{4} = (select max({4}) from {0}.{1} tr2 where tab1.{3} = tr2.{3}  and tr2.{5} = 0 )
or tab1.{4} = (select max({4}) from {0}.{1} tr2 where tab1.{3} = tr2.{3}  and tr2.{5} = 1 ))) and tab1.{6} = {7}) 
or (tab1.{9} = 3 and tab1.{10} = 0 and tab1.{4} = (select max({4}) from {0}.{1} tr2 where tab1.{3} = tr2.{3} and tr2.{9} = 3)))
and {3} = {8} and tab1.{11} = (select max({11}) from {0}.transfer tr3 where tr3.{3} = {8})
order by {3}