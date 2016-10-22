# WordCollocation2016          

Features:     

1. A asp.net mvc site which makes collocation between words. Eg: what are any adjectives available for the word "abandon"? Ans: passionate       
  
2. All collocation data are stored in a MySQL database. The site asks a PHP page for it and the PHP page returns JSON which is then used by the ASP.Net-based site as if it was talking to the DB itself.        

3. The site's backend administration is also run by a PHP-based platform -- except for the Authentication & Authorization, which applies asp.net's "Simple Memebership".