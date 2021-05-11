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
        return repo.findBookByIsbn(isbn);
    }

    @CacheEvict(value = "books", allEntries = true)
    public void delete(int id) {
        repo.deleteById(id);
    }
}
