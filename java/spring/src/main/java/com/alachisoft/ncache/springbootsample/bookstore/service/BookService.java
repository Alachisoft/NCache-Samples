package com.alachisoft.ncache.springbootsample.bookstore.service;

import com.alachisoft.ncache.springbootsample.bookstore.Book;
import com.alachisoft.ncache.springbootsample.bookstore.repository.BookRepository;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.CachePut;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.stereotype.Service;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

@Service
public class BookService {
    private List<Book> _books;

    @Autowired
    private BookRepository repo;

    public List<Book> listAll() {
        return repo.findAll();
    }

    @CachePut(value = "books", key = "#book.getISBN()")
    public void save(Book book) {
        repo.save(book);
    }

    @CachePut(value = "books", key = "#book.getISBN()")
    public void update(Book book) { repo.save(book); }

    @Cacheable(value = "books", key = "#id")
    public Book get(int id) {
        return repo.findById(id);
    }

    @Cacheable(value = "books", key = "#isbn")
    public Book findBookByIsbn(long isbn) {
        Book book = repo.findBookByIsbn(isbn);
        return book;
    }

    @CacheEvict(value = "books", allEntries = true)
    public void delete(int id) {
        repo.deleteById(id);
    }
}
