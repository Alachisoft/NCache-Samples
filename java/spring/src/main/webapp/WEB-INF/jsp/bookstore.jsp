<%@ page import="java.util.Map" %>
<%@ page import="com.alachisoft.ncache.springbootsample.bookstore.Book" %>
<%@page contentType="text/html" pageEncoding="UTF-8" %>
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
         <p>Following books are available in the book store.</p>
         <div class="container">
         <table class="table">
             <tbody>
                 <tr>
                     <th>ISBN</th>
                     <th>Book Title</th>
                     <th>Sub Title</th>
                     <th>Author</th>
                     <th>Publisher</th>
                 </tr>
                 <%
                     Object result = request.getAttribute("availableBooks");
                     Map books = (Map)result;
                     for (Object bookDetails : books.entrySet()) {
                         Map.Entry<Long, Book> entry = (Map.Entry<Long, Book>)bookDetails;
                         Long isbn = entry.getKey();
                         Book book = entry.getValue();
                 %>
                         <tr>
                             <td> <% out.println(isbn); %> </td>
                             <td> <% out.println(book.getTitle()); %> </td>
                             <td> <% out.println(book.getSubTitle()); %> </td>
                             <td> <% out.println(book.getAuthor()); %> </td>
                             <td> <% out.println(book.getPublisher()); %> </td>
                         </tr>
                 <%
                     }
                 %>
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