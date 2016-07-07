<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <meta charset="UTF-8">
        <title></title>
    </head>
    <body>
        <h1>Server Installation</h1>
        <div>
            <form action="./check.php" method="POST">
                Database Host: <input type="text" name="dbhost" /><br />
                Database Name: <input type="text" name="dbname" /><br />
                Database Username: <input type="text" name="dbuser" /><br />
                Database Password: <input type="password" name="dbpwd" /><br />
                <input type="submit" value="Run Checks"/>
            </form>
        </div>
    </body>
</html>