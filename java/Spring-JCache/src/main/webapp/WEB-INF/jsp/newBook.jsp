<%@ taglib prefix="form" uri="http://www.springframework.org/tags/form" %>
<%--
  Created by IntelliJ IDEA.
  User: muhammad_danyial
  Date: 07/05/2021
  Time: 4:15 PM
  To change this template use File | Settings | File Templates.
--%>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>Book Store</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
</head>
<body>
<h1>Create New Book</h1>
<br />
<div class="container">
    <div class="row">
        <div class="col-md-6 col-md-offset-3 ">
            <div class="panel panel-primary">
                <div class="panel-heading">Add Book</div>
                <div class="panel-body">
                    <form:form method="post" modelAttribute="book" action="/bookstore/save">
                        <form:hidden path="id" />
                        <fieldset class="form-group">
                            <form:label path="ISBN">ISBN</form:label>
                            <form:input path="ISBN" type="number" class="form-control"
                                        required="required" />
                            <form:errors path="ISBN" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="title">Title</form:label>
                            <form:input path="title" type="text" class="form-control"
                                        required="required" />
                            <form:errors path="title" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="subTitle">Sub Title</form:label>
                            <form:input path="subTitle" type="text" class="form-control"
                                        required="required" />
                            <form:errors path="subTitle" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="author">Author</form:label>
                            <form:input path="author" type="text" class="form-control"
                                        required="required" />
                            <form:errors path="author" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="publishedDate">Published Date</form:label>
                            <form:input path="publishedDate" type="text" class="form-control date"
                                        required="required" />
                            <form:errors path="publishedDate" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="publisher">Publisher</form:label>
                            <form:input path="publisher" type="text" class="form-control"
                                        required="required" />
                            <form:errors path="publisher" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="pages">Pages</form:label>
                            <form:input path="pages" type="number" class="form-control"
                                        required="required" />
                            <form:errors path="pages" cssClass="text-warning" />
                        </fieldset>
                        <fieldset class="form-group">
                            <form:label path="description">Description</form:label>
                            <form:input path="description" type="text" class="form-control"
                                        required="required" />
                            <form:errors path="description" cssClass="text-warning" />
                        </fieldset>

                        <fieldset class="form-group">
                            <form:label path="webURL">Web Url</form:label>
                            <form:input path="webURL" type="text" class="form-control"
                                        required="required" />
                            <form:errors path="webURL" cssClass="text-warning" />
                        </fieldset>

                        <button type="submit" class="btn btn-success">Save</button>
                    </form:form>
                </div>
            </div>
        </div>
    </div>
</div>
</body>
</html>
