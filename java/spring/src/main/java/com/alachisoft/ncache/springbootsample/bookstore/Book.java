package com.alachisoft.ncache.springbootsample.bookstore;

import javax.persistence.*;
import java.io.Serializable;
import java.util.Date;

@Entity
@org.hibernate.annotations.NamedQuery(name = "Book.findBookByIsbn",
query = "select b from Book b where b.isbn = ?1")
public class Book implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    private long isbn;

    private String title;

    public Book(){}

    public long getIsbn() {
        return isbn;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setIsbn(long isbn) {
        this.isbn = isbn;
    }

    public Integer getId() {
        return id;
    }

    public Book(long isbn, String title, String subTitle,
                String author, Date publishedDate, String publisher,
                long pages, String description, String webURL) {
        this.isbn = isbn;
        this.title = title;
        this.subTitle = subTitle;
        this.author = author;
        this.publishedDate = publishedDate;
        this.publisher = publisher;
        this.pages = pages;
        this.description = description;
        this.webURL = webURL;
    }

    private String subTitle;

    private String author;

    private Date publishedDate;

    private String publisher;

    private long pages;

    @Column(length=3000)
    private String description;

    private String webURL;

    public long getISBN() {
        return isbn;
    }

    public void setISBN(long isbn) {
        this.isbn = isbn;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getSubTitle() {
        return subTitle;
    }

    public void setSubTitle(String subTitle) {
        this.subTitle = subTitle;
    }

    public String getAuthor() {
        return author;
    }

    public void setAuthor(String author) {
        this.author = author;
    }

    public Date getPublishedDate() {
        return publishedDate;
    }

    public void setPublishedDate(Date publishedDate) {
        this.publishedDate = publishedDate;
    }

    public String getPublisher() {
        return publisher;
    }

    public void setPublisher(String publisher) {
        this.publisher = publisher;
    }

    public long getPages() {
        return pages;
    }

    public void setPages(long pages) {
        this.pages = pages;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getWebURL() {
        return webURL;
    }

    public void setWebURL(String webURL) {
        this.webURL = webURL;
    }
}
