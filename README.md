# WordCollocation2016          

Features:     

1. A asp.net mvc site which makes collocation between words. Eg: what is an adjective available for the word "abandon"? Ans: passionate       
  
2. All collocation data are stored in a remote MySQL database. This site queries a PHP page for the data and the PHP page returns a serialized JSON string, which is then deserialized and consumed by this ASP.Net-based site as if it was talking to the DB itself.        

3. This site's backend is also administrated by a PHP-based platform -- except for the Authentication & Authorization, which applies asp.net's "Simple Membership".

4. wcadmin is the source code of the backend (yii2-based, short for "Admininstration of WordCollocation")
