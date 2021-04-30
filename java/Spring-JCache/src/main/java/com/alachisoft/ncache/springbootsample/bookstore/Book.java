package com.alachisoft.ncache.springbootsample.bookstore;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.io.Serializable;
import java.util.Date;

public class Book implements Serializable {
    @JsonProperty("isbn")
    private long _isbn;
    @JsonProperty("title")
    private String _title;
    @JsonProperty("subtitle")
    private String _subTitle;
    @JsonProperty("author")
    private String _author;
    @JsonProperty("published")
    private Date _publishedDate;
    @JsonProperty("publisher")
    private String _publisher;
    @JsonProperty("pages")
    private long _pages;
    @JsonProperty("description")
    private String _description;
    @JsonProperty("website")
    private String _webURL;

    public long getISBN() {
        return _isbn;
    }

    public void setISBN(long _isbn) {
        this._isbn = _isbn;
    }

    public String getTitle() {
        return _title;
    }

    public void setTitle(String _title) {
        this._title = _title;
    }

    public String getSubTitle() {
        return _subTitle;
    }

    public void setSubTitle(String _subTitle) {
        this._subTitle = _subTitle;
    }

    public String getAuthor() {
        return _author;
    }

    public void setAuthor(String _author) {
        this._author = _author;
    }

    public Date getPublishedDate() {
        return _publishedDate;
    }

    public void setPublishedDate(Date _publishedDate) {
        this._publishedDate = _publishedDate;
    }

    public String getPublisher() {
        return _publisher;
    }

    public void setPublisher(String _publisher) {
        this._publisher = _publisher;
    }

    public long getPages() {
        return _pages;
    }

    public void setPages(long _pages) {
        this._pages = _pages;
    }

    public String getDescription() {
        return _description;
    }

    public void setDescription(String _description) {
        this._description = _description;
    }

    public String getWebURL() {
        return _webURL;
    }

    public void setWebURL(String _webURL) {
        this._webURL = _webURL;
    }
}
