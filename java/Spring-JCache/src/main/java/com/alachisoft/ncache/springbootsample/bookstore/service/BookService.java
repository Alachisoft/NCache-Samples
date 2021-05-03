package com.alachisoft.ncache.springbootsample.bookstore.service;

import com.alachisoft.ncache.springbootsample.bookstore.Book;
import com.fasterxml.jackson.databind.ObjectMapper;
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
    private Map<Long, Book> _books;

    @Cacheable(value = "demoCache")
    public Book getBookByISBN(long isbn) {
        if(_books == null || _books.size() == 0)
            loadBooksFromFile();
        System.out.println("Searching by isbn  : " + isbn);
        return _books.get(isbn);
    }

    private void loadBooksFromFile(){
        _books = new HashMap<Long, Book>();
        try {
            ObjectMapper mapper = new ObjectMapper();
            URL resource = getClass().getClassLoader().getResource("books.json");
            if (resource != null) {
                File booksFile = new File(resource.toURI());
                if (booksFile.exists()) {
                    Map booksMap= mapper.readValue(booksFile, Map.class);
                    List tempBooks = (List) booksMap.get("books");
                    if(tempBooks != null && tempBooks.size() > 0)
                    {
                        for (Object temp : tempBooks){
                            Book book = mapper.convertValue(temp, Book.class);
                            if (book != null) {
                                _books.put(book.getISBN(), book);
                            }
                        }
                    }
                }
            }
        } catch (URISyntaxException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public Map<Long, Book> getBookTitles() {
        if(_books == null || _books.size() == 0)
            loadBooksFromFile();

        Map<Long, Book> result = new HashMap<>();

        for (Book value: _books.values()) {
            if(value != null){
                result.put(value.getISBN(), value);
            }
        }
        return result;
    }
}
