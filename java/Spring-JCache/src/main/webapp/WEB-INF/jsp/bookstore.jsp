<%@page contentType="text/html" pageEncoding="UTF-8" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/fmt" prefix="fmt"%>
<%@taglib uri="http://www.springframework.org/tags/form" prefix="form"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Book Store</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
</head>
<body>
<br/><br/>
<div class="container">
    <div>
        <a type="button" class="btn btn-primary btn-md" href="${pageContext.request.contextPath}/bookstore/new">Add New Book</a>
    </div>
    <p>Following books are available in the book store.</p>
    <div class="container">
        <table class="table">
            <tbody>
            <tr>
                <th>ISBN</th>
                <th>Book Title</th>
                <th>Sub Title</th>
                <th>Pages</th>
                <th>Author</th>
                <th>Publisher</th>
                <th></th>
            </tr>
            <c:forEach items="${availableBooks}" var="book">
                <tr>
                    <td>${book.isbn}</td>
                    <td>${book.title}</td>
                    <td>${book.subTitle}</td>
                    <td>${book.pages}</td>
                    <td>${book.author}</td>
                    <td>${book.publisher}</td>
                    <td><a type="button" class="btn btn-success"
                           href="/bookstore/edit?id=${book.id}">Update</a>
                        <a type="button" class="btn btn-warning"
                           href="/bookstore/delete/?id=${book.id}">Delete</a></td>
                </tr>
            </c:forEach>
            </tbody>
        </table>
    </div>
    <p>
        <%
            Object error = request.getAttribute("errorMessage");
            if(error != null){
                out.println(error);
            }
        %>
    </p>

    <div class="container">
        <p>To get details of a specific book. Enter ISBN below: </p>
        <form method="post" action="${pageContext.request.contextPath}/bookstore">
            <div class="form-group">
                <div class="row">
                    <div class="col-sm-3">
                        <label for="isbn">ISBN:</label>
                        <input type="number" class="form-control" name="isbn" id="isbn">
                    </div>
                </div>
            </div>
            <input type="submit" class="btn btn-primary"/>
        </form>
    </div>
    <br/><br/>
</div>
</body>

</html>