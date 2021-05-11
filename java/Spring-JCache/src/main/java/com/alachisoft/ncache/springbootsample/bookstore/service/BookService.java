package com.alachisoft.ncache.springbootsample.bookstore.service;

import com.alachisoft.ncache.springbootsample.bookstore.Book;
import com.alachisoft.ncache.springbootsample.bookstore.repository.BookRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.CachePut;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class BookService {

    @Autowired
    private BookRepository repo;

    private final String cacheName = "demoCache";

    public List<Book> listAll() {
        return repo.findAll();
    }

    @CachePut(value = cacheName, key = "#book.getISBN()")
    public void save(Book book) {
        repo.save(book);
    }

    @CachePut(value = cacheName, key = "#book.getISBN()")
    public void update(Book book) { repo.save(book); }

    @Cacheable(value = cacheName, key = "#id")
    public Book get(int id) {
        return repo.findById(id);
    }

    @Cacheable(value = cacheName, key = "#isbn")
    public Book findBookByIsbn(long isbn) {
        return repo.findBookByIsbn(isbn);
    }

    @CacheEvict(value = cacheName, allEntries = true)
    public void delete(int id) {
        repo.deleteById(id);
    }
}
