<%--
  Created by IntelliJ IDEA.
  User: saad_farooq
  Date: 14/09/2021
  Time: 6:06 PM
  To change this template use File | Settings | File Templates.
--%>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>Error</title>
</head>
<body>
    <div class="container">
        <div class="flex centered">
            <span style="font-size: 3rem;">There was an error.</span>
            <span style="font-size: 1.5rem;"><a href="/bookstore">Go back to home page.</a></span>
        </div>
    </div>

    <style>
        body {
            margin: 0 auto;
            left: 0;
            top: 0;
        }

        div.container {
            max-width: 1200px;
            margin: 0 auto;
            height: 100vh;
        }

        div.flex.centered {
            display: flex;
            flex-direction: column;
            text-align: center;
            justify-content: center;
            height: 100%;
        }
    </style>
</body>
</html>
