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

    @CachePut(value = "books", key = "#book.id")
    public Book save(Book book) {
        return repo.save(book);
    }

    @CachePut(value = "books", key = "#book.id")
    public Book update(Book book) { return repo.save(book); }

    @Cacheable(value = "books", key = "#id")
    public Book get(long id) {
        return repo.findById(id);
    }

    @CacheEvict(value = "books", allEntries = true)
    public void delete(int id) {
        repo.deleteById(id);
    }
}
