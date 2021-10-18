package com.alachisoft.ncache.springbootsample.bookstore.repository;

import com.alachisoft.ncache.springbootsample.bookstore.Book;
import org.springframework.data.repository.CrudRepository;
import org.springframework.lang.NonNull;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface BookRepository extends CrudRepository<Book, Long> {
    Book findById(long id);
    @NonNull
    List<Book> findAll();
    void deleteById(long id);
}