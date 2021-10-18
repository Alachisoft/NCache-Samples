<%@ page import="com.alachisoft.ncache.springbootsample.bookstore.Book" %>
<%@page contentType="text/html" pageEncoding="UTF-8" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Book Details</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
</head>
<body>
<div class="container">
    <p>Books details are as follows.</p>
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
            </tr>
            <%
                Object result = request.getAttribute("book");
                Book book = (Book) result;
            %>
            <tr>
                <td> <% out.println(book.getId()); %> </td>
                <td> <% out.println(book.getTitle()); %> </td>
                <td> <% out.println(book.getSubTitle()); %> </td>
                <td> <% out.println(book.getPages()); %> </td>
                <td> <% out.println(book.getAuthor()); %> </td>
                <td> <% out.println(book.getPublisher()); %> </td>
            </tr>
            </tbody>
        </table>
    </div>
</div>
</body>

</html>